namespace KvihaugenIdentityAPI.Utilities;

public readonly struct Env{

    public static string? GetVariable(string key){
        return File.ReadAllText($"../.env/{key}");
    }

    public static async Task<string?> GetVariableAsync(string key){
        return await File.ReadAllTextAsync($"../.env/{key}");
    }

}