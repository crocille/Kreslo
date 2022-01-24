using MuagkoeKreslo.Database;
using MuagkoeKreslo.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MuagkoeKreslo.Pages
{
    /// <summary>
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : Page
    {
        const int NumInPage = 3;
        int PageNum = 0;
        public ProductList()
        {
            InitializeComponent();
            UpdateData();
            List<ProductType> productTypes = EfModel.Init().ProductTypes.ToList();
            productTypes.Insert(0, new ProductType { Title = "Все типы" });
            CbFilter.ItemsSource = productTypes;
            CbFilter.SelectedIndex = 0;

            CbSort.Items.Add("▲ Наименование");
            CbSort.Items.Add("▼ Наименование");
            CbSort.Items.Add("▲ Номер цеха");
            CbSort.Items.Add("▼ Номер цеха");
            CbSort.Items.Add("▲ Минимальная стоимость");
            CbSort.Items.Add("▼ Минимальная стоимость");

            CbSort.SelectedIndex = 0;

        }

        private void UpdateData() {
            IEnumerable<Product> products = EfModel.Init().Products
                .Where(p=>p.Title.Contains(TbSearch.Text) || p.Description.Contains(TbSearch.Text));

            if(CbFilter.SelectedIndex>0)
                products = products.Where(p => p.ProductType.ID == (CbFilter.SelectedItem as ProductType).ID);

            switch (CbSort.SelectedIndex)
            {
                case 0:
                    products= products.OrderBy(p => p.Title);
                    break;
                case 1:
                    products = products.OrderByDescending(p => p.Title);
                    break;
                case 3:
                    products = products.OrderBy(p => p.ProductionWorkshopNumber);
                    break;
                case 4:
                    products = products.OrderByDescending(p => p.ProductionWorkshopNumber);
                    break;
                case 5:
                    products = products.OrderBy(p => p.MinCostForAgent);
                    break;
                case 6:
                    products = products.OrderByDescending(p => p.MinCostForAgent);
                    break;
            }

            LVProducts.ItemsSource = products.Skip(NumInPage * PageNum).Take(NumInPage).ToList();
            LVProducts.SelectedItems.Clear();

            StackPages.Children.Clear();
            int PageCount = (products.Count() - 1 ) / NumInPage + 1;
            for(int i=0; i<PageCount; i++)
            {
                Button button = new Button { Content = new TextBlock { Text = (i + 1).ToString() }, Tag = i };
                button.Click += PageClick;
                if(i == PageNum)
                    (button.Content as TextBlock).TextDecorations = TextDecorations.Underline;
                StackPages.Children.Add(button);
            }

            btBackPage.Visibility = Visibility.Visible;
            BtNextPage.Visibility = Visibility.Visible;

            if (PageNum == 0)
                btBackPage.Visibility = Visibility.Collapsed;
            if (PageNum >= PageCount-1)
                BtNextPage.Visibility = Visibility.Collapsed;
        }

        private void PageClick(object sender, RoutedEventArgs e)
        {
            PageNum = Convert.ToInt32((sender as Button).Tag);
            UpdateData();
        }

        private void BtChangeCostClick(object sender, RoutedEventArgs e)
        {
            IEnumerable<Product> products =  LVProducts.SelectedItems.Cast<Product>();
            new ChangeMinCost(products).ShowDialog();
            UpdateData();
        }

        private void btAddClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductEditPage(new Product()));
        }

        private void BtEditClick(object sender, RoutedEventArgs e)
        {
            if (LVProducts.SelectedItems.Count > 0)
            {
                Product product = LVProducts.SelectedItem as Product;
                NavigationService.Navigate(new ProductEditPage(product));
            }
        }

        private void ProductListVisChange(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateData();
        }

        private void SearchChange(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void SortChange(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void BtBackPageClick(object sender, RoutedEventArgs e)
        {
            PageNum--;
            UpdateData();
        }

        private void BtNextPageClick(object sender, RoutedEventArgs e)
        {
            PageNum++;
            UpdateData();
        }
    }
}
