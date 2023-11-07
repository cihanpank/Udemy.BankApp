using Microsoft.EntityFrameworkCore;
using Udemy.BankApp.Web.Data.Context;
using Udemy.BankApp.Web.Data.Interfaces;
using Udemy.BankApp.Web.Data.Repositories;
using Udemy.BankApp.Web.Data.UnitOfWork;
using Udemy.BankApp.Web.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BankContext>(opt =>
{
    opt.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=BankAppDb;integrated security=true;");
});

//builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
//builder.Services.AddScoped<IApplicationUserRepository,ApplicationUserRepository>();
//builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUow, Uow>();
builder.Services.AddScoped<IUserMapper,UserMapper>();
builder.Services.AddScoped<IAccountMapper, AccountMapper>();


builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
