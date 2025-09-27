#if DEBUG
// Launch DbLauncher before starting API
using System.Diagnostics;

var process = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = "docker",
        Arguments = "ps --filter name=hisabwala_db --format \"{{.Names}}\"",
        RedirectStandardOutput = true,
        UseShellExecute = false
    }
};

process.Start();
var output = process.StandardOutput.ReadToEnd().Trim();
process.WaitForExit();

if (string.IsNullOrEmpty(output))
{
    Console.WriteLine("⏳ Starting DB container...");
    Process.Start("docker", "compose -f docker-compose.dev.yml up --build");
}
else
{
    Console.WriteLine("✅ DB already running");
}
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
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
