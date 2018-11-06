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
using System.Data.SqlClient;
using Npgsql;
using System.Diagnostics;

namespace BIMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlParameter param;
        TraceListener listener = new DelimitedListTraceListener(@"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\logging.txt");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFromAExtendFile_Click(object sender, RoutedEventArgs e)
        {
            Debug.Listeners.Add(listener);
            SqlDataAccess sql = new SqlDataAccess();
            param = new SqlParameter("@cement_id", 2);
            SqlParameter[] para = new SqlParameter[1];
            para[0] = param;
            DataTable reader = sql.ExecuteSelectQuery("select * from cement where cement_id = @cement_id", para);
            for (int i = 0; i < reader.Count; i++)
            {
                Debug.WriteLine(reader.GetElementAt(i).Value("cement_id").ToString());
                Debug.WriteLine(reader.GetElementAt(i).Value("name").ToString());
                Debug.WriteLine(reader.GetElementAt(i).Value("symbol").ToString());
            }
            Debug.Flush();
            return;
            /*
            Position position = new Position();
            Dictionary<string, string> column = ExcelColumnAttribute.ColumnNamesMapping(position);
            string url = @"C:\Users\TUAN-LINH\Desktop\TestData.xlsx";
            ExcelReader reader = ExcelReader.GetInstance();
            Dictionary<string,Position> positions = reader.Read<Position>(url);
            // reader.Read(url);
            */
        }
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

    }
}
