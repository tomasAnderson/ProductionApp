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

namespace ProductionApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly ViewModel viewModel;
        public MainWindow()
        {
            viewModel = new ViewModel();
            DataContext = viewModel;

            InitializeComponent();

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;


        }

        private void subdivisions_DataGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            viewModel.addDataSUB();
        }

        private void employees_DataGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            viewModel.addDataEMP();
        }
    }
}
