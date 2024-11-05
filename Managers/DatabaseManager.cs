using KvihaugenIdentityAPI.Models;
using KvihaugenIdentityAPI.Utilities;
using MongoDB.Driver;

namespace KvihaugenIdentityAPI.Managers;

public readonly struct DatabaseManager{

    static readonly string Identity = "identity";

    public readonly static IMongoCollection<User>? Users =
        new MongoClient(Env.GetVariable("DatabaseConnectionString"))
            .GetDatabase(Identity)
            .GetCollection<User>("users");

}