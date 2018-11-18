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

namespace EHospital.Allergies.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = @"Server=(localdb)\mssqllocaldb;Database=EHospitalDB;Trusted_Connection=True;";
            services.AddDbContext<AllergyDbContext>(options => options.UseSqlServer(connection));

            Mapper.Initialize(cfg => cfg.AddProfile<AllergyProfile>());

            services.AddScoped<IRepository<Allergy>, Repository<Allergy>>();
            services.AddScoped<IRepository<Symptom>, Repository<Symptom>>();
            services.AddScoped<IRepository<PatientAllergy>, Repository<PatientAllergy>>();
            services.AddScoped<IRepository<AllergySymptom>, Repository<AllergySymptom>>();
            services.AddScoped<IAllergyService, AllergyService>();
            services.AddScoped<IAllergySymptomService, AllergySymptomService>();
            services.AddScoped<IPatientAllergyService, PatientAllergyService>();
            services.AddScoped<ISymptomService, SymptomService>();

            services.AddSingleton<IUnitOfWork, UnitOfWork>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0.1", new Info
                {
                    Version = "v0.1",
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
                c.SwaggerEndpoint("/swagger/v0.1/swagger.json", "AllergyAPI V0.1");
            });
        }
    }
}
