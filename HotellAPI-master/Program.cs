using HotellAPI.Data; 
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext configuration
builder.Services.AddDbContext<HotellContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

// Add Swagger (API-dokumentasjon)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Map API Controllers
app.MapControllers();

// Redirect root URL ("/") to Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return System.Threading.Tasks.Task.CompletedTask;
});

app.Run();


