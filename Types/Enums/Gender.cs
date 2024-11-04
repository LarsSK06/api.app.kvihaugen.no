using System.Text.Json.Serialization;

namespace KvihaugenIdentityAPI.Types.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender{
    Male,
    Female,
    Other,
    Undefined
}