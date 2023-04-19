namespace Yogeshwar.DB.DataSeeding;

internal static class DataSeeding
{
    internal static void SeedData(this ModelBuilder builder)
    {
        #region User

        builder.Entity<User>()
            .HasData(new User
            {
                Id = 1,
                Name = "Soham Patel",
                Username = "Soham Patel",
                Email = "sohampipaliyapatel@gmail.com",
                PhoneNo = "8128195769",
                Password = "xE1lAwv+UqE7GX5q6MWJZA==",
                UserType = 1,
                CreatedDate = DateTime.Now
            }, new User
            {
                Id = 2,
                Name = "Harshad Patel",
                Username = "yogeshwar",
                Email = "harshadpatel1571@gmail.com",
                PhoneNo = "8128382487",
                Password = "+e2tW/Ybi3njCdaCY5kG3g==",
                UserType = 1,
                CreatedDate = DateTime.Now
            });

        #endregion

        #region Categories

        builder.Entity<Category>()
            .HasData(new Category
            {
                Id = 1,
                Name = "Machine",
                HsnNo = "HSN4547JD7",
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsActive = true
            }, new Category
            {
                Id = 2,
                Name = "Spare Part",
                HsnNo = "HSNSP54475",
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsActive = true
            }, new Category
            {
                Id = 3,
                Name = "Electronic",
                HsnNo = "HSN5445ELC",
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsActive = true
            }, new Category
            {
                Id = 4,
                Name = "Iron",
                HsnNo = "HSN7887IRN",
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsActive = true
            }, new Category
            {
                Id = 5,
                Name = "Bolt & Nuts",
                HsnNo = "HSN87707BN",
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsActive = true
            });

        #endregion
    }
}