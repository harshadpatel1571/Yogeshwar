namespace Yogeshwar.Service.Service;

/// <summary>
/// Class UserService.
/// Implements the <see cref="IUserService" />
/// </summary>
/// <seealso cref="IUserService" />
[RegisterService(ServiceLifetime.Scoped, typeof(IUserService))]
internal sealed class UserService : IUserService
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly YogeshwarContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public UserService(YogeshwarContext context)
    {
        _context = context;
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
    /// Gets the user by credential.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="password">The password.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;UserDetailDto&gt; representing the asynchronous operation.</returns>
    public async Task<UserDetailDto?> GetUserByCredentialAsync(string userName,
        string password, CancellationToken cancellationToken)
    {
        var encryptedPassword = EncryptionHelper.Encrypt(password);

        return await _context.Users
            .Where(x => x.Username == userName && x.Password == encryptedPassword)
            .Select(x => new UserDetailDto
            {
                Id = x.Id,
                Name = x.Name,
                Username = x.Username,
                CreatedDate = x.CreatedDate,
                Email = x.Email,
                PhoneNo = x.PhoneNo,
                UserType = x.UserType
            }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }
}