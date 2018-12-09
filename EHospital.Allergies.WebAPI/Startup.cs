using System.Reflection;
using AutoMapper;
using EHospital.Allergies.BusinessLogic.Contracts;
using EHospital.Allergies.BusinessLogic.Services;
using EHospital.Allergies.Data;
using EHospital.Allergies.Model;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace EHospital.Allergies.WebAPI
{
    /// <summary>
    /// Configuration object for allergy service.
    /// </summary>
    public class Startup
    {

        private static readonly ILog Log = LogManager
                                                          .GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.  
        /// </summary>
        /// <param name="services">The services.</param>      
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AllergyDbContext>(options =>
                     options.UseSqlServer(Configuration.GetConnectionString("EHospitalDatabase")));

            Mapper.Initialize(cfg => cfg.AddProfile<AllergyProfile>());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRepository<Allergy>, Repository<Allergy>>();
            services.AddScoped<IRepository<Symptom>, Repository<Symptom>>();
            services.AddScoped<IRepository<PatientAllergy>, Repository<PatientAllergy>>();
            services.AddScoped<IRepository<AllergySymptom>, Repository<AllergySymptom>>();
            services.AddScoped<IAllergyService, AllergyService>();
            services.AddScoped<IAllergySymptomService, AllergySymptomService>();
            services.AddScoped<IPatientAllergyService, PatientAllergyService>();
            services.AddScoped<ISymptomService, SymptomService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            Log.Info("Using allergy service.");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "AllergyAPI",
                    Description = "UI for testing correct functionality of Allergy service",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Maksim Tsedov", Email = "maksim.czedov.99@gmail.com", Url = "" }
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()

                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            });
        }

        //      
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.   
        /// </summary>
        /// <param name="app">The application build.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();  
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "AllergyAPI V1");
            });
        }
    }
}
