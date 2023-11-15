using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using MiniETrade.API.Middlewares.ExceptionHandling;
using MiniETrade.API.Middlewares.Localization;
using MiniETrade.Application;
using MiniETrade.Application.Common.Abstractions.Localization;
using MiniETrade.Application.Features.Products.Commands.CreateProduct;
using MiniETrade.Infrastructure;
using MiniETrade.Infrastructure.Filters;
using MiniETrade.Persistence;
using System.Globalization;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//MassTransit Registration
//builder.Services.AddMassTransitRegistration(builder.Configuration); TODO-HUS şirket internetinde olunca hata veriyor. Kapattık şimdilik.

// Add services to the container.
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddCors(options => options.AddDefaultPolicy(
    //policy => policy.AllowAnyHeader().AllowAnyOrigin()  //her s.a diyen siteye girebilir şeklinde bir ayarlama.
    policy => policy.WithOrigins("http://localhost:4200/", "https://localhost:4200/").AllowAnyHeader().AllowAnyMethod() //böylece sadece burdaki arkadaþlar istek atabilirler API'ye.
));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>()) //Kendi yazdığımız custom filter'ı devreye sokuyoruz.
    .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()) //Böylece Application assembly'sindeki tüm Validator
//sınıflarını otomatik tarar ve bulur. Burda CreateProductValidator demiş olmamızında bir önemi yok. Aslında o assemblideki tüm sınıfları tarıyor.
//TODO-HUS Burada CreateProductValidator yazması hoşuma gitmedi .bİ bakalım buna.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);    //Bu ayarlama ile default Filter'ın çalışmasını engellemiş oluyoruz
//Default filter ile validasyon hatası oluştuğunda .Net otomatik kendisi bize sormadan ErrorResponse üretip döndürüyor.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<ILanguageService, LanguageService>();

builder.Services.AddTransient<GlobalExceptionHandler>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        //Token geldiğinde hangi kontroller yapılsın ayarları:
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //Oluþturulacak token deðerini kimlerin/hangi originlerin/sitelerin kullanýcý belirlediðimiz deðerdir. -> www.bilmemne.com
            ValidateIssuer = true, //Oluþturulacak token deðerini kimin daðýttýný ifade edeceðimiz alandýr. -> www.myapi.com
            ValidateLifetime = true, //Oluþturulan token deðerinin süresini kontrol edecek olan doðrulamadýr.
            ValidateIssuerSigningKey = true, //Üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden suciry key verisinin doðrulanmasýdýr.
            ValidAudience = builder.Configuration["TokenOptions:Audience"],
            ValidIssuer = builder.Configuration["TokenOptions:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenOptions:SecurityKey"]!)),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,
            NameClaimType = ClaimTypes.Name //JWT üzerinde Name claimne karşılık gelen değeri User.Identity.Name propertysinden elde edebiliriz.
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Localization
var supportedCultures = new List<CultureInfo>
{
    new CultureInfo("en-US"),   // English (United States)
    new CultureInfo("es-ES"),   // Spanish (Spain)
    new CultureInfo("tr-TR"),   // Turkish (Turkey)
    new CultureInfo("ru-RU")    // Russian (Russia)
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>() { new AcceptLanguageHeaderRequestCultureProvider() }
});

//if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
app.UseMiddleware<GlobalExceptionHandler>();

app.UseStaticFiles();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();