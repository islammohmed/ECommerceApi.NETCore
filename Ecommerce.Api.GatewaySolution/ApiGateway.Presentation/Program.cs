using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using ECommerce.SharedLibrary.DependencyInjection;
using ApiGateway.Presentation.Middleware;
using Ocelot.Middleware;
var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("ocelot.jsomn", optional: false, reloadOnChange: true);
builder.Services.AddOcelot().AddCacheManager(x=>x.WithDictionaryHandle());
JWTAuthenticationSchema.AddJWTAuthenticationSchena(builder.Services, builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
        });
});
var app = builder.Build();

app.UseCors();
app.UseMiddleware<AttachSignatureToRequest>();
app.UseOcelot().Wait();
app.UseHttpsRedirection();
app.Run();
