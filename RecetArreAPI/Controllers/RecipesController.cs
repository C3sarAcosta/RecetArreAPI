using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.Recipes;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public RecipesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<RecipeDto>>> Get()
        {
            var recipes = await context.Recipes.AsNoTracking().ToListAsync();
            return mapper.Map<List<RecipeDto>>(recipes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RecipeDto>> GetById(int id)
        {
            var recipe = await context.Recipes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (recipe is null)
            {
                return NotFound();
            }

            return mapper.Map<RecipeDto>(recipe);
        }

        [HttpPost]
        public async Task<ActionResult<RecipeDto>> Create(RecipeCreateDto dto)
        {
            var recipe = mapper.Map<Recipe>(dto);
            context.Recipes.Add(recipe);
            await context.SaveChangesAsync();

            var response = mapper.Map<RecipeDto>(recipe);
            return CreatedAtAction(nameof(GetById), new { id = recipe.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, RecipeUpdateDto dto)
        {
            var recipe = await context.Recipes.FindAsync(id);
            if (recipe is null)
            {
                return NotFound();
            }

            mapper.Map(dto, recipe);
            recipe.UpdatedAtUtc = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await context.Recipes.FindAsync(id);
            if (recipe is null)
            {
                return NotFound();
            }

            context.Recipes.Remove(recipe);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
