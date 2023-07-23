
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;

namespace TaskManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddDbContext<TaskContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("TaskContext")));
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<TaskContext>();
                context.Database.EnsureCreated();
                // DbInitializer.Initialize(context);
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseErrorHandlingMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}