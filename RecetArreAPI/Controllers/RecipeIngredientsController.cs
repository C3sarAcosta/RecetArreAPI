using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.RecipeIngredients;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeIngredientsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public RecipeIngredientsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<RecipeIngredientDto>>> Get()
        {
            var items = await context.RecipeIngredients.AsNoTracking().ToListAsync();
            return mapper.Map<List<RecipeIngredientDto>>(items);
        }

        [HttpGet("{recipeId:int}/{ingredientId:int}")]
        public async Task<ActionResult<RecipeIngredientDto>> GetById(int recipeId, int ingredientId)
        {
            var item = await context.RecipeIngredients.AsNoTracking()
                .FirstOrDefaultAsync(x => x.RecipeId == recipeId && x.IngredientId == ingredientId);

            if (item is null)
            {
                return NotFound();
            }

            return mapper.Map<RecipeIngredientDto>(item);
        }

        [HttpPost]
        public async Task<ActionResult<RecipeIngredientDto>> Create(RecipeIngredientCreateDto dto)
        {
            var item = mapper.Map<RecipeIngredient>(dto);
            context.RecipeIngredients.Add(item);
            await context.SaveChangesAsync();

            var response = mapper.Map<RecipeIngredientDto>(item);
            return CreatedAtAction(nameof(GetById), new { recipeId = item.RecipeId, ingredientId = item.IngredientId }, response);
        }

        [HttpPut("{recipeId:int}/{ingredientId:int}")]
        public async Task<IActionResult> Update(int recipeId, int ingredientId, RecipeIngredientUpdateDto dto)
        {
            var item = await context.RecipeIngredients.FindAsync(recipeId, ingredientId);
            if (item is null)
            {
                return NotFound();
            }

            mapper.Map(dto, item);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{recipeId:int}/{ingredientId:int}")]
        public async Task<IActionResult> Delete(int recipeId, int ingredientId)
        {
            var item = await context.RecipeIngredients.FindAsync(recipeId, ingredientId);
            if (item is null)
            {
                return NotFound();
            }

            context.RecipeIngredients.Remove(item);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
