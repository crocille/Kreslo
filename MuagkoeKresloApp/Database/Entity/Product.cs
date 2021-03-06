namespace MuagkoeKreslo.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Класс товара
    /// </summary>
    [Table("Product")]
    public partial class Product: INotifyPropertyChanged 
        //Наследования интерфейса INotifyPropertyChanged
        //Для дальнейшего уведомления WPF об изменениях в классе
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            ProductCostHistories = new HashSet<ProductCostHistory>();
            ProductMaterials = new HashSet<ProductMaterial>();
            ProductSales = new HashSet<ProductSale>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int? ProductTypeID { get; set; }

        [Required]
        [StringLength(10)]
        public string ArticleNumber { get; set; }

        [StringLength(1073741823)]
        public string Description { get; set; }

        private byte[] _Image;

        public byte[] Image
        {
            get { return _Image; }
            set { 
                _Image = value;
                //Вызов обновления при обновлении картинки
                PropChange();
            }
        }


        public int? ProductionPersonCount { get; set; }

        public int? ProductionWorkshopNumber { get; set; }

        private decimal _MinCostForAgent;

        [Required]
        public decimal MinCostForAgent
        {
            get { return _MinCostForAgent; }
            set {
                if (value > 0)
                    _MinCostForAgent = value;
                else
                    return;
            }
        }

        [Required]
        public virtual ProductType ProductType { get; set; }

        public string MaterialsStr => String.Join(", ", ProductMaterials.Select(m => m.Material.Title));
        public decimal CalculateCost => ProductMaterials.Sum(p => p.Material.Cost * (decimal)p.Count);
        public bool IsSaleLastMount => ProductSales.Count(p => p.SaleDate > DateTime.Now.AddMonths(-1)) > 0;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductCostHistory> ProductCostHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductSale> ProductSales { get; set; }

        /// <summary>
        /// Событие изменения значения в классе
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Метод уведомления об изменении
        /// </summary>
        /// <param name="PropertyName">Название изменившегося поля</param>
        public void PropChange([CallerMemberName]string PropertyName = "")
        {
            //Если событие ни кто-то слушает (оно не пустое)
            if (PropertyChanged != null)
                //Вызываем его, передавая название поле
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
