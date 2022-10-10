using Yogeshwar.Helper.Extension;

namespace Yogeshwar.Service.Service;

internal class UserService : IUserService
{
    private readonly YogeshwarContext _context;

    public UserService(YogeshwarContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<UserDetailDto?> GetUserByCredential(string username, string password)
    {
        var encryptedPassword = EncryptionHelper.Encrypt(password);

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