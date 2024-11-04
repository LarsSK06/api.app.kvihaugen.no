using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KvihaugenIdentityAPI.Models;

public class UserRequest{

    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? MongoId { get; set; }

    [BsonElement("id")]
    [BsonRepresentation(BsonType.Int32)]
    public required int Id { get; set; }

    [BsonElement("user")]
    public required User User { get; set; }

    public PublicUserRequest ToPublic(){
        return new(){
            Id = Id,
            User = User.ToPublic()
        };
    }

}

public class PublicUserRequest{

    public required int Id { get; set; }
    public required PublicUser User { get; set; }

}