using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebShop.Domain;
using WebShop.Domain.Models;
using WebShop.Domain.Services;
using WebShopAPI.Helper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<WebShopContext>(options =>
{
    options.UseSqlServer(connectionString);

});


//Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();


// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization", 
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddSingleton<Common>();

builder.Services.AddAuthentication(config => {
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
    };
});

//Set graphQL with Authorization
builder.Services
    .AddGraphQLServer()
    .AddAuthorization() 
    .AddQueryType<WebShopAPI.GraphQL.Queries.ProductQuery>()
    .AddMutationType<WebShopAPI.GraphQL.Mutations.UserMutations>();


var allowSpecificOrigins = "_allowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("AllowLocalhost");
app.UseCors(allowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

//Add graphQL to API
app.MapGraphQL();

app.MapControllers();

app.Run();
