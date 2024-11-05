using KvihaugenIdentityAPI.Attributes;
using KvihaugenIdentityAPI.Managers;
using KvihaugenIdentityAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace KvihaugenIdentityAPI.Controllers;

[VerifyIdentity(admin: true)]
[ApiController]
[Route("users")]
public class UsersController : ControllerBase{

    [HttpGet]
    public async Task<ActionResult<List<PublicUser>>> GetUsers(){
        IAsyncCursor<User> cursor =
            await DatabaseManager.Users.FindAsync(Builders<User>.Filter.Empty);

        List<User> users = await cursor.ToListAsync();
        List<PublicUser> publicUsers = [];

        foreach(User i in users)
            publicUsers.Add(i.ToPublic());

        return Ok(publicUsers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PublicUser>> GetUser(int id){
        IAsyncCursor<User> cursor =
            await DatabaseManager.Users.FindAsync(i => i.Id == id);

        User? user = await cursor.FirstOrDefaultAsync();

        return user is null
            ? NotFound()
            : Ok(user.ToPublic());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PublicUser>> EditUser(int id, HardMutableUser data){
        if(DatabaseManager.Users is null)
            return NotFound();

        FilterDefinition<User> filter =
            Builders<User>.Filter.Where(i => i.Id == id);

        IAsyncCursor<User> cursor =
            await DatabaseManager.Users.FindAsync(filter);

        User? user = await cursor.FirstOrDefaultAsync();

        if(user is null)
            return NotFound();

        UpdateDefinition<User> update =
            Builders<User>.Update
                .Set(i => i.FirstName, data.FirstName)
                .Set(i => i.LastName, data.LastName)
                .Set(i => i.Gender, data.Gender)
                .Set(i => i.Email, data.Email)
                .Set(i => i.Administrator, data.Administrator)
                .Set(i => i.Active, data.Active);

        await DatabaseManager.Users.UpdateOneAsync(filter, update);

        user = await cursor.FirstOrDefaultAsync();

        return Ok(user.ToPublic());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<PublicUser>> DeleteUser(int id){
        if(DatabaseManager.Users is null)
            return NotFound();

        FilterDefinition<User> filter =
            Builders<User>.Filter.Where(i => i.Id == id);

        IAsyncCursor<User> cursor =
            await DatabaseManager.Users.FindAsync(filter);

        User? user = await cursor.FirstOrDefaultAsync();

        if(user is null)
            return NotFound();

        await DatabaseManager.Users.DeleteOneAsync(filter);

        return Ok(user.ToPublic());
    }

}