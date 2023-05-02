using Yogeshwar.Service.Abstraction;

namespace Yogeshwar.Service.Service;

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
    /// </summary>
    private readonly IMappingService _mappingService;

    /// <summary>
    /// The root path
    /// </summary>
    private readonly string _rootPath;

    /// <summary>
    /// The prefix path
    /// </summary>
    private const string PrefixPath = "/DataImages/CompanyLogo/";

   
    public ConfigurationService(YogeshwarContext context, IWebHostEnvironment hostEnvironment,
        ICachingService cachingService,
        IMappingService mappingService)
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

    public async Task<ConfigurationDto?> GetSingleAsync(CancellationToken cancellationToken)
    {
        return await _cachingService.GetConfigurationSingleAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> UpdateAsync(ConfigurationDto configurationDto, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Configurations
            .FirstOrDefaultAsync(x => x.Id == configurationDto.Id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        if (configurationDto.ImageFile is not null)
        {
            var image = PrefixPath +
                        Guid.NewGuid().ToString().Replace("-", "") +
                        Path.GetExtension(configurationDto.ImageFile.FileName);

            await configurationDto.ImageFile.SaveAsync(_rootPath + image, cancellationToken).ConfigureAwait(false);

            DeleteFileIfExist(_rootPath + dbModel.CompanyLogo);

            dbModel.CompanyLogo = image;
        }

        dbModel.CompanyName = configurationDto.CompanyName;
        dbModel.GstNumber = configurationDto.GstNumber;
        dbModel.TermAndCondition = configurationDto.TermAndCondition;
        dbModel.Gst = configurationDto.Gst;

        _context.Configurations.Update(dbModel);
        var count = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveConfiguration();

        return count;
    }

    private static void DeleteFileIfExist(string name)
    {
        if (File.Exists(name))
        {
            File.Delete(name);
        }
    }
}
