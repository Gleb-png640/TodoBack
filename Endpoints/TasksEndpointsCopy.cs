using FluentValidation;
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
            group.MapGet("/", ([AsParameters] GetPageQuery query,  ITaskRepository repo, IValidator<GetPageQuery> validator) =>
            {
                FluentValidation.Results.ValidationResult result = validator.Validate(query);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                return Results.Ok(repo.GetPaged(query));
            });

            // GET /tasks/1
            group.MapGet("/{id}", (int id, ITaskRepository repo) => 
            {
                var task = repo.GetById(id);
                return task is null ? Results.NotFound() : Results.Ok(task.EntityToDto());
            }).WithName(GetTaskEndpointName);



            // POST /tasks
            group.MapPost("/", (CreateUserTaskDto taskDto, ITaskRepository repo, IValidator<CreateUserTaskDto> validator) =>
            {

                // validation
                FluentValidation.Results.ValidationResult result = validator.Validate(taskDto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // adding to db
                var createdTask = repo.Add(taskDto.DtoToEntity());

                return Results.Created($"/tasks/{createdTask.TaskId}", createdTask.EntityToDto());
            });


            // PUT /tasks/1
            group.MapPut("/{id}", (int id, UpdateUserTaskDto taskDto, ITaskRepository repo, IValidator<UpdateUserTaskDto> validator) => 
            {

                // searching in db
                var task = repo.GetById(id);
                if (task is null) { return Results.NotFound(); }

                // validation
                FluentValidation.Results.ValidationResult result = validator.Validate(taskDto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // updating
                repo.ChangeExistingTask(task, taskDto);

                return Results.Ok(task.EntityToDto());
            });


            // DLELETE /tasks/1
            group.MapDelete("/{id}", (int id, ITaskRepository repo) =>
            {
                var task = repo.GetById(id);
                if (task is null) { return Results.NotFound(); }

                repo.Delete(task);

                return Results.NoContent();
            });

        }
    }
}
