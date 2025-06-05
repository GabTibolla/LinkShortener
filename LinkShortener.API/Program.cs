var builder = WebApplication.CreateBuilder(args);

// Configuração e serviços
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Config\\config.json", false, true);

builder.Services.AddSingleton<LinkShortener.API.Services.ConfigService>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Swagger só em ambiente dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapRazorPages();
app.MapControllers();

app.Run();
