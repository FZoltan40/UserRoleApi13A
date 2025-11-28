using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserRoleAPi.Models;
using UserRoleAPi.Models.Dtos;

namespace UserRoleAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRoleDbContext _context;
        public UserController(UserRoleDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewUser(AddUserDto addUserDto)
        {
            try
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = addUserDto.Name,
                    Email = addUserDto.Email,
                    Password = addUserDto.Password
                };

                if(user != null)
                {
                    await _context.users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    return StatusCode(201, new { message = "Sikeres hozzáadás", result = user });
                }

                return StatusCode(404, new { message = "Sikertelen hozzáadás", result = user });
            }
            catch (Exception ex)
            {

                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUser()
        {
            try
            {
                return Ok(new { message = "Sikeres lekérdezés", result = await _context.users.ToListAsync() });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            try
            {
                var user = _context.users.FirstOrDefault(x=>x.Id == id);
                if(user!= null)
                {
                    _context.Remove(user);
                    await _context.SaveChangesAsync();
                    return StatusCode(200, new { message = "Sikeres törlés", result = user });
                }

                return StatusCode(404, new { message = "Nincs ilyen Id", result = user });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(Guid id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.Id == id);

                if( user!=null)
                {
                    user.Name = updateUserDto.Name;
                    user.Email = updateUserDto.Email;
                    user.Password = updateUserDto.Password;

                    _context.users.Update(user);
                    await _context.SaveChangesAsync();
                    return StatusCode(200, new { message = "Sikeres frissítés", result = user });
                }

                return StatusCode(404, new { message = "nincs ily Id", result = user });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message, result = "" });
            }
        }
    }
}
