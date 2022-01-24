namespace MuagkoeKreslo.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Material")]
    public partial class Material
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            MaterialCountHistories = new HashSet<MaterialCountHistory>();
            ProductMaterials = new HashSet<ProductMaterial>();
            Suppliers = new HashSet<Supplier>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int CountInPack { get; set; }

        [Required]
        [StringLength(10)]
        public string Unit { get; set; }

        public double? CountInStock { get; set; }

        public double MinCount { get; set; }

        [StringLength(1073741823)]
        public string Description { get; set; }

        public decimal Cost { get; set; }

        public byte[] Image { get; set; }

        public int MaterialTypeID { get; set; }

        public virtual MaterialType MaterialType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialCountHistory> MaterialCountHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
