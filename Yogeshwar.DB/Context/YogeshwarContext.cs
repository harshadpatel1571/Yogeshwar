namespace Yogeshwar.DB.Context;

/// <summary>
/// Class YogeshwarContext. This class cannot be inherited.
/// Implements the <see cref="DbContext" />
/// </summary>
/// <seealso cref="DbContext" />
public sealed class YogeshwarContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="YogeshwarContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public YogeshwarContext(DbContextOptions<YogeshwarContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Called when [model creating].
    /// </summary>
    /// <param name="builder">The builder.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.SeedData();
        base.OnModelCreating(builder);
    }

    /// <summary>
    /// Gets or sets the accessories.
    /// </summary>
    /// <value>The accessories.</value>
    internal DbSet<Accessory> Accessories { get; set; }

    /// <summary>
    /// Gets or sets the categories.
    /// </summary>
    /// <value>The categories.</value>
    internal DbSet<Category> Categories { get; set; }

    /// <summary>
    /// Gets or sets the configurations.
    /// </summary>
    /// <value>The configurations.</value>
    internal DbSet<Configuration> Configurations { get; set; }

    /// <summary>
    /// Gets or sets the customers.
    /// </summary>
    /// <value>The customers.</value>
    internal DbSet<Customer> Customers { get; set; }

    /// <summary>
    /// Gets or sets the customer addresses.
    /// </summary>
    /// <value>The customer addresses.</value>
    internal DbSet<CustomerAddress> CustomerAddresses { get; set; }

    /// <summary>
    /// Gets or sets the customer services.
    /// </summary>
    /// <value>The customer services.</value>
    internal DbSet<CustomerService> CustomerServices { get; set; }

    /// <summary>
    /// Gets or sets the notifications.
    /// </summary>
    /// <value>The notifications.</value>
    internal DbSet<Notification> Notifications { get; set; }

    /// <summary>
    /// Gets or sets the orders.
    /// </summary>
    /// <value>The orders.</value>
    internal DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Gets or sets the order details.
    /// </summary>
    /// <value>The order details.</value>
    internal DbSet<OrderDetail> OrderDetails { get; set; }

    /// <summary>
    /// Gets or sets the products.
    /// </summary>
    /// <value>The products.</value>
    internal DbSet<Product> Products { get; set; }

    /// <summary>
    /// Gets or sets the product accessories.
    /// </summary>
    /// <value>The product accessories.</value>
    internal DbSet<ProductAccessory> ProductAccessories { get; set; }

    /// <summary>
    /// Gets or sets the product categories.
    /// </summary>
    /// <value>The product categories.</value>
    internal DbSet<ProductCategory> ProductCategories { get; set; }

    /// <summary>
    /// Gets or sets the product images.
    /// </summary>
    /// <value>The product images.</value>
    internal DbSet<ProductImage> ProductImages { get; set; }

    /// <summary>
    /// Gets or sets the users.
    /// </summary>
    /// <value>The users.</value>
    internal DbSet<User> Users { get; set; }
}