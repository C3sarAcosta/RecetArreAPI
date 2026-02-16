using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.UserProfiles;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserProfilesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserProfileDto>>> Get()
        {
            var profiles = await context.UserProfiles.AsNoTracking().ToListAsync();
            return mapper.Map<List<UserProfileDto>>(profiles);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserProfileDto>> GetById(string userId)
        {
            var profile = await context.UserProfiles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            if (profile is null)
            {
                return NotFound();
            }

            return mapper.Map<UserProfileDto>(profile);
        }

        [HttpPost]
        public async Task<ActionResult<UserProfileDto>> Create(UserProfileCreateDto dto)
        {
            var profile = mapper.Map<UserProfile>(dto);
            context.UserProfiles.Add(profile);
            await context.SaveChangesAsync();

            var response = mapper.Map<UserProfileDto>(profile);
            return CreatedAtAction(nameof(GetById), new { userId = profile.UserId }, response);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(string userId, UserProfileUpdateDto dto)
        {
            var profile = await context.UserProfiles.FindAsync(userId);
            if (profile is null)
            {
                return NotFound();
            }

            mapper.Map(dto, profile);
            profile.UpdatedAtUtc = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            var profile = await context.UserProfiles.FindAsync(userId);
            if (profile is null)
            {
                return NotFound();
            }

            context.UserProfiles.Remove(profile);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
