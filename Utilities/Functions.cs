using Microsoft.Extensions.Primitives;

namespace KvihaugenAppAPI.Utilities;

public readonly struct Functions{

    public static int GetEpoch(){
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public static string? GetAuthHeader(HttpRequest request){
        if(!request.Headers.TryGetValue("Authorization", out StringValues header))
            return null;

        return header.ToString();
    }

    public static string? GetBearerToken(HttpRequest request){
        string? header = GetAuthHeader(request);

        if(header is null)
            return null;
        
        string[] split = header.Split(" ");

        if(split.Length < 2)
            return null;
        
        return split[1];
    }

    public static async Task<string> RequestBodyToString(HttpRequest request){
        if(!request.Body.CanSeek)
            request.EnableBuffering();
        
        request.Body.Position = 0;

        StreamReader reader = new(request.Body);

        string body = await reader.ReadToEndAsync().ConfigureAwait(false);

        request.Body.Position = 0;

        return body;
    }

}