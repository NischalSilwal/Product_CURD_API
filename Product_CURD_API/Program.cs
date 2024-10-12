using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Product_CURD_API.Data;
using Product_CURD_API.Repositories;
using Product_CURD_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DemoDbContext>(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    x.UseSqlServer(connectionString);
});

// Registering dependencies for services and repositories
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IFileService, FileService>();

// Define CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://example.com", "https://anotherexample.com") // Replace with your allowed origins
              .AllowAnyHeader()
              .AllowAnyMethod(); // Allow specific HTTP methods or AllowAnyMethod()
    });
});

var app = builder.Build();

// Ensure the "Uploads" directory exists
var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "Uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath); // Create directory if it doesn't exist
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Resources"
});

// Enable CORS
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
