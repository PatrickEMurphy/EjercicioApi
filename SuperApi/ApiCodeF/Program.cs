using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Cadena Conexion
builder.Services.AddDbContext<JuegosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JuegosContext") ?? throw new InvalidOperationException("Connection string 'JuegosContext' not found.")));

// Add services to the container.
// Hacer que no haga Referencia circular (.AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);)
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Identity
builder.Services.AddIdentityCore<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = false
)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<JuegosContext>();

// Servivio de autenticación
// Opciones de validación y se validan los parametros del JSON de configuración
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    }   
);

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

// Autenticación antes de autorización :3
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
