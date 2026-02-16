using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.Ingredients;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public IngredientsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<IngredientDto>>> Get()
        {
            var ingredients = await context.Ingredients.AsNoTracking().ToListAsync();
            return mapper.Map<List<IngredientDto>>(ingredients);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<IngredientDto>> GetById(int id)
        {
            var ingredient = await context.Ingredients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (ingredient is null)
            {
                return NotFound();
            }

            return mapper.Map<IngredientDto>(ingredient);
        }

        [HttpPost]
        public async Task<ActionResult<IngredientDto>> Create(IngredientCreateDto dto)
        {
            var ingredient = mapper.Map<Ingredient>(dto);
            context.Ingredients.Add(ingredient);
            await context.SaveChangesAsync();

            var response = mapper.Map<IngredientDto>(ingredient);
            return CreatedAtAction(nameof(GetById), new { id = ingredient.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, IngredientUpdateDto dto)
        {
            var ingredient = await context.Ingredients.FindAsync(id);
            if (ingredient is null)
            {
                return NotFound();
            }

            mapper.Map(dto, ingredient);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ingredient = await context.Ingredients.FindAsync(id);
            if (ingredient is null)
            {
                return NotFound();
            }

            context.Ingredients.Remove(ingredient);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
