using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using FluentValidation;
using validation.pipelineBehaviours;
using logging.pipelineBehaviours;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs.txt")
            .CreateLogger();


builder.Host.UseSerilog();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


var app = builder.Build();

app.Run();



public record MyRequest(string Text): IRequest<MyResponse>;

public record  MyResponse(string Message);

public sealed class MyHandler : IRequestHandler<MyRequest, MyResponse>
{
    public Task<MyResponse> Handle(MyRequest request, CancellationToken cancellationToken)
        => Task.FromResult<MyResponse>(new("..."));
}



public record CreateRoleCommandRequest(string Name) : IRequest<CreateRoleCommandResponse>;

public record CreateRoleCommandResponse(string Message);

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandResponse>
{
    public Task<CreateRoleCommandResponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult<CreateRoleCommandResponse>(new("..."));
    }
}

public class CreateRoleCommandValidator: AbstractValidator<CreateRoleCommandRequest>
{
   public CreateRoleCommandValidator()
   {
        RuleFor(x=> x.Name).NotEmpty().WithMessage("Required field!");
   }
}
