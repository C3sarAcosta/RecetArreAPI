using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Context;
using RecetArreAPI.DTOs.Comments;
using RecetArreAPI.Models;

namespace RecetArreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CommentsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentDto>>> Get()
        {
            var comments = await context.Comments.AsNoTracking().ToListAsync();
            return mapper.Map<List<CommentDto>>(comments);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CommentDto>> GetById(int id)
        {
            var comment = await context.Comments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (comment is null)
            {
                return NotFound();
            }

            return mapper.Map<CommentDto>(comment);
        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> Create(CommentCreateDto dto)
        {
            var comment = mapper.Map<Comment>(dto);
            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            var response = mapper.Map<CommentDto>(comment);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CommentUpdateDto dto)
        {
            var comment = await context.Comments.FindAsync(id);
            if (comment is null)
            {
                return NotFound();
            }

            mapper.Map(dto, comment);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await context.Comments.FindAsync(id);
            if (comment is null)
            {
                return NotFound();
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
