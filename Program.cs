var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services
    .AddControllers()
    .AddXmlSerializerFormatters();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllerRoute(
    name: "V2ServiceIndex",
    pattern: "api/v2",
    defaults: new { controller = "V2Api", action = "Index" });

app.MapControllerRoute(
    name: "V2List",
    pattern: "api/v2/Packages()",
    defaults: new { controller = "V2Api", action = "List" });

app.MapControllerRoute(
    name: "V2Search",
    pattern: "api/v2/Search()",
    defaults: new { controller = "V2Api", action = "Search" });

app.MapControllerRoute(
    name: "V2Package",
    pattern: "api/v2/FindPackagesById()",
    defaults: new { controller = "V2Api", action = "Package" });

app.MapControllerRoute(
    name: "V2PackageVersion",
    pattern: "api/v2/Packages(Id='{id}',Version='{version}')",
    defaults: new { controller = "V2Api", action = "PackageVersion" });

app.Run();
