using MuagkoeKreslo.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MuagkoeKreslo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Перекючение на страницу списка товаров
            FrNav.Navigate(new ProductList());
            MessageBox.Show(String.Format(MuagkoeKresloApp.Strings.Messages.WelocomeMessage, "username"));
        }


        private void btSetLocaleRu(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).SetCulture("ru");
        }

        private void btSetLocaleEn(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).SetCulture("en");
        }
    }
}
