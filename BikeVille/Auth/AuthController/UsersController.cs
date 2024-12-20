﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeVille.Auth;
using BikeVille.Auth.AuthContext;
using BikeVille.Entity.EntityContext;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using BikeVille.CriptingDecripting;

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

        [HttpGet("UserPass/{password}/{id}")]
        public async Task<ActionResult<bool>> GetUserPass(int id, string password)
        {
            var user = await _authContext.Users.FindAsync(id);
            if(SaltEncrypt.SaltDecryptPass(password, user.PasswordSalt) == user.PasswordHash){
                return true;
            }
            return false;
            
        }



        [HttpGet("AuthUser/{emailAddress}")]
        public async Task<ActionResult<User>> GetUser(string emailAddress)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.EmailAddress.Equals(emailAddress));

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
        [HttpPut("UpdatePass")]
        public async Task<IActionResult> PutUpdatePass([FromBody] ChangePassRequest changePassRequest)
        {
            var user = await _authContext.Users.FindAsync(changePassRequest.Id);
            KeyValuePair<string, string> passHashSalt = SaltEncrypt.SaltEncryptPass(changePassRequest.Password);
            if (user == null)
            {
                return BadRequest();
            }
            user.PasswordHash = passHashSalt.Key;
            user.PasswordSalt = passHashSalt.Value;
            _authContext.Entry(user).State = EntityState.Modified;
                await _authContext.SaveChangesAsync();
           
            return NoContent();

           
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Add")]
        public async Task<ActionResult<User>> PostUser(UserDto userDto)
        {
            KeyValuePair<string, string> passHashSalt = SaltEncrypt.SaltEncryptPass(userDto.Password);

            var user =new User()
            {
                FirstName = userDto.FirstName,
                MiddleName = userDto.MiddleName,
                LastName = userDto.LastName,
                Suffix = userDto.Suffix,
                EmailAddress = userDto.EmailAddress,
                Phone = userDto.Phone,
                PasswordHash = passHashSalt.Key,
                PasswordSalt = passHashSalt.Value,
                Role = userDto.Role,
                Rowguid=Guid.NewGuid(),

            };
            
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
