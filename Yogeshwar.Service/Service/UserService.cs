﻿namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(IUserService))]
internal class UserService : IUserService
{
    private readonly YogeshwarContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
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
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <returns></returns>
    public async Task<UserDetailDto?> GetUserByCredential(string username, string password)
    {
        var encryptedPassword = ServiceExtension.Encrypt(password);

        return await _context.Users
            .Where(x => x.Username == username && x.Password == encryptedPassword)
            .Select(x => new UserDetailDto
            {
                Id = x.Id,
                Name = x.Name,
                Username = x.Username,
                CreatedDate = x.CreatedDate.ToString("dd-MM-yyyy"),
                Email = x.Email,
                PhoneNo = x.PhoneNo,
                UserType = x.UserType
            }).FirstOrDefaultAsync().ConfigureAwait(false);
    }
}