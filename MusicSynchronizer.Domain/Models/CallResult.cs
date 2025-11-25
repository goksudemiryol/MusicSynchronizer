namespace MusicSynchronizer.Domain.Models;

public class CallResult<TResponseModel>
{
    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public TResponseModel? Result { get; set; }

    public string? ErrorMessage { get; set; }
}
