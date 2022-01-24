using MuagkoeKreslo.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MuagkoeKreslo.Windows
{
    /// <summary>
    /// Interaction logic for ChangeMinCost.xaml
    /// </summary>
    public partial class ChangeMinCost : Window
    {
        public string ProductListStr { get; set; }
        public Decimal Price { get; set; }
        
        IEnumerable<Product> Products;
        public ChangeMinCost(IEnumerable<Product> products)
        {
            Products = products;
            ProductListStr = String.Join(", ", products.Select(p => p.Title));
            Price = products.Average(p => p.MinCostForAgent);
            DataContext = this;
            InitializeComponent();
        }

        private void BtSaveClick(object sender, RoutedEventArgs e)
        {
            if(Price > 0)
            {
                foreach (Product product in Products)
                    product.MinCostForAgent = Price;
                EfModel.Init().SaveChanges();
                Close();
            }
        }
    }
}
