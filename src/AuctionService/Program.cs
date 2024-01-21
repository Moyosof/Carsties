using AuctionService;
using AuctionService.Data;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
//this is what we need to do to connect our rabbitMQ to use local host as the address of the host to connect to.
builder.Services.AddMassTransit(x =>
{
    // this configuration helps to retry the data in the rabbitMq out message when it fails.
    x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(10);

        o.UsePostgres();
        o.UseBusOutbox();
    });

    x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(op =>
    {
        op.Authority = builder.Configuration["IdentityServiceUrl"];
        op.RequireHttpsMetadata = false;
        op.TokenValidationParameters.ValidateAudience = false;
        op.TokenValidationParameters.NameClaimType = "username";
    });
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();
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
