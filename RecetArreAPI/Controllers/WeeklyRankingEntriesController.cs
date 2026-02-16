using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.WeeklyRankingEntries;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeeklyRankingEntriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public WeeklyRankingEntriesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<WeeklyRankingEntryDto>>> Get()
        {
            var entries = await context.WeeklyRankingEntries.AsNoTracking().ToListAsync();
            return mapper.Map<List<WeeklyRankingEntryDto>>(entries);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WeeklyRankingEntryDto>> GetById(int id)
        {
            var entry = await context.WeeklyRankingEntries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (entry is null)
            {
                return NotFound();
            }

            return mapper.Map<WeeklyRankingEntryDto>(entry);
        }

        [HttpPost]
        public async Task<ActionResult<WeeklyRankingEntryDto>> Create(WeeklyRankingEntryCreateDto dto)
        {
            var entry = mapper.Map<WeeklyRankingEntry>(dto);
            context.WeeklyRankingEntries.Add(entry);
            await context.SaveChangesAsync();

            var response = mapper.Map<WeeklyRankingEntryDto>(entry);
            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, WeeklyRankingEntryUpdateDto dto)
        {
            var entry = await context.WeeklyRankingEntries.FindAsync(id);
            if (entry is null)
            {
                return NotFound();
            }

            mapper.Map(dto, entry);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entry = await context.WeeklyRankingEntries.FindAsync(id);
            if (entry is null)
            {
                return NotFound();
            }

            context.WeeklyRankingEntries.Remove(entry);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
