using SKLEP_INTERNETOWY.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SKLEP_INTERNETOWY;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SklepInternetowyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<SklepInternetowyDbContext>();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }

    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database." + DateTime.Now.ToString());
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Dodaj now¹ trasê dla localhost
    endpoints.MapControllerRoute(
        name: "home",
        pattern: "",
        defaults: new { controller = "SklepInternetowy", action = "Index" });

    endpoints.MapControllerRoute(
    name: "order",
    pattern: "",
    defaults: new { controller = "Order", action = "Index" });

    endpoints.MapControllerRoute(
    name: "adminOrders",
    pattern: "Order/Orders",
    defaults: new { controller = "Order", action = "Orders" });


});


app.Run();
