# RecetArreAPI - Guía Paso a Paso para Clase

## Tabla de Contenidos
1. [Requisitos Previos](#requisitos-previos)
2. [Instalación de NuGet Packages](#instalación-de-nuget-packages)
3. [Configuración de appsettings.json](#configuración-de-appsettingsjson)
4. [Creación de Models](#creación-de-models)
5. [Configuración de ApplicationDbContext](#configuración-de-applicationdbcontext)
6. [Configuración de Identity](#configuración-de-identity)
7. [Configuración de AutoMapper](#configuración-de-automapper)
8. [Creación de DTOs](#creación-de-dtos)
9. [Migrations (EF Core)](#migrations-ef-core)
10. [Creación de Controllers](#creación-de-controllers)
11. [JWT y Autenticación](#jwt-y-autenticación)
12. [Temas para Profundizar](#temas-para-profundizar)

---

## Requisitos Previos

### Software Requerido
- Visual Studio 2022+ o Visual Studio Code
- .NET 8 SDK instalado
- PostgreSQL (u otra BD SQL compatible)
- Postman o Swagger (para testing)

### Conceptos Clave a Entender Antes
- ASP.NET Core Web API
- Entity Framework Core
- Relaciones entre tablas (1:1, 1:N, N:N)
- REST API (GET, POST, PUT, DELETE)
- JWT Token Authentication

---

## 1. Instalación de NuGet Packages

### Crear el Proyecto
```bash
dotnet new webapi -n RecetArreAPI
cd RecetArreAPI
```

### Packages Necesarios
Usa el Package Manager Console o CLI:

```bash
# Entity Framework Core y PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# Identity y Autenticación
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

# AutoMapper (sin extensiones DI)
dotnet add package AutoMapper

# Otros
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
dotnet add package Swashbuckle.AspNetCore
```

### Verificar instalación
```bash
dotnet list package
```

---

## 2. Configuración de appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=recetarre_db;Username=postgres;Password=your_password"
  },
  "LlaveJWT": "tu_clave_secreta_muy_larga_minimo_32_caracteres_para_HS256",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

**Conceptos Clave:**
- `ConnectionStrings`: Cadena de conexión a PostgreSQL
- `LlaveJWT`: Clave para firmar tokens JWT (debe ser larga y segura)
- Configuración sensible debería estar en `secrets.json` en producción

---

## 3. Creación de Models

### 3.1 ApplicationUser (Extiende IdentityUser)

```csharp
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(60)]
        public string? DisplayName { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public UserProfile? Profile { get; set; }
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<UserMedal> UserMedals { get; set; } = new List<UserMedal>();
        public ICollection<WeeklyRankingEntry> WeeklyRankingEntries { get; set; } = new List<WeeklyRankingEntry>();
    }
}
```

**¿Por qué IdentityUser?**
- Proporciona gestión segura de contraseñas
- Implementa lockouts y seguridad
- Integración nativa con roles y claims

### 3.2 Otros Models Principales

```csharp
// Category.cs
public class Category
{
    public int Id { get; set; }
    [Required]
    [StringLength(60, MinimumLength = 2)]
    public string Name { get; set; } = default!;
    [StringLength(250)]
    public string? Description { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public ICollection<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();
}

// Recipe.cs
public class Recipe
{
    public int Id { get; set; }
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Title { get; set; } = default!;
    [StringLength(1000)]
    public string? Description { get; set; }
    [Required]
    [StringLength(15000)]
    public string Instructions { get; set; } = default!;
    [Range(0, 24 * 60)]
    public int PrepTimeMinutes { get; set; }
    [Range(0, 24 * 60)]
    public int CookTimeMinutes { get; set; }
    [Range(1, 100)]
    public int Servings { get; set; } = 1;
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    // FK
    [Required]
    public string AuthorId { get; set; } = default!;

    // Navigation
    public ApplicationUser Author { get; set; } = default!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();
}
```

---

## 4. Configuración de ApplicationDbContext

```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Models;

namespace RecetArreAPI.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurar ApplicationUser
            builder.Entity<ApplicationUser>(e =>
            {
                e.HasDiscriminator<string>("Discriminator")
                    .HasValue("IdentityUser")
                    .HasValue(nameof(ApplicationUser));
                e.Property(x => x.DisplayName).HasMaxLength(60);
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");
                e.HasIndex(x => x.DisplayName);
            });

            // Configurar Recipe
            builder.Entity<Recipe>(e =>
            {
                e.Property(x => x.Title).HasMaxLength(120).IsRequired();
                e.Property(x => x.Instructions).HasMaxLength(15000).IsRequired();
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");
                e.Property(x => x.UpdatedAtUtc).HasDefaultValueSql("now()");

                e.HasOne(x => x.Author)
                    .WithMany(u => u.Recipes)
                    .HasForeignKey(x => x.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurar Category
            builder.Entity<Category>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(60).IsRequired();
                e.HasIndex(x => x.Name).IsUnique();
            });

            // Relaciones muchos a muchos
            builder.Entity<RecipeCategory>(e =>
            {
                e.HasKey(x => new { x.RecipeId, x.CategoryId });
                e.HasOne(x => x.Recipe)
                    .WithMany(r => r.RecipeCategories)
                    .HasForeignKey(x => x.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Category)
                    .WithMany(c => c.RecipeCategories)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        // DbSets
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Rating> Ratings => Set<Rating>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        // ... más DbSets
    }
}
```

**Conceptos Clave:**
- `IdentityDbContext<ApplicationUser>`: Proporciona tablas de Identity
- `OnModelCreating()`: Configuración avanzada de relaciones y restricciones
- `HasDefaultValueSql("now()")`: Valor por defecto en la BD
- `OnDelete(DeleteBehavior.Cascade/Restrict)`: Comportamiento de eliminación en cascada

---

## 5. Configuración de Identity en Program.cs

```csharp
// En Program.cs, después de AddControllers()

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

**¿Qué hace cada línea?**
- `AddIdentity<ApplicationUser, IdentityRole>()`: Registra Identity con tu usuario custom
- `AddEntityFrameworkStores<ApplicationDbContext>()`: Usa EF Core para almacenar datos
- `AddDefaultTokenProviders()`: Genera tokens para email, autenticación 2FA, etc.

---

## 6. Configuración de AutoMapper

### 6.1 Crear AutoMapperProfile

```csharp
using AutoMapper;
using RecetArreAPI.DTOs.Identity;
using RecetArreAPI.Models;

namespace RecetArreAPI.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Identity
            CreateMap<UserCredentialsDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Email));

            CreateMap<RegisterUserDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName));

            // Models a DTOs
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Recipe, RecipeDto>();
            
            // DTOs a Models
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<RecipeCreateDto, Recipe>();
            CreateMap<RecipeUpdateDto, Recipe>();
        }
    }
}
```

### 6.2 Registrar AutoMapper en Program.cs (Sin extensiones DI)

```csharp
using RecetArreAPI.Profiles;

// ... código previo ...

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());
```

**¿Por qué sin extensiones?**
- Más control sobre la configuración
- Menos dependencias
- Mejor para entender qué está pasando

---

## 7. Creación de DTOs

### Estructura Recomendada

```
DTOs/
├── Identity/
│   ├── UserCredentialsDto.cs
│   ├── RegisterUserDto.cs
│   └── AuthenticationResponseDto.cs
├── ApplicationUsers/
│   └── ApplicationUserDtos.cs
├── Categories/
│   └── CategoryDtos.cs
├── Recipes/
│   └── RecipeDtos.cs
└── ... (más DTOs)
```

### Ejemplo: CategoryDtos.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.Categories
{
    // DTO para lectura (GET)
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    // DTO para creación (POST)
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; } = default!;

        [StringLength(250)]
        public string? Description { get; set; }
    }

    // DTO para actualización (PUT)
    public class CategoryUpdateDto
    {
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; } = default!;

        [StringLength(250)]
        public string? Description { get; set; }
    }
}
```

**Patrón de Segregación:**
- `Dto`: Para lectura (proyección de datos)
- `CreateDto`: Solo campos necesarios para crear
- `UpdateDto`: Solo campos actualizables (excluye ID, CreatedAt, etc.)

**Ventajas:**
- Seguridad: No expone IDs internos
- Validación: DataAnnotations específicos por operación
- Mantenibilidad: Cambios de BD no afectan API

---

## 8. Migrations (EF Core)

### 8.1 Crear Primera Migración

```bash
# Crear migración inicial
dotnet ef migrations add InitialCreate

# Aplicar migración a la BD
dotnet ef database update
```

### 8.2 Comando Útiles

```bash
# Ver estado de migraciones
dotnet ef migrations list

# Crear nueva migración tras cambios en models
dotnet ef migrations add NombreDeMigracion

# Revertir última migración
dotnet ef migrations remove

# Revertir a una migración específica
dotnet ef database update NombreDeMigracion

# Ver SQL que generaría
dotnet ef migrations script
```

**Buenas Prácticas:**
- Nomina migraciones descriptivamente: `AddRecipeTable`, `AddUserProfiles`
- Revisa el código auto-generado antes de aplicar
- En equipo: nunca hagas `remove` de migraciones publicadas

---

## 9. Creación de Controllers

### Patrón Base

```csharp
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.Categories;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CategoriesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> Get()
        {
            var categories = await context.Categories
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<CategoryDto>>(categories);
        }

        // GET: api/categories/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (category is null)
                return NotFound();

            return mapper.Map<CategoryDto>(category);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create(CategoryCreateDto dto)
        {
            var category = mapper.Map<Category>(dto);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), 
                new { id = category.Id }, 
                mapper.Map<CategoryDto>(category));
        }

        // PUT: api/categories/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            var category = await context.Categories.FindAsync(id);
            if (category is null)
                return NotFound();

            mapper.Map(dto, category);
            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/categories/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category is null)
                return NotFound();

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
```

**Conceptos Clave:**
- `AsNoTracking()`: Mejor rendimiento para lectura
- `FindAsync()`: Busca por clave primaria
- `CreatedAtAction()`: Retorna 201 + Location header
- `NoContent()`: Retorna 204 (sin contenido)

---

## 10. JWT y Autenticación

### 10.1 Configurar JWT en Program.cs

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["LlaveJWT"]!)),
        ClockSkew = TimeSpan.Zero
    });

// Agregar Authorization middleware
app.UseAuthentication();
app.UseAuthorization();
```

### 10.2 AccountsController

```csharp
[HttpPost("register")]
public async Task<ActionResult<AuthenticationResponseDto>> Register(UserCredentialsDto dto)
{
    var usuario = mapper.Map<ApplicationUser>(dto);
    var resultado = await userManager.CreateAsync(usuario, dto.Password);
    
    if (resultado.Succeeded)
        return await BuildToken(dto.Email);
    
    return BadRequest(resultado.Errors);
}

[HttpPost("login")]
public async Task<ActionResult<AuthenticationResponseDto>> Login(UserCredentialsDto dto)
{
    var resultado = await signInManager.PasswordSignInAsync(
        dto.Email, 
        dto.Password, 
        isPersistent: false, 
        lockoutOnFailure: false);

    if (resultado.Succeeded)
        return await BuildToken(dto.Email);
    
    return BadRequest("Invalid login");
}

private async Task<AuthenticationResponseDto> BuildToken(string email)
{
    var user = await userManager.FindByEmailAsync(email);
    var claims = new List<Claim>
    {
        new Claim("email", email),
        new Claim(ClaimTypes.Email, email)
    };

    var roles = await userManager.GetRolesAsync(user!);
    foreach (var rol in roles)
        claims.Add(new Claim(ClaimTypes.Role, rol));

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(configuration["LlaveJWT"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expiration = DateTime.UtcNow.AddDays(30);

    var token = new JwtSecurityToken(
        issuer: null,
        audience: null,
        claims: claims,
        expires: expiration,
        signingCredentials: creds);

    return new AuthenticationResponseDto
    {
        Token = new JwtSecurityTokenHandler().WriteToken(token),
        Expiration = expiration,
        UserId = user!.Id
    };
}

[HttpGet("renew")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public async Task<ActionResult<AuthenticationResponseDto>> Renew()
{
    var emailClaim = HttpContext.User.Claims
        .FirstOrDefault(x => x.Type == "email");
    return await BuildToken(emailClaim!.Value);
}
```

---

## 11. Estructura Final del Proyecto

```
RecetArreAPI/
├── Controllers/
│   ├── AccountsController.cs
│   ├── CategoriesController.cs
│   ├── RecipesController.cs
│   ├── ApplicationUsersController.cs
│   └── ... (más controllers)
├── Context/
│   └── ApplicationDbContext.cs
├── Models/
│   ├── ApplicationUser.cs
│   ├── Recipe.cs
│   ├── Category.cs
│   └── ... (más models)
├── DTOs/
│   ├── Identity/
│   ├── Categories/
│   ├── Recipes/
│   └── ... (más DTOs)
├── Profiles/
│   └── AutoMapperProfile.cs
├── Migrations/
│   ├── [timestamp]_InitialCreate.cs
│   └── ApplicationDbContextModelSnapshot.cs
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

---

## 12. Temas para Profundizar en Clase

### A. ALTA PRIORIDAD (Fundamental)

#### 1. **Relaciones en EF Core (1:1, 1:N, N:N)**
- Ejemplo: Recipe → Author (1:N)
- Ejemplo: Recipe ↔ Category (N:N vía RecipeCategory)
- Lazy Loading vs Eager Loading
- Ejercicio: Crear nuevas relaciones y migraciones

#### 2. **Validación de Datos (DataAnnotations)**
- `[Required]`, `[StringLength]`, `[Range]`, `[EmailAddress]`
- Validación en DTOs vs Models
- Custom Validators
- Ejercicio: Agregar validaciones específicas por negocio

#### 3. **Flujo de Autenticación y Autorización**
- Diferencia: Authentication vs Authorization
- Cómo funcionan los Claims
- Roles basados en acceso
- Proteger endpoints con `[Authorize]`
- Ejercicio: Agregar roles (Admin, User) y permisos

#### 4. **Async/Await en .NET**
- Por qué es importante en APIs
- `Task`, `Task<T>`, `async`, `await`
- Deadlocks comunes
- Ejercicio: Refactorizar métodos síncronos a asincronos

### B. MEDIA PRIORIDAD (Importante)

#### 5. **Operaciones CRUD Avanzadas**
- Incluir relaciones: `.Include(x => x.RelatedEntity)`
- Paginación: Skip y Take
- Ordenamiento dinámico
- Filtros complejos
- Ejercicio: Implementar búsqueda de recetas con filtros

#### 6. **Patrones de Diseño en APIs**
- Repository Pattern
- Unit of Work Pattern
- Dependency Injection
- Ejercicio: Refactorizar a Repository Pattern

#### 7. **Manejo de Errores**
- Try-catch específicos
- Logging (Serilog)
- Custom Exception Handling Middleware
- Ejercicio: Crear middleware de errores global

#### 8. **Versionado de APIs**
- Versionado por URL vs Header
- API Versioning con Swagger
- Deprecación de endpoints
- Ejercicio: Implementar v2 de un endpoint

### C. BAJA PRIORIDAD (Ampliar Horizonte)

#### 9. **Performance y Optimización**
- N+1 Query Problem
- Query Optimization
- Caching (Redis)
- Índices de BD

#### 10. **Seguridad Avanzada**
- Rate Limiting
- CORS
- SQL Injection Prevention
- CSRF Protection

#### 11. **Testing**
- Unit Tests (xUnit/NUnit)
- Integration Tests
- Mocking con Moq
- Ejercicio: Escribir tests para controllers

#### 12. **Deployment**
- Docker
- CI/CD con GitHub Actions
- Hosting en Azure / Railway

---

## Ejercicios Propuestos para Clase

### Nivel 1 - Consolidar Fundamentos
1. Crear un nuevo modelo `Tag` y relacionarlo con `Recipe` (N:N)
2. Agregar validaciones adicionales a `RecipeCreateDto`
3. Crear un endpoint para obtener todas las recetas de un usuario

### Nivel 2 - Expandir Funcionalidad
1. Implementar paginación en `GET /api/recipes`
2. Agregar búsqueda por título en recetas
3. Crear un endpoint para "recetas favoritas" (N:N con User)
4. Implementar soft delete para recipes

### Nivel 3 - Arquitectura
1. Refactorizar a Repository Pattern
2. Crear custom exception classes
3. Implementar logging global
4. Agregar rate limiting a endpoints públicos

### Nivel 4 - Testing
1. Escribir tests unitarios para AutoMapperProfile
2. Crear integration tests para AccountsController
3. Mock de ApplicationDbContext para tests

---

## Checklist de Implementación

- [ ] NuGet packages instalados
- [ ] appsettings.json configurado
- [ ] ApplicationDbContext creado
- [ ] Models implementados
- [ ] AutoMapper configurado
- [ ] DTOs creados por entidad
- [ ] Migrations iniciales aplicadas
- [ ] Controllers CRUD funcionando
- [ ] JWT autenticación implementada
- [ ] Endpoints probados en Swagger
- [ ] Validación de datos activa
- [ ] Error handling mejorado
- [ ] Documentación en Swagger

---

## Recursos Adicionales

### Microsoft Docs
- [EF Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [JWT Bearer Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn)
- [AutoMapper Documentation](https://docs.automapper.org/)

### Librerias Recomendadas para Futuros Proyectos
- **Serilog**: Logging estructurado
- **FluentValidation**: Validación más potente
- **MediatR**: Implementar CQRS
- **Polly**: Resilience y retry policies
- **EF Core Power Tools**: Reverse engineering BD

---

## Conclusión

Este proyecto cubre los conceptos **esenciales** de una API REST profesional en .NET:
✅ Autenticación y Autorización
✅ ORM con EF Core
✅ Mapeo de datos con AutoMapper
✅ Validación robusta
✅ Relaciones complejas
✅ RESTful design

**Siguiente paso lógico:** Agregar Repository Pattern y aplicar Clean Architecture.
