using BIMS.Attributes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BIMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFromAExtendFile_Click(object sender, RoutedEventArgs e)
        {
            Position position = new Position();
            Dictionary<string, string> column = ExcelColumnAttribute.ColumnNamesMapping(position);
            string url = @"C:\Users\TUAN-LINH\Desktop\TestData.xlsx";
            ExcelReader reader = ExcelReader.GetInstance();
            Dictionary<string,Cement> positions = reader.Read<Cement>(url);
            // reader.Read(url);
        }
    }
}
