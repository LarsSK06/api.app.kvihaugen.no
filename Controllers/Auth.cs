using KvihaugenAppAPI.Managers;
using KvihaugenAppAPI.Models;
using KvihaugenAppAPI.Types.Enums;
using KvihaugenAppAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace KvihaugenAppAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase{
    
    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUp(SignUpData data){
        if(DatabaseManager.Users is null)
            return NotFound();

        if(await DatabaseManager.Users.CountDocumentsAsync(
            i => i.Email.ToLower() == data.Email.ToLower()
        ) > 0) return Conflict("Email is already in use!");

        bool owner = data.Email.ToLower() == "lars@kvihaugen.no";

        User user = new(){
            Id = Functions.GetEpoch(),
            FirstName = data.FirstName,
            LastName = data.LastName,
            Gender = data.Gender,
            Email = data.Email,
            Password = Crypto.Hash(data.Password),
            Administrator = owner,
            Active = owner
        };

        await DatabaseManager.Users.InsertOneAsync(user);

        return NoContent();
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<string>> SignIn(SignInData data){
        if(DatabaseManager.Users is null)
            return NotFound();

        IAsyncCursor<User> cursor =
            await DatabaseManager.Users.FindAsync(
                Builders<User>.Filter.Or(
                    Builders<User>.Filter.Where(i => i.Email.ToLower() == data.Email.ToLower()),
                    Builders<User>.Filter.Where(i => i.Active)
                )
            );
        
        User? user = await cursor.FirstOrDefaultAsync();

        if(user is null)
            return NotFound();

        if(!Crypto.Verify(data.Password, user.Password))
            return Unauthorized();

        return Ok(await SessionManager.Create(user));
    }

}

public class SignUpData{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class SignInData{
    public required string Email { get; set; }
    public required string Password { get; set; }
}