using AuctionService.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>( op => 
{
    op.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//configuring the automapper using appdomain to get the location of the MappingProfile
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//This is to run our seed data before the program launches 
try
{
    DbInitializer.InitDb(app);
}
catch(Exception e)
{
    Console.WriteLine(e);
}
app.Run();
