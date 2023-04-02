namespace Yogeshwar.DB.Models;

public partial class YogeshwarContext : DbContext
{
    public YogeshwarContext()
    {
    }

    public YogeshwarContext(DbContextOptions<YogeshwarContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accessory> Accessories { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerService> CustomerServices { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductAccessory> ProductAccessories { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=65.108.57.75;Database=Yogeshwar;MultipleActiveResultSets=true;TrustServerCertificate=True;User Id=sa;Password=Harshad@@1234;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accessory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Accessor__3214EC0762AD5F61");

            entity.Property(e => e.CreatedDate).HasPrecision(0);
            entity.Property(e => e.Image)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasPrecision(0);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CreatedDate).HasPrecision(0);
            entity.Property(e => e.Image)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasPrecision(0);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07A6E7EE92");

            entity.ToTable("Customer");

            entity.Property(e => e.AccountHolderName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.BankName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.BranchName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasPrecision(0)
                .HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(25);
            entity.Property(e => e.Gstnumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("GSTNumber");
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.Image)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(25);
            entity.Property(e => e.ModifiedDate).HasPrecision(0);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CustomerService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0743B23ADF");

            entity.ToTable("CustomerService");

            entity.Property(e => e.ComplainDate).HasPrecision(0);
            entity.Property(e => e.ServiceCompletionDate).HasPrecision(0);
            entity.Property(e => e.WorkerName).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.CustomerServices)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_customerservice_order_id");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07EEE41DF9");

            entity.ToTable("Notification");

            entity.Property(e => e.Date).HasPrecision(0);

            entity.HasOne(d => d.Order).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_notification_order_id");

            entity.HasOne(d => d.ProductAccessories).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ProductAccessoriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_notification_productaccessories_id");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC0751AAEB3F");

            entity.ToTable("Order");

            entity.Property(e => e.CreatedDate).HasPrecision(0);
            entity.Property(e => e.Discount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ModifiedDate).HasPrecision(0);
            entity.Property(e => e.OrderDate).HasPrecision(0);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_customer_id");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC071C9029AE");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedDate).HasPrecision(0);
            entity.Property(e => e.ModifiedDate).HasPrecision(0);
            entity.Property(e => e.ReceiveDate).HasPrecision(0);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderdetail_order_id");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderdetail_product_id");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC070625F62F");

            entity.ToTable("Product");

            entity.Property(e => e.CreatedDate)
                .HasPrecision(0)
                .HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModelNo).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasPrecision(0);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Video)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductAccessory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductA__3214EC0748A0BDF1");

            entity.HasOne(d => d.Accessories).WithMany(p => p.ProductAccessories)
                .HasForeignKey(d => d.AccessoriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productaccessories_accessories_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductAccessories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productaccessories_product_id");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProductCategory");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductCategories_Category");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductCategories_Product");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC07C1E1AE34");

            entity.Property(e => e.Image)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productimages_product_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07244638E6");

            entity.ToTable("User");

            entity.Property(e => e.CreatedDate)
                .HasPrecision(0)
                .HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasPrecision(0);
            entity.Property(e => e.Name).HasMaxLength(50);
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
