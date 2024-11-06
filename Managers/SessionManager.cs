using KvihaugenAppAPI.Classes;
using KvihaugenAppAPI.Models;
using KvihaugenAppAPI.Utilities;
using MongoDB.Driver;

namespace KvihaugenAppAPI.Managers;

public readonly struct SessionManager{

    readonly static List<Session> sessions = [];

    public static async Task<string> Create(User user){
        Session session = new(await UniqueToken(), user);

        sessions.Add(session);

        return session.Token;
    }

    public static async Task<Session?> GetAsync(string token, bool ignoreSync = false){
        foreach(Session i in sessions){
            if(i.Token != token) continue;
            
            if(!ignoreSync){
                IAsyncCursor<User> cursor =
                    await DatabaseManager.Users.FindAsync(x => x.Id == i.User.Id);
                
                User? user = await cursor.FirstOrDefaultAsync();

                if(user is null || !user.Active){
                    sessions.Remove(i);
                    return null;
                }
                else i.User = user;
            }

            return i;
        }

        return null;
    }

    public static async Task<Session?> GetAsync(int userId, bool ignoreSync = false){
        foreach(Session i in sessions){
            if(i.User.Id != userId) continue;
            
            if(!ignoreSync){
                IAsyncCursor<User> cursor =
                    await DatabaseManager.Users.FindAsync(x => x.Id == i.User.Id);
                
                User? user = await cursor.FirstOrDefaultAsync();

                if(user is null || !user.Active){
                    sessions.Remove(i);
                    return null;
                }
                else i.User = user;
            }

            return i;
        }

        return null;
    }

    static async Task<string> UniqueToken(){
        string result = Crypto.RandomString();

        while(await GetAsync(result) is not null)
            result = Crypto.RandomString();
        
        return result;
    }

}