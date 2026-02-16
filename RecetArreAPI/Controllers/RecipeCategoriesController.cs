using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.RecipeCategories;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public RecipeCategoriesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<RecipeCategoryDto>>> Get()
        {
            var items = await context.RecipeCategories.AsNoTracking().ToListAsync();
            return mapper.Map<List<RecipeCategoryDto>>(items);
        }

        [HttpGet("{recipeId:int}/{categoryId:int}")]
        public async Task<ActionResult<RecipeCategoryDto>> GetById(int recipeId, int categoryId)
        {
            var item = await context.RecipeCategories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.RecipeId == recipeId && x.CategoryId == categoryId);

            if (item is null)
            {
                return NotFound();
            }

            return mapper.Map<RecipeCategoryDto>(item);
        }

        [HttpPost]
        public async Task<ActionResult<RecipeCategoryDto>> Create(RecipeCategoryCreateDto dto)
        {
            var item = mapper.Map<RecipeCategory>(dto);
            context.RecipeCategories.Add(item);
            await context.SaveChangesAsync();

            var response = mapper.Map<RecipeCategoryDto>(item);
            return CreatedAtAction(nameof(GetById), new { recipeId = item.RecipeId, categoryId = item.CategoryId }, response);
        }

        [HttpDelete("{recipeId:int}/{categoryId:int}")]
        public async Task<IActionResult> Delete(int recipeId, int categoryId)
        {
            var item = await context.RecipeCategories.FindAsync(recipeId, categoryId);
            if (item is null)
            {
                return NotFound();
            }

            context.RecipeCategories.Remove(item);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
