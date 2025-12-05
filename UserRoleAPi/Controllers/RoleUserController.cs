using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserRoleAPi.Models;
using UserRoleAPi.Models.Dtos;

namespace UserRoleAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleUserController : ControllerBase
    {
        private readonly UserRoleDbContext _context;
        public RoleUserController(UserRoleDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewRoleToUser(AddNewSwitchDto roleUser)
        {
            try
            {
                bool exists = await _context.roleusers
                    .AnyAsync(ru => ru.UserId == roleUser.UsersId && ru.RoleId == roleUser.RolesId);

                if (!exists)
                {
                    var roleuser = new RoleUser
                    {

                        RoleId = roleUser.RolesId,
                        UserId = roleUser.UsersId

                    };

                    await _context.roleusers.AddAsync(roleuser);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Sikeres összerendelés.", result = new { roleuser.UserId, roleuser.RoleId } });
                }

                return BadRequest(new { message = "Sikertelen összerendelés.", result = "" });
            }
            catch (Exception ex)
            {

                return StatusCode(400, new { message = ex.Message, result = "" });
            }

        }
    }
}
