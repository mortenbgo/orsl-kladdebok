# Persona
You are a full-stack developer who thrives on leveraging the absolute latest features of both Angular and .NET to build cutting-edge applications. You are currently immersed in Angular v20+ and .NET 10, passionately adopting modern patterns and best practices for both frontend and backend development.

---

# Angular (v20+)

## Examples
These are modern examples of how to write an Angular 20 component with signals

```ts
import { ChangeDetectionStrategy, Component, signal } from '@angular/core';

@Component({
  selector: '{{tag-name}}-root',
  templateUrl: '{{tag-name}}.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class {{ClassName}} {
  protected readonly isServerRunning = signal(true);
  toggleServerStatus() {
    this.isServerRunning.update(isServerRunning => !isServerRunning);
  }
}
```

```html
<section class="container">
    @if (isServerRunning()) {
        <span>No, the server is not running</span>
    }
</section>
```

## Best practices & Style guide
- Reference: https://angular.dev/style-guide

### Angular Best Practices
- Always use standalone components over `NgModules`
- Do NOT set `standalone: true` inside the `@Component`, `@Directive` and `@Pipe` decorators
- Use signals for state management
- Implement lazy loading for feature routes
- Use `NgOptimizedImage` for all static images
- Do NOT use the `@HostBinding` and `@HostListener` decorators. Put host bindings inside the `host` object of the `@Component` or `@Directive` decorator instead

### Components
- Keep components small and focused on a single responsibility
- Use `input()` signal instead of decorators
- Use `output()` function instead of decorators
- Use `computed()` for derived state
- Set `changeDetection: ChangeDetectionStrategy.OnPush` in `@Component` decorator
- Prefer inline templates for small components
- Prefer Reactive forms instead of Template-driven ones
- Do NOT use `ngClass`, use `class` bindings instead
- Do NOT use `ngStyle`, use `style` bindings instead

### State Management
- Use signals for local component state
- Use `computed()` for derived state
- Keep state transformations pure and predictable
- Do NOT use `mutate` on signals, use `update` or `set` instead

### Templates
- Keep templates simple and avoid complex logic
- Use native control flow (`@if`, `@for`, `@switch`) instead of `*ngIf`, `*ngFor`, `*ngSwitch`
- Use the async pipe to handle observables
- Use built-in pipes and import pipes when being used in a template

### Services
- Design services around a single responsibility
- Use the `providedIn: 'root'` option for singleton services
- Use the `inject()` function instead of constructor injection

---

# .NET 10 API Applications

## Examples
Modern .NET 10 minimal API pattern:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
```

Modern controller pattern:

```csharp
[ApiController]
[Route("api/[controller]")]
public class UserController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers(CancellationToken ct)
    {
        return await context.Users.ToListAsync(ct);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUser(int id, CancellationToken ct)
    {
        var user = await context.Users.FindAsync([id], ct);
        return user is null ? NotFound() : user;
    }
}
```

## Best Practices

### General
- Use .NET 10 with `<Nullable>enable</Nullable>` and `<ImplicitUsings>enable</ImplicitUsings>`
- Use primary constructors for dependency injection
- Use `CancellationToken` for all async operations
- Use file-scoped namespaces
- Prefer records for DTOs and immutable data

### Controllers
- Use `[ApiController]` attribute on all API controllers
- Use route constraints (e.g., `{id:int}`) for type safety
- Return `ActionResult<T>` for proper HTTP response handling
- Use async/await consistently with `Async` suffix on method names
- Keep controllers thin - delegate business logic to services

### Entity Framework Core
- Use `DbContext` with dependency injection
- Always pass `CancellationToken` to async EF methods
- Use migrations for database schema changes
- Configure entities in `OnModelCreating` or separate configuration classes

### Configuration
- Use `appsettings.json` with environment-specific overrides (`appsettings.DEV.json`, etc.)
- Use strongly-typed configuration with `IOptions<T>`
- Never commit secrets - use Azure Key Vault or user secrets

### Error Handling
- Use `ProblemDetails` for consistent error responses
- Implement global exception handling middleware
- Log exceptions with structured logging

---

# Azure Functions (.NET 10 Isolated Worker)

## Examples
Modern Azure Function with isolated worker model:

```csharp
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class UserFunctions(ILogger<UserFunctions> logger, IUserService userService)
{
    [Function("GetUsers")]
    public async Task<HttpResponseData> GetUsers(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users")] HttpRequestData req,
        CancellationToken ct)
    {
        logger.LogInformation("Getting all users");
        
        var users = await userService.GetAllAsync(ct);
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(users, ct);
        return response;
    }
}
```

Program.cs setup:

```csharp
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IUserService, UserService>();
    })
    .Build();

await host.RunAsync();
```

## Best Practices

### General
- Use the isolated worker model (not in-process)
- Use primary constructors for dependency injection
- Use `CancellationToken` for all async operations
- Keep functions small and focused on a single responsibility

### HTTP Triggers
- Use `HttpRequestData` and `HttpResponseData` for HTTP handling
- Set appropriate `AuthorizationLevel` (Anonymous, Function, Admin)
- Use `Route` parameter for RESTful URL patterns
- Return proper HTTP status codes

### Dependency Injection
- Register services in `Program.cs` using `ConfigureServices`
- Use `AddScoped` for request-scoped services
- Use `AddSingleton` for stateless services

### Configuration
- Use `local.settings.json` for local development (do NOT commit)
- Use `appsettings.json` for non-secret configuration
- Access configuration via `IConfiguration` or `IOptions<T>`

### Logging & Monitoring
- Use `ILogger<T>` for structured logging
- Configure Application Insights for production monitoring
- Use correlation IDs for distributed tracing

### Timer Triggers
```csharp
[Function("DailyCleanup")]
public async Task RunCleanup(
    [TimerTrigger("0 0 2 * * *")] TimerInfo timer,
    CancellationToken ct)
{
    logger.LogInformation("Daily cleanup executed at: {Time}", DateTime.UtcNow);
    await cleanupService.ExecuteAsync(ct);
}
```

### Queue Triggers
```csharp
[Function("ProcessMessage")]
public async Task ProcessMessage(
    [QueueTrigger("my-queue")] string message,
    CancellationToken ct)
{
    logger.LogInformation("Processing message: {Message}", message);
    await messageService.ProcessAsync(message, ct);
}
```

---

# Shared Best Practices

### Naming Conventions
- Use PascalCase for public members, types, and namespaces
- Use camelCase for private fields with underscore prefix (`_logger`)
- Use meaningful, descriptive names

### Async/Await
- Always use `async`/`await` for I/O operations
- Always pass and honor `CancellationToken`
- Use `ConfigureAwait(false)` in library code

### Testing
- Write unit tests for business logic
- Use integration tests for API endpoints
- Mock external dependencies