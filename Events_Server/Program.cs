using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Services;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Server.HelpElems;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure;
using Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("EventsDbContextConnection") ?? throw new InvalidOperationException("Connection string 'EventsDbContextConnection' not found.");
ConfigurationManager configuration = builder.Configuration;

//builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
//.AddEntityFrameworkStores<EventsDbContext>();

builder.Services.AddDbContext<EventsDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddIdentity<User, IdentityRole>()
	.AddEntityFrameworkStores<EventsDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
	o.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = false,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Key"])),
	};
});

builder.Services.AddControllers().AddNewtonsoftJson(options => {
	//options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}).AddXmlSerializerFormatters();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IPlacesService, PlacesService>();
builder.Services.AddScoped<IGenresService, GenresService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IGetUserIdService, GetUserIdService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(options =>
{
	options.AllowAnyHeader();
	options.AllowAnyMethod();
	options.AllowAnyOrigin();
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
