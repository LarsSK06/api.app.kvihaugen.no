using KvihaugenIdentityAPI.Models;

namespace KvihaugenIdentityAPI.Classes;

public class Session{

    public string Token { get; private set; }
    public User User { get; set; }
    public DateTime Expires { get; private set; } = DateTime.Now.AddMinutes(5);

    public Session(string token, User user){
        Token = token;
        User = user;
    }

    public bool Expired{
        get => DateTime.Now > Expires;
    }

    public void Touch(){
        Expires = DateTime.Now.AddMinutes(5);
    }

}