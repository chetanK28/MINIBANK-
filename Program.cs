using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MINIBANK.DBContext;
using MINIBANK.Repositories.Implementation;
using MINIBANK.Repositories.Interface;
using MINIBANK.Services.Implementation;
using MINIBANK.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Fetch connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("MyDatabase");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string 'MyDatabase' is not configured.");
}

// Add services to the container
builder.Services.AddControllersWithViews();

// Register dependencies for DI
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUsers, UsersRepository>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuth, AuthRepo>();

// Configure session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(session =>
{
    session.Cookie.HttpOnly = true;
    session.Cookie.IsEssential = true;
});

// Configure authentication and authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Redirect to login page if not authenticated
        options.LogoutPath = "/Auth/Logout"; // Redirect to logout page on sign-out
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Optional: Access Denied Page
        options.Cookie.Name = "UserLoginCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                // Redirect to User Dashboard if already logged in
                if (context.Request.Path == "/Auth/Login" && context.HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    context.Response.Redirect("/BankAccount/UserDashboard");
                }
                else
                {
                    context.Response.Redirect(context.RedirectUri);
                }
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

// Configure EF Core with MySQL
builder.Services.AddDbContext<Context>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 2))));

var app = builder.Build();

// Configure middleware for the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Detailed errors in development
}
else
{
    app.UseExceptionHandler("/Home/Error"); // General error handler for production
}

app.UseStaticFiles(); // Serve static files from wwwroot

app.UseRouting(); // Enable endpoint routing

// Enable authentication and authorization
app.UseAuthentication(); // Use cookie authentication
app.UseAuthorization(); // Use authorization middleware

app.UseSession(); // Enable session handling

// Map default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the application
app.Run();
