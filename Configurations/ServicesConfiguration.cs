using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Todo API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Вставьте JWT токен"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


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

            builder.Services.AddScoped<IValidator<CreateTaskCommonDto>, CreateTaskValidation>();
            builder.Services.AddScoped<IValidator<UpdateTaskCommonDto>, UpdateTaskValidation>();
            builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidation>();
            builder.Services.AddScoped<IValidator<LoginUserDto>, LoginUserValidation>();

            builder.Services.AddScoped<IValidator<GetPageQuery>, GetTasksValidation>();

            builder.Services.AddScoped<ITaskRepository, PostgresRepository>();
            builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();

            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            return builder;
        }
    }
}
