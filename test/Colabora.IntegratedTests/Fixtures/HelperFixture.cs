using System;
using System.Text.Json;

namespace Colabora.IntegrationTests.Fixtures;

public class HelperFixture
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public HelperFixture()
    {
        _jsonSerializerOptions = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
    }

    public T DeserializeToObject<T>(string content)
    {
        var deserializeResult = JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions)
            ?? throw new NullReferenceException();

        return deserializeResult;
    }
}