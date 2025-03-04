using System.Text.Json.Serialization;
using Galini.API;
using Galini.API.ConfigHub;
using Galini.API.Constants;
using Galini.Models.Enum;
using Galini.Models.Payload;
using Galini.Models.Payload.Response.Wallet;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabase();
builder.Services.AddUnitOfWork();
builder.Services.AddCustomServices();
builder.Services.AddJwtValidation();
builder.Services.AddHttpClientServices();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Harmon's API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        BearerFormat = "JWT",
        Scheme = "Bearer",
        Description = "Enter 'Bearer' [space] and then your token in the text input below.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            new string[] { }
        },
    };
    c.AddSecurityRequirement(securityRequirement);
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.AddSecurityRequirement(securityRequirement);
    c.MapType<TypeEnum>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(TypeEnum))
               .Select(name => new OpenApiString(name) as IOpenApiAny)
               .ToList()
    });
    c.MapType<FriendShipEnum>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(FriendShipEnum))
                   .Select(name => new OpenApiString(name) as IOpenApiAny)
                   .ToList()
    });
    c.MapType<TransactionTypeEnum>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(TransactionTypeEnum))
                   .Select(name => new OpenApiString(name) as IOpenApiAny)
                   .ToList()
    });
    c.MapType<BookingEnum>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(BookingEnum))
                   .Select(name => new OpenApiString(name) as IOpenApiAny)
                   .ToList()
    });
    c.MapType<TransactionStatusEnum>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(TransactionStatusEnum))
                   .Select(name => new OpenApiString(name) as IOpenApiAny)
                   .ToList()
    });
    c.MapType<TopicNameEnum>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(TopicNameEnum))
                   .Select(name => new OpenApiString(name) as IOpenApiAny)
                   .ToList()
    });

});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddRedis();
builder.Services.AddSignalR();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<PayOSSettings>(builder.Configuration.GetSection("PayOS"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsConstant.PolicyName,
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500", "http://localhost:5173", "http://127.0.0.1", "https://harmon.love", "https://galini-admin-fe.vercel.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsConstant.PolicyName);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapHub<CallHub>("callhub");

app.Run();
