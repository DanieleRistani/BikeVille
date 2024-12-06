using AuthJwt.Mail;
using BikeVille.Auth.AuthContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeVille.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
       private AdventureWorksLt2019usersInfoContext _context;
        public AdminController(AdventureWorksLt2019usersInfoContext context)
        {
            _context = context;
        }

        [HttpGet("toBeAdmin/{email}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> toBeAdmin(string email)
        {
            if(email != null)
            {
                if(_context.Users.Any(x => x.EmailAddress == email))
                {
                    _context.Users.FirstOrDefault(x => x.EmailAddress == email).Role ="ADMIN";
                    await _context.SaveChangesAsync();
                    return Ok("User is now an admin");

                }else
                {
                   return  BadRequest("Email not found");
                }
            }
            else
            {
                return BadRequest("Email null");
            }
           
        }

    }
}
