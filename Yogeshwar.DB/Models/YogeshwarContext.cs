namespace Yogeshwar.DB.Models;

public partial class YogeshwarContext : DbContext
{
    public YogeshwarContext()
    {
    }

    public virtual DbSet<Customer> Customers { get; set; } = null!;
    public virtual DbSet<CustomerService> CustomerServices { get; set; } = null!;
    public virtual DbSet<Notification> Notifications { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<ProductAccessory> ProductAccessories { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Database=Yogeshwar;MultipleActiveResultSets=true;Trusted_Connection=true;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.City)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.Property(e => e.CreatedDate)
                .HasPrecision(2)
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ModifiedDate).HasPrecision(2);

            entity.Property(e => e.PhoneNo)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CustomerService>(entity =>
        {
            entity.ToTable("CustomerService");

            entity.Property(e => e.ComplainDate).HasPrecision(2);

            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.ServiceCompletedDate).HasPrecision(2);

            entity.Property(e => e.WorkerName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.CustomerServices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_customerservice_customer_id");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.Date).HasPrecision(3);

            entity.HasOne(d => d.Order)
                .WithMany(p => p.Notifications)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_notification_order_id");

            entity.HasOne(d => d.ProductAccessories)
                .WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ProductAccessoriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_notification_productaccessories_id");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.Discount).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.OrderDate).HasPrecision(4);

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_customer_id");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.ReceiveDate).HasPrecision(4);

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderdetail_order_id");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderdetail_product_id");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.CreatedDate)
                .HasPrecision(2)
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Description).HasMaxLength(250);

            entity.Property(e => e.Image).HasMaxLength(50);

            entity.Property(e => e.ModelNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.Video).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductAccessory>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductAccessories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productaccessories_product_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.CreatedDate)
                .HasPrecision(2)
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.PhoneNo)
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
