using TodoBack.Configurations;
using TodoBack.Endpoints;

namespace TodoBack
{
    public class Program {
        public static void Main(string[] args) {

            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.AddServices(configuration);

            var app = builder.Build();

            app.AddMiddleware();

            app.MapCommonTasksEndpoints();
            app.MapUsersEndpoints();

            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
