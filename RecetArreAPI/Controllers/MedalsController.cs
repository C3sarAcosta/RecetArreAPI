using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.Medals;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedalsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public MedalsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<MedalDto>>> Get()
        {
            var medals = await context.Medals.AsNoTracking().ToListAsync();
            return mapper.Map<List<MedalDto>>(medals);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MedalDto>> GetById(int id)
        {
            var medal = await context.Medals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (medal is null)
            {
                return NotFound();
            }

            return mapper.Map<MedalDto>(medal);
        }

        [HttpPost]
        public async Task<ActionResult<MedalDto>> Create(MedalCreateDto dto)
        {
            var medal = mapper.Map<Medal>(dto);
            context.Medals.Add(medal);
            await context.SaveChangesAsync();

            var response = mapper.Map<MedalDto>(medal);
            return CreatedAtAction(nameof(GetById), new { id = medal.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, MedalUpdateDto dto)
        {
            var medal = await context.Medals.FindAsync(id);
            if (medal is null)
            {
                return NotFound();
            }

            mapper.Map(dto, medal);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var medal = await context.Medals.FindAsync(id);
            if (medal is null)
            {
                return NotFound();
            }

            context.Medals.Remove(medal);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
