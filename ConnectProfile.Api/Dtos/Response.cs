namespace ConnectProfile.Api.Dtos;

public class Response<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static Response<T> Ok(T data) =>
        new() { Success = true, Data = data };

    public static Response<T> Fail(string message) =>
        new() { Success = false, Message = message };
}
