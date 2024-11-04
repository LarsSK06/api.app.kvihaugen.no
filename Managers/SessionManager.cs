using KvihaugenIdentityAPI.Classes;
using KvihaugenIdentityAPI.Models;
using MongoDB.Driver;

namespace KvihaugenIdentityAPI.Managers;

public readonly struct SessionManager{

    readonly static List<Session> sessions = [];

    public static string Create(User user){
        Session session = new(user);

        sessions.Add(session);

        return session.Token;
    }

    public static async Task<Session?> GetAsync(string token, bool ignoreSync = false){
        foreach(Session i in sessions){
            if(i.Token != token) continue;
            
            if(!ignoreSync){
                IAsyncCursor<User>? cursor =
                    await DatabaseManager.Users.FindAsync(x => x.Id == i.User.Id);
                
                User? user = await cursor.FirstOrDefaultAsync();

                if(user is null){
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
                IAsyncCursor<User>? cursor =
                    await DatabaseManager.Users.FindAsync(x => x.Id == i.User.Id);
                
                User? user = await cursor.FirstOrDefaultAsync();

                if(user is null){
                    sessions.Remove(i);
                    return null;
                }
                else i.User = user;
            }

            return i;
        }

        return null;
    }

}