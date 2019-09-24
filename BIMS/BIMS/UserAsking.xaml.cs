using BIMS.Model;
using BIMS.Utilities;
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

namespace BIMS
{
    /// <summary>
    /// Interaction logic for UserAsking.xaml
    /// </summary>
    public partial class UserAsking : Window
    {
        private string _Url = @"C:\Users\vulin\Desktop\Workspace\data.xlsx";
        public UserAsking()
        {
            InitializeComponent();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
