using Bel_Souvenirs.MiddleWare;
using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CartService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<EmailService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});


var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

//    string[] roleNames = { "Admin", "User" };
//    foreach (var roleName in roleNames)
//    {
//        var roleExist = await roleManager.RoleExistsAsync(roleName);
//        if (!roleExist)
//        {
//            await roleManager.CreateAsync(new IdentityRole(roleName));
//        }
//    }

//    var adminUser = await userManager.FindByEmailAsync("admin@example.com");
//    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
//    {
//        await userManager.AddToRoleAsync(adminUser, "Admin");
//    }
//}


//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    SeedData.InitializeAsync(services).GetAwaiter().GetResult();
//}




if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<UserInfoMiddleware>();


app.MapControllerRoute(
    name: "cart",
    pattern: "cart/{action=Index}",
    defaults: new { controller = "Cart" });

app.MapControllerRoute(
    name: "catalog",
    pattern: "catalog/{action=Index}/{id?}",
    defaults: new {controller = "Catalog" });

app.MapControllerRoute(
    name: "productDetails",
    pattern: "products/{id}",
    defaults: new { controller = "Product", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
