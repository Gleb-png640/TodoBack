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

            var group = app.MapGroup("tasks");

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
                TaskCommon? task = repo.GetById(id);
                return task is null ? Results.NotFound() : Results.Ok(task.EntityToDto());
            }).WithName(GetTaskEndpointName);



            // POST /tasks
            group.MapPost("/", (CreateTaskCommonDto taskDto, ITaskRepository repo, IValidator<CreateTaskCommonDto> validator) =>
            {

                // validation
                FluentValidation.Results.ValidationResult result = validator.Validate(taskDto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // adding to db
                TaskCommon task = taskDto.DtoToEntity();
                TaskCommon createdTask = repo.Add(task);

                return Results.Created($"/tasks/{createdTask.TaskId}", createdTask.EntityToDto());
            });


            // PUT /tasks/1
            group.MapPut("/{id}", (int id, UpdateTaskCommonDto taskDto, ITaskRepository repo, IValidator<UpdateTaskCommonDto> validator) => 
            {

                // searching in db
                var task = repo.GetById(id);
                if (task is null) { return Results.NotFound(); }


                // validation
                FluentValidation.Results.ValidationResult result = validator.Validate(taskDto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // updating
                repo.ChangeExistingTask(task, taskDto);
                repo.Update(task);

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
