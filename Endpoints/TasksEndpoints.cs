using FluentValidation;
using System.Security.Claims;
using TodoBack.Dtos.Tasks;
using TodoBack.Mapping;
using TodoBack.Models.Tasks;
using TodoBack.QueryParameters;
using TodoBack.Repositories;

namespace TodoBack.Endpoints {
    public static class TasksEndpoints {

        public static void MapCommonTasksEndpoints(this WebApplication app) {


            const string GetTaskEndpointName = "GetTask";

            var group = app.MapGroup("tasks").RequireAuthorization();

            // GET /tasks
            group.MapGet("/", ([AsParameters] GetPageQuery query, ITaskRepository repo, IValidator<GetPageQuery> validator, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                FluentValidation.Results.ValidationResult result = validator.Validate(query);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                return Results.Ok(repo.GetPaged(query, userId));
            });

            // GET /tasks/1
            group.MapGet("/{id}", (int taskId, ITaskRepository repo, ClaimsPrincipal user) => 
            {
                var UserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var task = repo.GetById(taskId, UserId);
                return task is null ? Results.NotFound() : Results.Ok(task.EntityToDto());
            }).WithName(GetTaskEndpointName);



            // POST /tasks
            group.MapPost("/", (CreateUserTaskDto taskDto, ITaskRepository repo, IValidator<CreateUserTaskDto> validator, ClaimsPrincipal user) =>
            {

                // validation
                FluentValidation.Results.ValidationResult result = validator.Validate(taskDto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // adding to db
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var createdTask = repo.Add(taskDto.DtoToEntity(userId));

                return Results.Created($"/tasks/{createdTask.TaskId}", createdTask.EntityToDto());
            });


            // PUT /tasks/1
            group.MapPut("/{id}", (int id, UpdateUserTaskDto taskDto, ITaskRepository repo, IValidator<UpdateUserTaskDto> validator, ClaimsPrincipal user) => 
            {

                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value); 

                // searching in db
                var task = repo.GetByIdTracked(id, userId);
                if (task is null) { return Results.NotFound(); }

                // validation
                FluentValidation.Results.ValidationResult result = validator.Validate(taskDto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // updating
                repo.ChangeExistingTask(task, taskDto);

                return Results.Ok(task.EntityToDto());
            });


            // DLELETE /tasks/1
            group.MapDelete("/{id}", (int id, ITaskRepository repo, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var task = repo.GetById(id, userId);

                if (task is null) { return Results.NotFound(); }

                repo.Delete(task);

                return Results.NoContent();
            });

        }
    }
}
