using System.Collections.Immutable;
using System.Text.Json;

namespace Colabora.Application.Shared;

public static class Defaults
{
    public static JsonSerializerOptions JsonSerializerOptions => new() {PropertyNameCaseInsensitive = true};
}