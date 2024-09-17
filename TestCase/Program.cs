using Serilog;
using TestCase.DependencyResolvers;
using TestCase.Utlis;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ConnSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<SysSettings>(builder.Configuration.GetSection("SysSettings"));

builder.Services.AddServices();
builder.Services.AddDbContextService();
builder.Services.AddIdentityService();
builder.Services.AddSeriLogService();
builder.Services.AddTokenService();
builder.Services.AddValidationService();

var app = builder.Build();

app.Services.AddIdentityProvider();
app.AddMiddlewareService();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Critical error occurred while starting the application.");
}
finally
{
    Log.CloseAndFlush();
}