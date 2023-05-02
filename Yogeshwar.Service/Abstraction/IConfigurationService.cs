namespace Yogeshwar.Service.Abstraction;

public interface IConfigurationService : IDisposable
{
    Task<ConfigurationDto?> GetSingleAsync(CancellationToken cancellationToken);

    Task<int> UpdateAsync(ConfigurationDto configuration, CancellationToken cancellationToken);
}
