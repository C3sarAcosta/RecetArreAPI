using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.Ratings;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public RatingsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<RatingDto>>> Get()
        {
            var ratings = await context.Ratings.AsNoTracking().ToListAsync();
            return mapper.Map<List<RatingDto>>(ratings);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RatingDto>> GetById(int id)
        {
            var rating = await context.Ratings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (rating is null)
            {
                return NotFound();
            }

            return mapper.Map<RatingDto>(rating);
        }

        [HttpPost]
        public async Task<ActionResult<RatingDto>> Create(RatingCreateDto dto)
        {
            var rating = mapper.Map<Rating>(dto);
            context.Ratings.Add(rating);
            await context.SaveChangesAsync();

            var response = mapper.Map<RatingDto>(rating);
            return CreatedAtAction(nameof(GetById), new { id = rating.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, RatingUpdateDto dto)
        {
            var rating = await context.Ratings.FindAsync(id);
            if (rating is null)
            {
                return NotFound();
            }

            mapper.Map(dto, rating);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rating = await context.Ratings.FindAsync(id);
            if (rating is null)
            {
                return NotFound();
            }

            context.Ratings.Remove(rating);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
