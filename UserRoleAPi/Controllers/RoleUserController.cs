using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
           
                var roleuser = new RoleUser
                {
                   
                     RoleId = roleUser.RolesId,
                     UserId = roleUser.UsersId

                };

                await _context.roleusers.AddAsync(roleuser);
                await _context.SaveChangesAsync();
                return Ok(roleuser);
          
        }
    }
}
