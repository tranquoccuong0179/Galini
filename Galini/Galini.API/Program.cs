using Galini.API;
using Galini.API.ConfigHub;
using Galini.API.Constants;
using Galini.Models.Enum;
using Galini.Models.Payload;
using Galini.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
    c.MapType<TypeEnum>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(TypeEnum))
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
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsConstant.PolicyName,
        policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsConstant.PolicyName);

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapPost("broadcast", async (string message, IHubContext<ChatHub, IChatClient> context) => {
    await context.Clients.All.ReceiveMessage(message);

    return Results.NoContent();
});

app.MapControllers();

app.MapHub<ChatHub>("chat-hub");

app.Run();
