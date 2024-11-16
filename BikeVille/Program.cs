using BikeVille.Auth;
using BikeVille.Auth.AuthContext;
using BikeVille.Entity.EntityContext;
using BikeVille.Transition;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace BikeVille
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddControllers().AddJsonOptions(o=>o.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.Preserve);
            //Service Transition
            builder.Services.AddHostedService<TransitionService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AdventureWorksLt2019Context>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("BikeVilleDb")));
            //auth
            builder.Services.AddDbContext<AdventureWorksLt2019usersInfoContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("BikeVilleUsersDb")));
            //cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                });
            });


            var app = builder.Build();

            //cors
            app.UseCors("AllowSpecificOrigin");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseAuthentication();
           



            app.MapControllers();

            app.Run();
        }
    }
}
