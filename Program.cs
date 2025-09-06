using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sprint.Data;
using Sprint.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Adicionar as services aqui:
builder.Services.AddScoped<IClienteService, ClienteService>();


builder.Services.AddSwaggerGen(configurationSwagger =>
{
    configurationSwagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de gerenciamento de motos e sensores",
        Version = "v1",
        Description = "Uma API simples de gerenciamento de clientes, motos, patios e sensores  > + SOLID \r\n+ CleanCode \r\n+ Documentação com a OpenAPI (Swashbuckle)\r\n",
        Contact = new OpenApiContact
        {
            Name = "juan",
           
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    configurationSwagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(configurationSwagger =>
{
configurationSwagger.SwaggerEndpoint("/swagger/v1/swagger.json", "API MottuSense v1");
configurationSwagger.RoutePrefix = string.Empty; // Swagger UI as the home page
});


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();