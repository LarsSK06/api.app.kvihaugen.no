using System.Text.Json.Serialization;

namespace KvihaugenAppAPI.Types.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender{
    Male,
    Female,
    Other,
    Undefined
}