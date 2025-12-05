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

        [HttpDelete]
        public async Task<ActionResult> DeleteUseRoles(Guid userid)
        {

            try
            {
                var deltedUserRoles = await _context.roleusers
                    .Where(ru => ru.UserId == userid)
                    .ToListAsync();

                if (deltedUserRoles != null)
                {
                    foreach (var i in deltedUserRoles)
                    {
                        _context.roleusers.Remove(i);
                    }

                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Sikeres törlés.", result = deltedUserRoles });
                }

                return NotFound(new { message = "nincs ilyn id.", result = deltedUserRoles });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }

    }
}
