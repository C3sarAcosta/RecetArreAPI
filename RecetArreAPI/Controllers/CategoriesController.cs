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

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> Get()
        {
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return mapper.Map<List<CategoryDto>>(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                return NotFound();
            }

            return mapper.Map<CategoryDto>(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create(CategoryCreateDto dto)
        {
            var category = mapper.Map<Category>(dto);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var response = mapper.Map<CategoryDto>(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            var category = await context.Categories.FindAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            mapper.Map(dto, category);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
