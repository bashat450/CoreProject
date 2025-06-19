using AdminDyanamoEnterprises.Repository;
using AdminDyanamoEnterprises.Repository;
using AdminDyanamoEnterprises.Repository;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IHomeRepository, HomeRepository>();
builder.Services.AddTransient<IMasterRepository, MasterRepository>();
builder.Services.AddTransient<IMaterialRepository, MaterialRepository>();
builder.Services.AddTransient<IFabricRepository, FabricRepository>();
builder.Services.AddTransient<IColorRepository, ColorRepository>();
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 20;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
}
);
//I am Narayan 
var app = builder.Build();
//I am BASHAt
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.UseNotyf();
app.Run();