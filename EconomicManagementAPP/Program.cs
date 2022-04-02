using EconomicManagementAPP.Models;
using EconomicManagementAPP.Interface;
using EconomicManagementAPP.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositorieAccountTypes, RepositorieAccountTypes>();
builder.Services.AddTransient<IRepositorieUsers, RepositorieUsers>();
builder.Services.AddTransient<IRepositorieAccounts, RepositorieAccounts>();
builder.Services.AddTransient<IRepositorieOperationTypes, RepositorieOperationTypes>();
//builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IRepositorieCategories, RepositorieCategories>();
builder.Services.AddTransient<IRepositorieTransactions, RepositorieTransactions>();
//builder.Services.AddTransient<SignInManager<Users>>();
//builder.Services.AddIdentityCore<Users>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = false;
//});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
//    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
//    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
//}).AddCookie(IdentityConstants.ApplicationScheme);

//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//    options.Cookie.Name = ".EconmocManagementAPP.Session";
//    options.IdleTimeout = TimeSpan.FromSeconds(10);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

//app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
