using pdt.svc.services;
using Google.Cloud.Diagnostics.Common;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddGoogle(new LoggingServiceOptions { ProjectId = "cloud-run-project-01" });  // .net 6 function methodology
builder.Configuration.AddEnvironmentVariables(prefix: "Env_");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext(ApplicationDbContext>( options => options.UseSqlServer(connectionString));

// .NET Core dependency injection
//builder.Services.AddTransient<ISearchEngine, SearchEngine>();  // using new keyword and constructor values in code, cleaner


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_=> true).AllowCredentials());

app.MapControllers();

app.Run();
