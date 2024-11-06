using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using KvihaugenAppAPI.Types.Enums;

namespace KvihaugenAppAPI.Models;

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
    public required bool Administrator { get; set; }

    [BsonElement("active")]
    [BsonRepresentation(BsonType.Boolean)]
    public required bool Active { get; set; }

    public SoftMutableUser ToSoftMutable(){
        return new(){
            FirstName = FirstName,
            LastName = LastName,
            Gender = Gender,
            Email = Email
        };
    }

    public HardMutableUser ToHardMutable(){
        return new(){
            FirstName = FirstName,
            LastName = LastName,
            Gender = Gender,
            Email = Email,
            Administrator = Administrator,
            Active = Active
        };
    }

    public PublicUser ToPublic(){
        return new(){
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            Gender = Gender,
            Email = Email,
            Administrator = Administrator,
            Active = Active
        };
    }

}

public class SoftMutableUser{

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public required string Email { get; set; }

}

public class HardMutableUser{

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public required string Email { get; set; }
    public required bool Administrator { get; set; }
    public required bool Active { get; set; }

}

public class PublicUser{

    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public required string Email { get; set; }
    public required bool Administrator { get; set; }
    public required bool Active { get; set; }

}