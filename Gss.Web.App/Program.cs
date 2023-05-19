using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add domain and data services to the container (ideally use dll-specific configuration objects so this class doesn't need to have references to everything).


// TODO: set up automapper
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // TODO: use the file-based catalogs
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = string.Empty;
    });
}
else
{
    // TODO: use the RDBMS catalogs
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
