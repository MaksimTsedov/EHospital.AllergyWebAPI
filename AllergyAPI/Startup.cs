using EHospital.AllergyDA;
using EHospital.AllergyDA.Contracts;
using EHospital.AllergyDA.Entities;
using EHospital.AllergyDA.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace EHospital.AllergyAPI
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
            string connection = @"Server=(localdb)\mssqllocaldb;Initial Catalog=EHospital;Integrated Security=True;";
            services.AddDbContext<AllergyDbContext>(options => options.UseSqlServer(connection));

            services.AddScoped<IRepository<Allergy>, Repository<Allergy>>();
            services.AddScoped<IRepository<Symptom>, Repository<Symptom>>();
            services.AddScoped<IRepository<PatientAllergy>, Repository<PatientAllergy>>();
            services.AddScoped<IRepository<AllergySymptom>, Repository<AllergySymptom>>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

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
