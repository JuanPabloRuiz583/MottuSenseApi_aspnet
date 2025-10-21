using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sprint.Data;
using Sprint.Services;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Injeção de dependecias:
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IPatioService, PatioService>();
builder.Services.AddScoped<ISensorLocalizacaoService, SensorLocalizacaoService>();

// Chave secreta para JWT (ideal: coloque no appsettings.json)
var key = Encoding.ASCII.GetBytes("minha-chave-super-secreta-1234567890");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

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

    // JWT no Swagger
    configurationSwagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no formato: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    configurationSwagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
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
app.UseAuthentication(); // Adicionado para JWT funcionar
app.UseAuthorization();
app.MapControllers();
app.Run();
