using KvihaugenIdentityAPI.Managers;
using KvihaugenIdentityAPI.Models;
using KvihaugenIdentityAPI.Types.Enums;
using KvihaugenIdentityAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace KvihaugenIdentityAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase{
    
    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUp(SignUpData data){
        if(DatabaseManager.Users is null)
            return NotFound();

        if(DatabaseManager.UserRequests is null)
            return NotFound();

        if(await DatabaseManager.Users.CountDocumentsAsync(
            i => i.Email.ToLower() == data.Email.ToLower()
        ) > 0) return Conflict("Email is already in use!");

        User user = new(){
            Id = Functions.GetEpoch(),
            FirstName = data.FirstName,
            LastName = data.LastName,
            Gender = data.Gender,
            Email = data.Email,
            Password = Crypto.Hash(data.Password),
            Administrator = data.Email.ToLower() == "lars@kvihaugen.no"
        };

        await DatabaseManager.UserRequests.InsertOneAsync(new(){
            Id = Functions.GetEpoch(),
            User = user
        });

        return NoContent();
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<Passport>> SignIn(SignInData data){
        if(DatabaseManager.Users is null)
            return NotFound();

        IAsyncCursor<User> cursor =
            await DatabaseManager.Users.FindAsync(
                i => i.Email.ToLower() == data.Email.ToLower()
            );
        
        User? user = await cursor.FirstOrDefaultAsync();

        if(user is null)
            return NotFound();

        if(!Crypto.Verify(data.Password, user.Password))
            return Unauthorized();

        string token = await SessionManager.Create(user);

        return Ok(new Passport(){
            Token = token,
            User = user.ToPublic()
        });
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

public class Passport{
    public required string Token { get; set; }
    public required PublicUser User { get; set; }
}