using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using KvihaugenIdentityAPI.Types.Enums;

namespace KvihaugenIdentityAPI.Models;

public class User{

    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? MongoId { get; set; }

    [BsonElement("id")]
    [BsonRepresentation(BsonType.Int32)]
    public required int Id { get; set; }

    [BsonElement("firstName")]
    [BsonRepresentation(BsonType.String)]
    public required string FirstName { get; set; }

    [BsonElement("lastName")]
    [BsonRepresentation(BsonType.String)]
    public required string LastName { get; set; }

    [BsonElement("gender")]
    [BsonRepresentation(BsonType.String)]
    public required Gender Gender { get; set; }

    [BsonElement("email")]
    [BsonRepresentation(BsonType.String)]
    public required string Email { get; set; }

    [BsonElement("password")]
    [BsonRepresentation(BsonType.String)]
    public required string Password { get; set; }

    [BsonElement("administrator")]
    [BsonRepresentation(BsonType.Boolean)]
    public required string Administrator { get; set; }

}