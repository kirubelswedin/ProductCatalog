using Resources.Shared.Models;

namespace Resources.Shared.Factories;

public static class ResultResponseFactory
{
    public static ResultResponse<T> Success<T>(T result, string? message = null)
    {
        return new ResultResponse<T>
        {
            Success = true,
            Result = result,
            Message = message
        };
    }
    
    public static ResultResponse<T> Failure<T>(string? message = null)
    {
        return new ResultResponse<T>
        {
            Success = false,
            Message = message
        };
    }

    public static ResultResponse<T> InvalidData<T>(string? message = null)
    {
        return new ResultResponse<T>
        {
            Success = false,
            Message = message
        };
    }

    public static ResultResponse<T> NotFound<T>(string? message = null)
    {
        return new ResultResponse<T>
        {
            Success = false,
            Message = message
        };
    }

    public static ResultResponse<T> Exists<T>(string? message = null!)
    {
        return new ResultResponse<T>
        {
            Success = false,
            Message = message
        };
    }

    public static ResultResponse<T> Exception<T>(Exception ex)
    {
        return new ResultResponse<T>
        {
            Success = false,
            Message = ex.Message
        };
    }
    
    public static ResultResponse<T> Empty<T>(string? message = null)
    {
        return new ResultResponse<T>
        {
            Success = true,
            Result = default,
            Message = message
        };
    }
}