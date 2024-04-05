var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DLMSContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DLMSContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.Configure<Jwt>(builder.Configuration.GetSection("JWT"));

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("NoCache", new CacheProfile { NoStore = true });
    options.CacheProfiles.Add("Any-60", new CacheProfile
    {
        Location = ResponseCacheLocation.Any,
        Duration = 60
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGenJwtAuthentication();
builder.Services.AddCustomJwtAuthentication(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p =>
    {
        p.AllowAnyOrigin();
        p.AllowAnyHeader();
        p.AllowAnyMethod();
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

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
