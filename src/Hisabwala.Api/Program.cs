using Hisabwala.Api.Behaviors;
using Hisabwala.Application.Features.Party.GeneratePartyCode;
using Hisabwala.Application.Features.Tags.AddTag;
using Hisabwala.Application.Interfaces;
using Hisabwala.Infrastructure.Persistence;
using Hisabwala.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetTagsQueryHandler).Assembly);
});

// Configer Validators
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IValidator<AddTagCommand>, AddTagValidator>();
builder.Services.AddScoped<IValidator<GeneratePartyCodeCommand>, GeneratePartyCodeValidator>();

// Repository Registration
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IPartyRepository, PartyRepository>();

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
