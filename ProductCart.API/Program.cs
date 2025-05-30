using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ProductCart.Infrastructure;
using ProductCart.Interfaces;
using ProductCart.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBContext"));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddCors((setup) =>
{
    setup.AddPolicy("Deafult", options =>
    {
        options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("Deafult");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "ProductImages")),
    RequestPath = "/ProductImages"
});

app.UseAuthorization();

app.MapControllers();

app.Run();