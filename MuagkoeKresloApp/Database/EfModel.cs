using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MuagkoeKreslo.Database
{
    public partial class EfModel : DbContext
    {
        private static EfModel Instace;
        public static EfModel Init() {
            if (Instace == null)
                Instace = new EfModel();
            return Instace;
        }
        public EfModel()
            : base("name=EfModel1")
        {
        }

        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<AgentPriorityHistory> AgentPriorityHistories { get; set; }
        public virtual DbSet<AgentType> AgentTypes { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<MaterialCountHistory> MaterialCountHistories { get; set; }
        public virtual DbSet<MaterialType> MaterialTypes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCostHistory> ProductCostHistories { get; set; }
        public virtual DbSet<ProductMaterial> ProductMaterials { get; set; }
        public virtual DbSet<ProductSale> ProductSales { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Agent>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Agent>()
                .Property(e => e.INN)
                .IsUnicode(false);

            modelBuilder.Entity<Agent>()
                .Property(e => e.KPP)
                .IsUnicode(false);

            modelBuilder.Entity<Agent>()
                .Property(e => e.DirectorName)
                .IsUnicode(false);

            modelBuilder.Entity<Agent>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Agent>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Agent>()
                .HasMany(e => e.AgentPriorityHistories)
                .WithRequired(e => e.Agent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Agent>()
                .HasMany(e => e.ProductSales)
                .WithRequired(e => e.Agent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Agent>()
                .HasMany(e => e.Shops)
                .WithRequired(e => e.Agent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AgentPriorityHistory>()
                .Property(e => e.ChangeDate)
                .HasPrecision(6);

            modelBuilder.Entity<AgentType>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<AgentType>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<AgentType>()
                .HasMany(e => e.Agents)
                .WithRequired(e => e.AgentType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.Unit)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.MaterialCountHistories)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.ProductMaterials)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.Suppliers)
                .WithMany(e => e.Materials)
                .Map(m => m.ToTable("MaterialSupplier").MapLeftKey("MaterialID").MapRightKey("SupplierID"));

            modelBuilder.Entity<MaterialCountHistory>()
                .Property(e => e.ChangeDate)
                .HasPrecision(6);

            modelBuilder.Entity<MaterialType>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<MaterialType>()
                .HasMany(e => e.Materials)
                .WithRequired(e => e.MaterialType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ArticleNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductCostHistories)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductMaterials)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductSales)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductCostHistory>()
                .Property(e => e.ChangeDate)
                .HasPrecision(6);

            modelBuilder.Entity<ProductType>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.INN)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.SupplierType)
                .IsUnicode(false);
        }
    }
}
