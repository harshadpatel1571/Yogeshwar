namespace Yogeshwar.Service.Service;

/// <summary>
/// Class ConfigurationService.
/// Implements the <see cref="IConfigurationService" />
/// </summary>
/// <seealso cref="IConfigurationService" />
[RegisterService(ServiceLifetime.Scoped, typeof(IConfigurationService))]
internal class ConfigurationService : IConfigurationService
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly YogeshwarContext _context;

    /// <summary>
    /// The caching service
    /// </summary>
    private readonly ICachingService _cachingService;

    /// <summary>
    /// The mapping service
    /// </summary>
    private readonly IMappingService _mappingService;

    /// <summary>
    /// The root path
    /// </summary>
    private readonly string _rootPath;

    /// <summary>
    /// The prefix path
    /// </summary>
    private const string PrefixPath = "/DataImages/Configuration/";

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="cachingService">The caching service.</param>
    /// <param name="mappingService">The mapping service.</param>
    public ConfigurationService(YogeshwarContext context, IWebHostEnvironment hostEnvironment,
        ICachingService cachingService, IMappingService mappingService)
    {
        _context = context;
        _cachingService = cachingService;
        _mappingService = mappingService;
        _rootPath = hostEnvironment.WebRootPath;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Get as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ConfigurationDto&gt; representing the asynchronous operation.</returns>
    public async Task<ConfigurationDto?> GetAsync(CancellationToken cancellationToken)
    {
        var data = await _cachingService.GetConfigurationsAsync(cancellationToken)
            .ConfigureAwait(false);

        return data.FirstOrDefault();
    }

    /// <summary>
    /// Create or update as an asynchronous operation.
    /// </summary>
    /// <param name="configurationDto">The configuration dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ConfigurationDto&gt; representing the asynchronous operation.</returns>
    public async Task<ConfigurationDto> CreateOrUpdateAsync(ConfigurationDto configurationDto, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Configurations
            .FirstOrDefaultAsync(x => x.Id == configurationDto.Id, cancellationToken)
            .ConfigureAwait(false);

        dbModel ??= new Configuration();

        if (configurationDto.ImageFile is not null)
        {
            var image = PrefixPath +
                        Guid.NewGuid().ToString().Replace("-", "") +
                        Path.GetExtension(configurationDto.ImageFile.FileName);

            await configurationDto.ImageFile.SaveAsync(_rootPath + image, cancellationToken).ConfigureAwait(false);

            if (dbModel.Id > 0)
            {
                DeleteFileIfExist(_rootPath + dbModel.CompanyLogo);
            }

            dbModel.CompanyLogo = image;
        }

        dbModel.CompanyName = configurationDto.CompanyName;
        dbModel.GstNumber = configurationDto.GstNumber;
        dbModel.TermAndCondition = configurationDto.TermAndCondition;
        dbModel.Gst = configurationDto.Gst;

        if (dbModel.Id > 0)
        {
            _context.Configurations.Update(dbModel);
        }
        else
        {
            _context.Configurations.Add(dbModel);
        }

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        _cachingService.RemoveConfigurations();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Deletes the file if exist.
    /// </summary>
    /// <param name="name">The name.</param>
    private static void DeleteFileIfExist(string name)
    {
        if (File.Exists(name))
        {
            File.Delete(name);
        }
    }
}
