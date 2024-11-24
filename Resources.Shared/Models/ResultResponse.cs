namespace Resources.Shared.Models;

public class ResultResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Result { get; set; } 
}

