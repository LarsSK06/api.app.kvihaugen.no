namespace KvihaugenIdentityAPI.Utilities;

public readonly struct Crypto{

    static readonly char[] chars = [
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
    ];

    public static string RandomString(int length = 20){
        List<char> result = [];

        Random random = new();

        for(int i = 0; i < length; i++)
            result.Add(chars[random.Next(chars.Length - 1)]);
        
        return string.Join("", result);
    }

    public static string Hash(string input, int rounds = 16){
        return BCrypt.Net.BCrypt.EnhancedHashPassword(input, rounds);
    }

    public static bool Verify(string text, string hash){
        return BCrypt.Net.BCrypt.EnhancedVerify(text, hash);
    }

}