using BE_Contacts.Models;
using BE_Contacts.Repository.Implementations;
using BE_Contacts.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("BEContactsBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Pegar el token generado después de autenticarse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "BEContactsBearerAuth"
                }
            }, new List<string>()
        }
    });
});

builder.Services.AddCors(options => options.AddPolicy("AllowWebapp",
builder => builder
    .WithOrigins("http://localhost:4200") // Solo permite el frontend de Angular
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials())); // Esto permite el envío de credenciales como cookies y tokens


//// Cors
//builder.Services.AddCors(options => options.AddPolicy("AllowWebapp",
//                                    builder => builder.AllowAnyOrigin()
//                                                    .AllowAnyHeader()
//                                                    .AllowAnyMethod()));
////                                                    .AllowCredentials()));

//Add Context
builder.Services.AddDbContext<AplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
    });

    // Automapper
    builder.Services.AddAutoMapper(typeof(Program));

    // Add Sevices
    builder.Services.AddScoped<IContactRepository, ContactRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    // Add Authentication with JWT
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Authentication:Issuer"],
                ValidAudience = builder.Configuration["Authentication:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
            };
        });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowWebapp");

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

