using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SelenMebelMVC.Configuration.LayerConfigurations;
using SelenMebelMVC.Middllewares;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureDataAccess(builder.Configuration);
builder.Services.AddWeb(builder.Configuration);
builder.Services.AddService();

builder.Services.AddControllers()
     .AddNewtonsoftJson(options =>
     {
         options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
     });

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v2", new OpenApiInfo { Title = "SerenMebel API.swagger", Version = "v2" });
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "bearer"
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		policy =>
		{
			policy.AllowAnyOrigin()
				  .AllowAnyMethod()
				  .AllowAnyHeader();
		});
});

//builder.Services.AddControllers().AddNewtonsoftJson(options =>
//				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


// Serialog
var logger = new LoggerConfiguration()
   .ReadFrom.Configuration(builder.Configuration)
   .Enrich.FromLogContext()
   .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v2/swagger.json", "SerenMebel.swagger");

});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();


if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "SerenMebel API V1");
	c.RoutePrefix = "area/swagger";
});
app.UseMiddleware<TokenRedirectMiddleware>();

app.UseStatusCodePages(async context =>
{
	if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
	{
		context.HttpContext.Response.Redirect("login");
	}
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
