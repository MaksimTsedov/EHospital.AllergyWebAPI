using AutoMapper;
using EHospital.Allergies.Data;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.BusinesLogic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using EHospital.Allergies.Model;
using Microsoft.Extensions.Logging;

namespace EHospital.Allergies.WebAPI
{
    public class Startup
    {

        private static readonly log4net.ILog log = log4net.LogManager
                                                          .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            log.Info("Using allergy service.");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "AllergyAPI",
                    Description = "UI for testing correct functionality of Allergy service",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Maksim Tsedov", Email = "maksim.czedov.99@gmail.com", Url = "" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseHttpsRedirection();  
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AllergyAPI V1");
            });
        }
    }
}
