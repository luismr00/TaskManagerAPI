# Task Manager API (ASP.NET Core + SQL Server + Docker)

A backend API for a task management application, built using ASP.NET Core 8, Entity Framework Core, and SQL Server running in Docker. This project is designed to reinforce .NET skills learned during MSSA and demonstrate full CRUD capabilities via a clean Web API.

---

## Prerequisites

- Visual Studio 2022
- Docker Desktop
- SQL GUI (SSMS or Azure Data Studio)
- .NET 8 SDK

---

## Running SQL Server in Docker

1. Pull the SQL Server 2022 image:
   ```bash
   docker pull mcr.microsoft.com/mssql/server:2022-latest
   ```

2. Run a container in Docker by pasting this in a console or powershell:
	```bash
	docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MyPassword!23" `
	-p 1433:1433 --name sqlserver `
	-v sqlvolume:/var/opt/mssql `
	-d mcr.microsoft.com/mssql/server:2022-latest
	```
---

## Coonecting to SQL Server in SSMS

1. Launch SSMS.
2 Connect to:
- Server: localhost,1433
- Authentication: SQL Server Auth
- Login: sa
- Password: MyPassword!23
3. Once connected, expand the Databases folder to view your tables after running migrations.

---
 
## Creating the .NET Web API Project

1. Open Visual Studio -> Create a New Project -> Select ASP.NET Core Web API
2. Choose .NET 8
3. Name your project and click Create

---

## Setting Up the Project

1. Install Entity Framework Packages

```powershell
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package Microsoft.EntityFrameworkCore.Tools
```

2. Configure the Database Connection in appsettings.json

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TaskManagerDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

3. Set up your Program.cs file. Should look similar to this:

```csharp
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

```

4. Create the ApplicationDbContext.cs file inside a Folder called Data:

```csharp
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks { get; set; }
}
```

5. Create your Models. Here's how a TaskItem model would look:

```csharp
using System.ComponentModel.DataAnnotations;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public TaskStatus Status { get; set; }
}

public enum TaskStatus
{
    ToDo,
    InProgress,
    Completed
}

```

6. Create controllers based on your APIs and models needed inside the controllers folder. Example:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        return await _context.Tasks.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
    }
}

```

---

## Running Migrations

First cd to the project folder containing the .csproj file. Example:

```bash
cd TaskManagerAPI/TaskManagerAPI
```

Then run in the console:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## Running the Application

- Press F5 in visual studio
    - Or run from terminal with dotnet run

Swagger UI should open automatically at:

```bash
http://localhost:5000/swagger
```

You can now test your API endpoints and integrate your frontend.