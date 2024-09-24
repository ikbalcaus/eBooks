using Microsoft.EntityFrameworkCore;
using eBooksAPI.Database;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<eBooksContext>(options => options.UseSqlServer("Name=Database"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

IConfiguration Configuration = builder.Configuration;

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors(
    options => options
        .SetIsOriginAllowed(x => _ = true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
