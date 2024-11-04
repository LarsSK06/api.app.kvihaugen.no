using KvihaugenIdentityAPI.Attributes;
using KvihaugenIdentityAPI.Managers;
using KvihaugenIdentityAPI.Models;
using KvihaugenIdentityAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace KvihaugenIdentityAPI.Controllers;

[VerifyIdentity(admin: true)]
[ApiController]
[Route("user-requests")]
public class UserRequestsController : ControllerBase{

    [HttpGet]
    public async Task<ActionResult<List<PublicUserRequest>>> GetUserRequests(){
        IAsyncCursor<UserRequest> cursor =
            await DatabaseManager.UserRequests.FindAsync(Builders<UserRequest>.Filter.Empty);

        List<UserRequest> userRequests = await cursor.ToListAsync();
        List<PublicUserRequest> publicUserRequests = [];

        foreach(UserRequest i in userRequests)
            publicUserRequests.Add(i.ToPublic());

        return Ok(publicUserRequests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PublicUserRequest>> GetUserRequest(int id){
        IAsyncCursor<UserRequest> cursor =
            await DatabaseManager.UserRequests.FindAsync(i => i.Id == id);

        UserRequest? userRequest = await cursor.FirstOrDefaultAsync();

        return userRequest is null
            ? NotFound()
            : Ok(userRequest.ToPublic());
    }

    [HttpHead("{id}/approve")]
    public async Task<ActionResult> ApproveUserRequest(int id){
        if(DatabaseManager.Users is null)
            return NotFound();

        if(DatabaseManager.UserRequests is null)
            return NotFound();

        FilterDefinition<UserRequest> filter =
            Builders<UserRequest>.Filter.Eq(i => i.Id, id);

        IAsyncCursor<UserRequest> cursor =
            await DatabaseManager.UserRequests.FindAsync(filter);
        
        UserRequest? userRequest = await cursor.FirstOrDefaultAsync();

        if(userRequest is null)
            return NotFound();
        
        await DatabaseManager.Users.InsertOneAsync(userRequest.User);

        await EmailManager.SendEmailAsync(
            userRequest.User.Email,
            "Your Kvihaugen ID request has been approved!",
            $"""
                Welcome to Kvihaugen ID!
                Your user has now been created.
            """
        );

        await DatabaseManager.UserRequests.DeleteOneAsync(filter);

        return NoContent();
    }

    [HttpPost("{id}/decline")]
    public async Task<ActionResult> DeclineUserRequest(int id){
        if(DatabaseManager.UserRequests is null)
            return NotFound();

        FilterDefinition<UserRequest> filter =
            Builders<UserRequest>.Filter.Eq(i => i.Id, id);

        IAsyncCursor<UserRequest> cursor =
            await DatabaseManager.UserRequests.FindAsync(filter);
        
        UserRequest? userRequest = await cursor.FirstOrDefaultAsync();

        await EmailManager.SendEmailAsync(
            userRequest.User.Email,
            "Your Kvihaugen ID request has been declined!",
            $"""
                Your Kvihaugen ID request has unfortunately been declined!
                Your requested user has been deleted.
                An administrator left this message:
                {await Functions.RequestBodyToString(Request)}
            """
        );

        await DatabaseManager.UserRequests.DeleteOneAsync(filter);

        return NoContent();
    }

}