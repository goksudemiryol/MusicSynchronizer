namespace MusicSynchronizer.Domain.Models;

public class ServiceResult<TModel>
{
    public ServiceResult()
    {
        Success = true;
        StatusCode = 200;
    }

    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public TModel? Data { get; set; }

    public string? ErrorMessage { get; set; }
}
