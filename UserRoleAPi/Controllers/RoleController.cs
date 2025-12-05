using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UserRoleAPi.Models;
using UserRoleAPi.Models.Dtos;

namespace UserRoleAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UserRoleDbContext _context;
        public RoleController(UserRoleDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewRole(AddRoleDto addRoleDto)
        {
            try
            {
                var role = new Role
                {
                    Id = Guid.NewGuid(),
                    RoleName = addRoleDto.RoleName
                };

                if (role != null)
                {
                    await _context.roles.AddAsync(role);
                    await _context.SaveChangesAsync();
                    return StatusCode(201, new { message = "Sikeres hozzáadás", result = role });
                }

                return StatusCode(404, new { message = "Sikertelen hozzáadás", result = role });
            }
            catch (Exception ex)
            {

                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRole()
        {
            try
            {
                return Ok(new { message = "Sikeres lekérdezés", result = await _context.roles.ToListAsync() });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteRole(Guid id)
        {
            try
            {
                var role = _context.roles.FirstOrDefault(x => x.Id == id);
                if (role != null)
                {
                    _context.Remove(role);
                    await _context.SaveChangesAsync();
                    return StatusCode(200, new { message = "Sikeres törlés", result = role });
                }

                return StatusCode(404, new { message = "Nincs ilyen Id", result = role });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRole(Guid id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                var role = await _context.roles.FirstOrDefaultAsync(x => x.Id == id);

                if (role != null)
                {
                    role.RoleName = updateRoleDto.RoleName;

                    _context.roles.Update(role);
                    await _context.SaveChangesAsync();
                    return StatusCode(200, new { message = "Sikeres frissítés", result = role });
                }

                return StatusCode(404, new { message = "nincs ily Id", result = role });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }

        [HttpGet("roleWithUsers")]
        public async Task<ActionResult> GetRoleWithUsers(Guid id)
        {
            try
            {
                var rolewithUsers = await _context.roleusers
                    .Where(ru => ru.RoleId == id)
                    .Include(ru => ru.User)
                    .Select(ru => new { Role = ru.Role.RoleName, UserNames = ru.User.Name })
                    .ToListAsync();

                if (rolewithUsers != null)
                {
                    return StatusCode(200, new { message = "Sikeres lekérdezés", result = rolewithUsers });
                }

                return StatusCode(404, new { message = "nincs ily Id", result = rolewithUsers });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }
    }
}
