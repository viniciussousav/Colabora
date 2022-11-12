namespace Colabora.Application.Shared;

public class Result<T> : Result
{
    public Result(T? data)
    {
        Data = data;
    }
    
    public T? Data;
    
    public static implicit operator Result<T>(T? data) => new(data);
}

public class Result
{
    public Result() { }
    
    public Result(params Error[] errors)
    {
        Errors.AddRange(errors);
    }
    
    public List<Error> Errors { get; } = new();

    public bool ContainsErrors => Errors.Any();
}