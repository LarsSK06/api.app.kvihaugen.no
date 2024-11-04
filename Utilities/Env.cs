namespace KvihaugenIdentityAPI.Utilities;

public readonly struct Env{

    public static string? GetVariable(string key){
        string filePath = $".env/{key}";

        if(!File.Exists(filePath)){
            Console.WriteLine($"Could not find environment variable {key}");
            return null;
        }

        return File.ReadAllText($".env/{key}");
    }

    public static async Task<string?> GetVariableAsync(string key){
        string filePath = $".env/{key}";

        if(!File.Exists(filePath)){
            Console.WriteLine($"Could not find environment variable {key}");
            return null;
        }
        
        return await File.ReadAllTextAsync($".env/{key}");
    }

}