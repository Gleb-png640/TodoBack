using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoBack.Dtos.Tasks;
using TodoBack.Mapping;
using TodoBack.QueryParameters;
using TodoBack.Repositories;

namespace TodoBack.Endpoints {
    public static class TasksEndpoints {

        public static void MapCommonTasksEndpoints(this WebApplication app) {


            const string GetTaskEndpointName = "GetTask";

            var group = app.MapGroup("tasks").RequireAuthorization();

            // GET /tasks
            group.MapGet("/", async ([AsParameters] GetPageQuery query, ITaskRepository repo, IValidator<GetPageQuery> validator, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(query);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                return Results.Ok(await repo.GetPagedAsync(query, userId));
            });

            // GET /tasks/1
            group.MapGet("/{id}", async ([FromRoute] int id, ITaskRepository repo, ClaimsPrincipal user) => 
            {
                var UserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var task = await repo.GetByIdAsync(id, UserId);
                return task is null ? Results.NotFound() : Results.Ok(task.EntityToDto());
            }).WithName(GetTaskEndpointName);



            // POST /tasks
            group.MapPost("/", async (CreateUserTaskDto taskDto, ITaskRepository repo, IValidator<CreateUserTaskDto> validator, ClaimsPrincipal user) =>
            {

                // validation
                FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(taskDto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // adding to db
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var createdTask = await repo.AddAsync(taskDto.DtoToEntity(userId));

                return Results.Created($"/tasks/{createdTask.TaskId}", createdTask.EntityToDto());
            });


            // PUT /tasks/1
            group.MapPut("/{id}", async (int id, UpdateUserTaskDto taskDto, ITaskRepository repo, IValidator<UpdateUserTaskDto> validator, ClaimsPrincipal user) => 
            {

                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value); 

                // searching in db
                var task = await repo.GetByIdTrackedAsync(id, userId);
                if (task is null) { return Results.NotFound(); }

                // validation
                FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(taskDto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // updating
                await repo.ChangeExistingTaskAsync(task, taskDto);

                return Results.Ok(task.EntityToDto());
            });


            // DLELETE /tasks/1
            group.MapDelete("/{id}", async (int id, ITaskRepository repo, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var task = await repo.GetByIdAsync(id, userId);

                if (task is null) { return Results.NotFound(); }

                await repo.DeleteAsync(task);

                return Results.NoContent();
            });

        }
    }
}
