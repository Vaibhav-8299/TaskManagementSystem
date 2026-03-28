using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Middleware;
using TaskManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────
// 1. Add Controllers
// ─────────────────────────────────────────────
builder.Services.AddControllers();

// ─────────────────────────────────────────────
// 2. Swagger Configuration
// ─────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ─────────────────────────────────────────────
// 3. MySQL Database Connection (Database First)
// ─────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseMySQL(connectionString!));

// ─────────────────────────────────────────────
// 4. Register Interface → Service (Dependency Injection)
// Controller uses ITaskService, NOT TaskService directly
// ─────────────────────────────────────────────
builder.Services.AddScoped<ITaskService, TaskService>();

// ─────────────────────────────────────────────
// 5. CORS (Allow Angular frontend to connect)
// ─────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ─────────────────────────────────────────────
// 6. Global Exception Middleware (must be FIRST)
// ─────────────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();

// ─────────────────────────────────────────────
// 7. Swagger UI (only in Development mode)
// ─────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ─────────────────────────────────────────────
// 8. Standard Pipeline
// ─────────────────────────────────────────────
app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();
