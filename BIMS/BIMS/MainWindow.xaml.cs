#define FORTESTING
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace BIMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private SqlParameter param;
        TraceListener listener = new DelimitedListTraceListener(@"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\logging.txt");

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoadFromAExtendFile_Click(object sender, RoutedEventArgs e)
        {
            List<string> message = new List<string>();
            listInformation.Items.Add("Starting updating data to Position table.");
            listInformation.Items.Refresh();
            listInformation.DataContext = message;
            Debug.WriteLine("Starting updating data to Position table.");
            ExcelToSqlManipulation.Execute<Construction>();
            return;


            Task task1 = Task.Run(() =>
            {
                ExcelToSqlManipulation.Execute<Position>();
            }).ContinueWith((theFirstTask)=> {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    listInformation.Items.Add("Updating data to Position table is success!");
                }));
               
                this.Dispatcher.Invoke((Action)(() =>
                {
                    listInformation.Items.Refresh();
                    listInformation.Items.Add("Updating data to Construction is success!");
                    Debug.WriteLine("Updating data to Position Construction is success!");
                }));

            });


            listInformation.Items.Refresh();
            #region Code statements for testing.

#if (DEBUG && TESTTED)
            TestSQLCommand();
             MessageBox.Show("Nothing to display.", "Infomation");
#endif

#if (DEBUG && TESTTED)
            TestExcelAccess();

#endif
            #endregion
        }

        #region Methods for testing
        private void TestExcelAccess()
        {
        
           Position position = new Position();
           Dictionary<string, string> column = ExcelColumnAttribute.ColumnNamesMapping(position);
           string url = @"C:\Users\TUAN-LINH\Desktop\TestData.xlsx";
           ExcelDataAccess reader = ExcelDataAccess.GetInstance();
           Dictionary<string,Construction> positions = reader.Read<Construction>(url);
           // reader.Read(url);
        }

        private void TestSQLCommand()
        {
            Debug.Listeners.Add(listener);
            SqlDataAccess sql = new SqlDataAccess();
            SqlParameter[] para = new SqlParameter[1];
           
            para[0] = new SqlParameter("@cement_id", 100);
            var value = sql.ExecuteDeleteQuery("DELETE FROM cement where cement_id=@cement_id", para);
            Debug.WriteLine("Query result: " + value);
            /*
            param = new SqlParameter("@cement_id", 2);
            SqlParameter[] para = new SqlParameter[1];
            para[0] = param;

            DataTable results = sql.ExecuteSelectQuery("select * from cement where cement_id = @cement_id", para);
            for (int i = 0; i < results.Count; i++)
            {
                Debug.WriteLine(results.GetElementAt(i).Value("cement_id").ToString());
                Debug.WriteLine(results.GetElementAt(i).Value("name").ToString());
                Debug.WriteLine(results.GetElementAt(i).Value("symbol").ToString());
            }
            Debug.Flush();
            List<SqlParameter> para2 = new List<SqlParameter>();
            para2.Add(new SqlParameter("cement_id", 102));
            para2.Add(new SqlParameter("symbol", "LINH"));
            para2.Add(new SqlParameter("name", "No"));
            sql.ExecuteInsertOrUpdateQuery("insert into cement values(@cement_id,@symbol,@name)", para2.ToArray());
            */
            return;
        }
        #endregion
    }
}
