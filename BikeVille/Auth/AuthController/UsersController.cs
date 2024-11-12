using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeVille.Auth;
using BikeVille.Auth.AuthContext;
using BikeVille.Entity.EntityContext;

namespace BikeVille.Auth.AuthController
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AdventureWorksLt2019usersInfoContext _authContext;
       

        public UsersController(AdventureWorksLt2019usersInfoContext authContext)
        {
            _authContext = authContext;
            
        }

        // GET: api/Users
        [HttpGet("Index")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _authContext.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _authContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _authContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _authContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Add")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _authContext.Users.Add(user);
            await _authContext.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _authContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _authContext.Users.Remove(user);
            await _authContext.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _authContext.Users.Any(e => e.UserId == id);
        }
    }
}
