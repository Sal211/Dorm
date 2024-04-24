using SchoolManagement.Models.Connection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Service To COntroller
builder.Services.AddSession();

// Add Service To View
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// Add Connectsting
ClsConstring.Constr = builder.Configuration.GetSection("ConnectionString").Value.ToString();

// Add Service To COntroller
builder.Services.AddSession();

// Add Service To View
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=ViewLogin}/{id?}");

app.Run();
