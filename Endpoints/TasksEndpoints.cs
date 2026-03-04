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
                var id = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                FluentValidation.Results.ValidationResult result = validator.Validate(query);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                return Results.Ok(repo.GetPaged(query, id));
            });

            // GET /tasks/1
            group.MapGet("/{id}", (int id, ITaskRepository repo, ClaimsPrincipal user) => 
            {
                var UserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                TaskCommon? task = repo.GetById(id, UserId);
                return task is null ? Results.NotFound() : Results.Ok(task.EntityToDto());
            }).WithName(GetTaskEndpointName);



            // POST /tasks
            group.MapPost("/", (CreateTaskCommonDto taskDto, ITaskRepository repo, IValidator<CreateTaskCommonDto> validator, ClaimsPrincipal user) =>
            {

                // validation
                FluentValidation.Results.ValidationResult result = validator.Validate(taskDto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // adding to db
                var id = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                TaskCommon task = taskDto.DtoToEntity(id);
                TaskCommon createdTask = repo.Add(task);

                return Results.Created($"/tasks/{createdTask.TaskId}", createdTask.EntityToDto());
            });


            // PUT /tasks/1
            group.MapPut("/{id}", (int id, UpdateTaskCommonDto taskDto, ITaskRepository repo, IValidator<UpdateTaskCommonDto> validator, ClaimsPrincipal user) => 
            {

                var UserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value); 
                // searching in db
                var task = repo.GetById(id, UserId);
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
                var UserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var task = repo.GetById(id, UserId);
                if (task is null) { return Results.NotFound(); }

                repo.Delete(task);

                return Results.NoContent();
            });

        }
    }
}
