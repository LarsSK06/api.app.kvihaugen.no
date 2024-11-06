using Microsoft.OpenApi.Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo{
        Version = "v1",
        Title = "Kvihaugen Identity",
        Description = "Identity service for Kvihaugen family and possibly external clients"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
        Description = "Custom bearer token authentication scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy
            .WithOrigins("http://localhost:3000", "http://app.kvihaugen.no")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication().AddBearerToken();

WebApplication? app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.MapControllers();

app.Run();