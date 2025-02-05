using TestsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", opt =>
    {
        opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
builder.Services.AddControllers();
builder.Services.AddSingleton<TestServices>();
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

app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();
