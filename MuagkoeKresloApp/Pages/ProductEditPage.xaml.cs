using MuagkoeKreslo.Database;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MuagkoeKreslo.Pages
{
    /// <summary>
    /// Interaction logic for ProductEditPage.xaml
    /// </summary>
    public partial class ProductEditPage : Page
    {
        Product product;
        public ProductEditPage(Product product)
        {
            this.product = product;
            
            DataContext = product;
            InitializeComponent();
            CbMaterials.ItemsSource = EfModel.Init().Materials.ToList()
                .Where(m => !product.ProductMaterials.Select(pm => pm.Material).Contains(m));
            CbProductTypes.ItemsSource = EfModel.Init().ProductTypes.ToList();
            DgvMaterialsList.ItemsSource = product.ProductMaterials;
        }

        private void ImageChangeClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog { Filter = "Jpeg files|*.jpg|All Files|*.*" };
            
            if(openFile.ShowDialog() == true)
            {
                product.Image = File.ReadAllBytes(openFile.FileName);
            }
        }

        private void BtSaveClick(object sender, RoutedEventArgs e)
        {
           
            try
            {
                //Если добавляем
                if (product.ID == 0)
                {
                    EfModel.Init().Products.Add(product);
                    if (EfModel.Init().Products.FirstOrDefault(p => p.ArticleNumber == product.ArticleNumber) != null)
                    {
                        MessageBox.Show("Артикул уже используется другим товаром!");
                        return;
                    }
                }
                EfModel.Init().SaveChanges();
            }catch(DbEntityValidationException ex)
            {
                MessageBox.Show(String.Join(Environment.NewLine, ex.EntityValidationErrors.Last().ValidationErrors.Select(ve => ve.ErrorMessage)));
            }
        }

        private void BtDelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (product.ProductSales.Count > 0)
                {
                    MessageBox.Show("Удаление запрещено! Продукт продавался!");
                    return;
                }

                if (MessageBox.Show("Вы действительно хотите удалить товар " + product.Title + "?",
                    "Удалить товар?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    product.ProductMaterials.Clear();
                    product.ProductCostHistories.Clear();
                    EfModel.Init().Products.Remove(product);
                    EfModel.Init().SaveChanges();
                    product.ID = 0;
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                }                
            }
            catch (Exception)
            {
                MessageBox.Show("Удаление невозможно!");
            }
        }

        private void btMaterialAddClick(object sender, RoutedEventArgs e)
        {
            int count;
            if (CbMaterials.SelectedItem != null
                && TbCount.Text.Length > 0
                && Int32.TryParse(TbCount.Text, out count)
                && product.ProductMaterials.Count(pm => pm.MaterialID == (CbMaterials.SelectedItem as Material).ID) == 0
            )
            {
                product.ProductMaterials.Add(
                    new ProductMaterial { Material = (CbMaterials.SelectedItem as Material), Count = count }
                );
                DgvMaterialsList.ItemsSource = product.ProductMaterials.ToList();
                CbMaterials.ItemsSource = EfModel.Init().Materials.ToList()
             .Where(m => !product.ProductMaterials.Select(pm => pm.Material).Contains(m));
            }
            else {
                MessageBox.Show("Проверьте данные!");
            }
        }

        private void btMaterialDelClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            product.ProductMaterials.Remove(button.DataContext as ProductMaterial);
            DgvMaterialsList.ItemsSource = product.ProductMaterials.ToList();
            CbMaterials.ItemsSource = EfModel.Init().Materials.ToList()
                .Where(m => !product.ProductMaterials.Select(pm => pm.Material).Contains(m));
        }
    }
}
