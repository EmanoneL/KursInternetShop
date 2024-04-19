using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DB;

public partial class HandmadeShopSystemContext : DbContext
{
    public HandmadeShopSystemContext()
    {
    }

    public HandmadeShopSystemContext(DbContextOptions<HandmadeShopSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartsHasProsuct> CartsHasProsucts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DeliveryCompany> DeliveryCompanies { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Right> Rights { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=C:\\\\\\\\Users\\\\\\\\Ekaterina\\\\\\\\source\\\\\\\\repos\\\\\\\\KursInternetShop\\\\\\\\DB\\\\\\\\bin\\\\\\\\Debug\\\\\\\\net5.0\\\\\\\\handmade shop system.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.IdAddress);

            entity.ToTable("Address");

            entity.Property(e => e.IdAddress).HasColumnName("idAddress");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.HomeNumber)
                .IsRequired()
                .HasColumnName("home number");
            entity.Property(e => e.Street).HasColumnName("street");

            entity.HasOne(d => d.CityNavigation).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.City)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StreetNavigation).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.Street)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.IdBank);

            entity.ToTable("Bank");

            entity.HasIndex(e => e.PhoneNumer, "IX_Bank_phone numer").IsUnique();

            entity.Property(e => e.IdBank).HasColumnName("idBank");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumer)
                .IsRequired()
                .HasColumnName("phone numer");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.IdCarts);

            entity.Property(e => e.IdCarts).HasColumnName("idCarts");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.IdCustomers).HasColumnName("idCustomers");

            entity.HasOne(d => d.IdCustomersNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.IdCustomers)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<CartsHasProsuct>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Carts_has_prosucts");

            entity.Property(e => e.IdCarts).HasColumnName("idCarts");
            entity.Property(e => e.IdProducts).HasColumnName("idProducts");

            entity.HasOne(d => d.IdCartsNavigation).WithMany()
                .HasForeignKey(d => d.IdCarts)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdProductsNavigation).WithMany()
                .HasForeignKey(d => d.IdProducts)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory);

            entity.ToTable("Category");

            entity.Property(e => e.IdCategory).HasColumnName("idCategory");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CityName)
                .HasColumnType("INTEGER")
                .HasColumnName("City name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.IdComments);

            entity.Property(e => e.IdComments).HasColumnName("idComments");
            entity.Property(e => e.Date)
                .IsRequired()
                .HasColumnName("date");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasColumnName("description");
            entity.Property(e => e.IdCustomers)
                .HasColumnType("NUMERIC")
                .HasColumnName("idCustomers");
            entity.Property(e => e.IdProduct)
                .HasColumnType("NUMERIC")
                .HasColumnName("idProduct");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.IdCustomersNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdCustomers)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.IdCustomers);

            entity.HasIndex(e => e.Email, "IX_Customers_email").IsUnique();

            entity.HasIndex(e => e.Login, "IX_Customers_login").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "IX_Customers_phone number").IsUnique();

            entity.Property(e => e.IdCustomers).HasColumnName("idCustomers");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("email");
            entity.Property(e => e.IdAddress).HasColumnName("idAddress");
            entity.Property(e => e.IdBank).HasColumnName("idBank");
            entity.Property(e => e.Login)
                .IsRequired()
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone number");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdAddressNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.IdAddress)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdBankNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.IdBank)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DeliveryCompany>(entity =>
        {
            entity.HasKey(e => e.IdDeliveryCompany);

            entity.ToTable("Delivery company");

            entity.HasIndex(e => e.Email, "IX_Delivery company_email").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "IX_Delivery company_phone_number").IsUnique();

            entity.Property(e => e.IdDeliveryCompany).HasColumnName("idDelivery company");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuItemId);

            entity.ToTable("menu");

            entity.Property(e => e.MenuItemId).HasColumnName("menu_item_id");
            entity.Property(e => e.Dll).HasColumnName("dll");
            entity.Property(e => e.Function).HasColumnName("function");
            entity.Property(e => e.ItemName).HasColumnName("item_name");
            entity.Property(e => e.Orders).HasColumnName("orders");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.IdAddress).HasColumnName("idAddress");
            entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");
            entity.Property(e => e.IdDeliveryCompany).HasColumnName("idDelivery company");
            entity.Property(e => e.IdOrders).HasColumnName("idOrders");
            entity.Property(e => e.IdProducts).HasColumnName("idProducts");
            entity.Property(e => e.ProductCount).HasColumnName("productCount");
            entity.Property(e => e.RegistrationDate)
                .IsRequired()
                .HasColumnName("registration date");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status");
            entity.Property(e => e.TotalCost).HasColumnName("totalCost");

            entity.HasOne(d => d.IdAddressNavigation).WithMany()
                .HasForeignKey(d => d.IdAddress)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdCustomerNavigation).WithMany()
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdDeliveryCompanyNavigation).WithMany().HasForeignKey(d => d.IdDeliveryCompany);

            entity.HasOne(d => d.IdProductsNavigation).WithMany()
                .HasForeignKey(d => d.IdProducts)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProducts);

            entity.Property(e => e.IdProducts).HasColumnName("idProducts");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Description)
                .HasColumnType("NUMERIC")
                .HasColumnName("description");
            entity.Property(e => e.IdCategory).HasColumnName("idCategory");
            entity.Property(e => e.IdSellers).HasColumnName("idSellers");
            entity.Property(e => e.IdStorages).HasColumnName("idStorages");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name");
            entity.Property(e => e.Picture)
                .IsRequired()
                .HasColumnName("picture");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdSellersNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdSellers)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdStoragesNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdStorages)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Right>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("rights");

            entity.Property(e => e.Del).HasColumnName("del");
            entity.Property(e => e.Edit).HasColumnName("edit");
            entity.Property(e => e.MenuItemId).HasColumnName("menu_item_id");
            entity.Property(e => e.Rd).HasColumnName("rd");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Write).HasColumnName("write");

            entity.HasOne(d => d.MenuItem).WithMany().HasForeignKey(d => d.MenuItemId);

            entity.HasOne(d => d.User).WithMany().HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.IdSellers);

            entity.HasIndex(e => e.Email, "IX_Sellers_email").IsUnique();

            entity.HasIndex(e => e.Login, "IX_Sellers_login").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "IX_Sellers_phone_number").IsUnique();

            entity.Property(e => e.IdSellers).HasColumnName("idSellers");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("email");
            entity.Property(e => e.IdAddress).HasColumnName("idAddress");
            entity.Property(e => e.IdBank)
                .HasColumnType("NUMERIC")
                .HasColumnName("idBank");
            entity.Property(e => e.Login)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("login");
            entity.Property(e => e.Logo).HasColumnName("logo");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("NUMERIC")
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");

            entity.HasOne(d => d.IdAddressNavigation).WithMany(p => p.Sellers)
                .HasForeignKey(d => d.IdAddress)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdBankNavigation).WithMany(p => p.Sellers)
                .HasForeignKey(d => d.IdBank)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.IdStorages);

            entity.Property(e => e.IdStorages).HasColumnName("idStorages");
            entity.Property(e => e.IdAddress).HasColumnName("idAddress");

            entity.HasOne(d => d.IdAddressNavigation).WithMany(p => p.Storages)
                .HasForeignKey(d => d.IdAddress)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Street>(entity =>
        {
            entity.ToTable("Street");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StreetName)
                .HasColumnType("INTEGER")
                .HasColumnName("street name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "IX_users_login").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Login)
                .IsRequired()
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
