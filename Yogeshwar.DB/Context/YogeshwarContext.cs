namespace Yogeshwar.DB.Context;

public sealed class YogeshwarContext : DbContext
{
    public YogeshwarContext(DbContextOptions<YogeshwarContext> options)
        : base(options)
    {
    }

    internal DbSet<Accessory> Accessories { get; set; }

    internal DbSet<Category> Categories { get; set; }

    internal DbSet<Configuration> Configurations { get; set; }

    internal DbSet<Customer> Customers { get; set; }

    internal DbSet<CustomerAddress> CustomerAddresses { get; set; }

    internal DbSet<CustomerService> CustomerServices { get; set; }

    internal DbSet<Notification> Notifications { get; set; }

    internal DbSet<Order> Orders { get; set; }

    internal DbSet<OrderDetail> OrderDetails { get; set; }

    internal DbSet<Product> Products { get; set; }

    internal DbSet<ProductAccessory> ProductAccessories { get; set; }

    internal DbSet<ProductCategory> ProductCategories { get; set; }

    internal DbSet<ProductImage> ProductImages { get; set; }

    internal DbSet<User> Users { get; set; }
}