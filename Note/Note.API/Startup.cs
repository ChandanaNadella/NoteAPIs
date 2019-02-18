namespace Note.API
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.PlatformAbstractions;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json.Serialization;
    using Note.API.Common.Attributes;
    using Note.API.Common.Extensions;
    using Note.API.Common.Settings;
    using Note.API.Swagger;
    using Note.IoC.Configuration.Profiles;
    using Note.Repository.Data;
    using Note.Services;
    using Note.Services.Contracts;
    using Note.Services.Services;
    using Swashbuckle.AspNetCore.Swagger;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public IHostingEnvironment HostingEnvironment { get; private set; }

        private AppSettings _appSettings;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            HostingEnvironment = env;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Configure<IISOptions>(options =>
            {

            });

            //API Explorer (for API Versioning)
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            services.AddMvcCore().AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });


            services.AddMvc(
                setupAction =>
                {
                    setupAction.Filters.Add(typeof(CustomFilterAttribute));
                    setupAction.ReturnHttpNotAcceptable = true;                   
                   // setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                   // setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
                });

            //API Version
            services.AddApiVersioning(
                o =>
                {
                    //o.Conventions.Controller<UserController>().HasApiVersion(1, 0);
                    o.ReportApiVersions = true;
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                    o.ApiVersionReader = new UrlSegmentApiVersionReader();
                }
                );

            //App settings
            var appSettingsSection = Configuration.GetSection("AppSettings");
            if (appSettingsSection == null)
                throw new System.Exception("No appsettings section has been found");

            services.Configure<AppSettings>(appSettingsSection);

            _appSettings = appSettingsSection.Get<AppSettings>();

            if (_appSettings.IsValid())
            {
                //Enabling the DB Context with connection string in the Appsettings:
                var connection = _appSettings.ConnectionString;
                services.AddDbContext<NoteDataContext>(options => options.UseSqlServer(connection));

                // configure jwt authentication
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
           .AddJwtBearer(x =>
           {
               x.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                      // var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                       var userId = context.Principal.Identity.Name;
                      // var user = userService.GetById(userId);
                       //if (user.Item1 == null)
                       //{
                       //    // return unauthorized if user no longer exists
                       //    context.Fail("Unauthorized");
                       //}
                       return Task.CompletedTask;
                   }
               };
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

                if (_appSettings.Swagger.Enabled)
                {
                    // Register the Swagger generator, defining 1 or more Swagger documents
                    services.AddSwaggerGen(options =>
                    {
                        // resolve the IApiVersionDescriptionProvider service
                        // note: that we have to build a temporary service provider here because one has not been created yet
                        var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                        // add a swagger document for each discovered API version
                        // note: you might choose to skip or document deprecated API versions differently
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                        }

                        // add a custom operation filter which sets default values
                        options.OperationFilter<SwaggerDefaultValues>();

                        // integrate xml comments
                        //options.IncludeXmlComments(XmlCommentsFilePath);

                    });
                }
            }

            //Automap settings
            services.AddAutoMapper();
            ConfigureMaps();

            //Custom services (.NET CORE 2.1)
            services.AddTransient<INoteService, NoteService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<ITypeHelperService, TypeHelperService>();
        

            //URI Helper enabling for pagination - Aviral
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                .ActionContext;
                return new UrlHelper(actionContext);
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
         {
            app.UseMvc();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddConsole();
                loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    //Adding custome exception error message for production environment.
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global Exception Logger.");
                            logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected error happened ! Please try again later.");
                    });
                });

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //Swagger section
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            if (_appSettings.IsValid())
            {
                if (_appSettings.Swagger.Enabled)
                {
                    app.UseSwagger();

                    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                    // specifying the Swagger JSON endpoint.
                    app.UseSwaggerUI(options =>
                    {
                        // build a swagger endpoint for each discovered API version
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                    });
                }
            }

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseMvc();
        }

        private void ConfigureMaps()
        {
            //Mapping settings
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<APIMappingProfile>();
                //cfg.AddProfile<ServicesMappingProfile>();
            });
        }

        string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"{_appSettings.API.Title} {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = _appSettings.API.Description
                //Contact = new Contact() { Name = "Bill Mei", Email = "bill.mei@somewhere.com" },
                //TermsOfService = "Shareware",
                //License = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
