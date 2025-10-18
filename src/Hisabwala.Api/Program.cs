using Hisabwala.Api.Behaviors;
using Hisabwala.Application.Features.Party.AddExpense;
using Hisabwala.Application.Features.Party.GeneratePartyCode;
using Hisabwala.Application.Features.Party.GetPartyInformation;
using Hisabwala.Application.Interfaces;
using Hisabwala.Infrastructure;
using Hisabwala.Infrastructure.Repositories;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular dev server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure MongoDB
builder.Services.AddSingleton<MongoDbContext>();

// Configure MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GeneratePartyCodeCommandHandler).Assembly);
});

// Configer Validators
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IValidator<GeneratePartyCodeCommand>, GeneratePartyCodeCommandValidator>();
builder.Services.AddScoped<IValidator<GetPartyInformationCommand>, GetPartyInformationCommandValidator>();
builder.Services.AddScoped<IValidator<AddExpenseCommand>, AddExpenseCommandValidator>();

// Repository Registration
builder.Services.AddScoped<IPartyRepository, PartyRepository>();

var app = builder.Build();

app.UseCors("AllowAngular"); // <-- Must be before MapControllers

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
