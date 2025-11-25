using MusicSynchronizer.Domain.Models;

namespace MusicSynchronizer.Domain.Interfaces.Integration;

public interface IIntegrationService
{
    Task<CallResult<TResponseModel>> GetAsync<TResponseModel>(string path);

    Task<CallResult<TResponseModel>> PostAsync<TRequestModel, TResponseModel>(string path, TRequestModel? body);
}
