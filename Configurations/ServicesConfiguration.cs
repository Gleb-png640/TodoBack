using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoBack.Data;
using TodoBack.Dtos.Tasks;
using TodoBack.Dtos.Users;
using TodoBack.Models.Users;
using TodoBack.QueryParameters;
using TodoBack.Repositories;
using TodoBack.Services.Security;
using TodoBack.Validations.Tasks;
using TodoBack.Validations.Users;

namespace TodoBack.Configurations
{
    public static class ServicesConfiguration
    {

        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder, ConfigurationManager configuration)
        {
            SwaggerWithAuth.AddSwaggerWithAuth(builder);

            builder.Services.AddDbContext<TodoDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                    ValidateAudience = true,
                    ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Token")!)),
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddSingleton<JwtTokenServices>();

            builder.Services.AddScoped<IValidator<CreateUserTaskDto>, CreateUserTaskValidation>();
            builder.Services.AddScoped<IValidator<UpdateUserTaskDto>, UpdateUserTaskValidation>();
            builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidation>();
            builder.Services.AddScoped<IValidator<LoginUserDto>, LoginUserValidation>();

            builder.Services.AddScoped<IValidator<GetPageQuery>, GetPageQueryValidation>();

            builder.Services.AddScoped<ITaskRepository, PostgresTaskRepository>();
            //builder.Services.AddScoped<ITaskRepository, PostgresTaskRepositoryDapper>();
            builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();

            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            return builder;
        }
    }
}
