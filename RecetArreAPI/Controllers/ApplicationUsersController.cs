using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.DTOs.ApplicationUsers;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ApplicationUsersController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationUserDto>>> Get()
        {
            var users = await userManager.Users.AsNoTracking().ToListAsync();
            return mapper.Map<List<ApplicationUserDto>>(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserDto>> GetById(string id)
        {
            var user = await userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                return NotFound();
            }

            return mapper.Map<ApplicationUserDto>(user);
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUserDto>> Create(ApplicationUserCreateDto dto)
        {
            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = string.IsNullOrWhiteSpace(dto.UserName) ? dto.Email : dto.UserName,
                DisplayName = dto.DisplayName
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var response = mapper.Map<ApplicationUserDto>(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ApplicationUserUpdateDto dto)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            user.DisplayName = dto.DisplayName;
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                user.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.UserName))
            {
                user.UserName = dto.UserName;
            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}
