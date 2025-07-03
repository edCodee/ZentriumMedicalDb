using APIhospital.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region 1. CONFIGURACIÓN DEL BUILDER

// Configuración de archivos de configuración por entorno
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

#endregion

#region 2. REGISTRO DE SERVICIOS

// 2.1 Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2.2 Autenticación con JWT
var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new InvalidOperationException("Jwt:Key no configurado en appsettings.");
var jwtKeyBytes = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true; // HTTPS obligatorio
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

// 2.3 Configuración de CORS con política nombrada
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        policy.WithOrigins(builder.Configuration
                    .GetSection("Cors:AllowedOrigins")
                    .Get<string[]>() ?? [])
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2.4 Configuración de subida de archivos grandes (videos, pdf, etc.)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = builder.Configuration.GetValue<long>("UploadLimits:MaxBody", 200_000_000);
});

// 2.5 Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API DocBot",
        Version = "v1",
        Description = "API para gestionar documentos y bots"
    });

    // Configuración de esquema de seguridad para enviar el token por Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa **Bearer {tu_token}**"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// 2.6 Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();

builder.Services.AddControllers();

#endregion

#region 3. CONFIGURACIÓN DEL HOST

// Configuración de Kestrel (límite de tamaño de petición)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = builder.Configuration.GetValue<long>("UploadLimits:MaxBody", 200_000_000);
});

//Escuchar en cualquier puerto como lo exige Azure, comenta para pruebas locales
//builder.WebHost.UseUrls("http://+:80");

#endregion

var app = builder.Build();

#region 4. MIDDLEWARE

// 4.1 Redirección de encabezados cuando estás detrás de un proxy (ej: Azure App Service)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// 4.2 Seguridad HTTPS + HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseHsts(); // HTTP Strict Transport Security
}
app.UseHttpsRedirection();

// 4.3 Servir archivos estáticos (pdf, imágenes, etc.)
app.UseStaticFiles();

// 4.4 CORS
app.UseCors("DefaultCors");

// 4.5 Autenticación + Autorización
app.UseAuthentication();
app.UseAuthorization();

// 4.6 Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API DocBot v1");
        //c.RoutePrefix = string.Empty; // sirve swagger en la raíz
    });
}

//Sirve en produccion para Azure
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API DocBot v1");
//    c.RoutePrefix = string.Empty; // Esto hace que Swagger se sirva en la raíz "/"
//});

// 4.7 Health Checks
app.MapHealthChecks("/health");

// 4.8 Map Controllers
app.MapControllers();

#endregion

#region 5. EJECUCIÓN

// Validación inicial de conexión a la base de datos
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.CanConnect())
    {
        Console.WriteLine(" Conexión exitosa a la base de datos.");
    }
    else
    {
        Console.WriteLine(" No se pudo conectar a la base de datos.");
    }
}

app.Run();

#endregion