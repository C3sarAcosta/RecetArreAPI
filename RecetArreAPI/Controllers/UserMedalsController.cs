using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.UserMedals;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserMedalsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserMedalsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserMedalDto>>> Get()
        {
            var items = await context.UserMedals.AsNoTracking().ToListAsync();
            return mapper.Map<List<UserMedalDto>>(items);
        }

        [HttpGet("{userId}/{medalId:int}")]
        public async Task<ActionResult<UserMedalDto>> GetById(string userId, int medalId)
        {
            var item = await context.UserMedals.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.MedalId == medalId);

            if (item is null)
            {
                return NotFound();
            }

            return mapper.Map<UserMedalDto>(item);
        }

        [HttpPost]
        public async Task<ActionResult<UserMedalDto>> Create(UserMedalCreateDto dto)
        {
            var item = mapper.Map<UserMedal>(dto);
            context.UserMedals.Add(item);
            await context.SaveChangesAsync();

            var response = mapper.Map<UserMedalDto>(item);
            return CreatedAtAction(nameof(GetById), new { userId = item.UserId, medalId = item.MedalId }, response);
        }

        [HttpPut("{userId}/{medalId:int}")]
        public async Task<IActionResult> Update(string userId, int medalId, UserMedalUpdateDto dto)
        {
            var item = await context.UserMedals.FindAsync(userId, medalId);
            if (item is null)
            {
                return NotFound();
            }

            mapper.Map(dto, item);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{userId}/{medalId:int}")]
        public async Task<IActionResult> Delete(string userId, int medalId)
        {
            var item = await context.UserMedals.FindAsync(userId, medalId);
            if (item is null)
            {
                return NotFound();
            }

            context.UserMedals.Remove(item);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
