using HomeTask.Data;
using HomeTask.Data.Interfaces;
using HomeTask.Service;
using HomeTask.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IActivityValidatorService, ActivityValidatorService>();
builder.Services.AddScoped<IStudentValidatorService, StudentValidatorService>();
builder.Services.AddScoped<IActivityData, ActivityData>();
builder.Services.AddScoped<IStudentData, StudentData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
