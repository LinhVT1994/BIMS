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
       // private string _Url = @"C:\Users\VuLin\Desktop\TestData.xlsx";
        //TraceListener listener = new DelimitedListTraceListener(@"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\logging.txt");
        private string _Url = @"C:\Users\TUAN-LINH\Desktop\TestData.xlsx";
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoadFromAExtendFile_Click(object sender, RoutedEventArgs e)
        {
            listInformation.Items.Add("Starting updating data to Position table...");
            Task<bool> task1 = Task<bool>.Run(() =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url);
                try
                {
                    excelToSql.Execute<Position>();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }).ContinueWith<bool>((theFirstTask)=> {
                if (theFirstTask.Result)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        listInformation.Items.Add("Updating data to Position table is success!");
                        listInformation.Items.Add("Starting updating data to Construction table...");
                        listInformation.Items.Refresh();
                    }));

                    ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url);
                    try
                    {
                        excelToSql.Execute<Construction>();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }).ContinueWith<bool>((theFirstTask) => {
                if (theFirstTask.Result)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        listInformation.Items.Add("Updating data to Construction table is success!");
                        listInformation.Items.Add("Starting updating data to Cement table...");
                        listInformation.Items.Refresh();
                    }));
                    ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url);
                    try
                    {
                        excelToSql.Execute<Cement>();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }).ContinueWith<bool>((theFirstTask) => {
                if (theFirstTask.Result)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        listInformation.Items.Add("Updating data to Cement table is success!");
                        listInformation.Items.Add("Starting updating data to TestingSample table...");
                        listInformation.Items.Refresh();
                    }));
                    ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url);
                    try
                    {
                        excelToSql.Execute<TestingSample>();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }).ContinueWith<bool>((theFirstTask) => {
                if (theFirstTask.Result)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        listInformation.Items.Refresh();
                        listInformation.Items.Add("Updating data to Cement table is success!");
                        listInformation.Items.Add("Starting updating data to MixingResult table...");
                    }));
                    ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url);
                    try
                    {
                        excelToSql.ExecuteMultiRecords<MixingResult>();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return false;
            }).ContinueWith<bool>((theFirstTask) => {
                if (theFirstTask.Result)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        listInformation.Items.Refresh();
                        listInformation.Items.Add("Updating data to Cement table is success!");
                        listInformation.Items.Add("Starting updating data to ConstructionExecuting table...");
                    }));
                    ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url);
                    try
                    {
                        excelToSql.ExecuteMultiRecords<ConstructionExecuting>();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return false;
            }).ContinueWith<bool>((theFirstTask) => {
                if (theFirstTask.Result)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        listInformation.Items.Refresh();
                        listInformation.Items.Add("Updating data to ConstructionExecuting table is success!");
                        listInformation.Items.Add("Starting updating data to QualityTesting table...");
                    }));
                    ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url);
                    try
                    {
                        excelToSql.ExecuteMultiRecords<QualityTesting>();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return false;
            }).ContinueWith<bool>((theFirstTask)=> {
                if (theFirstTask.Result)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        listInformation.Items.Refresh();
                        listInformation.Items.Add("All of the tables has updated successfuly!");
                    }));
                    return true;
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        listInformation.Items.Refresh();
                        listInformation.Items.Add("Has something error. I am so sorry about that.");
                    }));
                }
                return false;
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
           //string url = @"C:\Users\VuLin\Desktop\TestData.xlsx";
           ExcelDataAccess reader = ExcelDataAccess.GetInstance();
           Dictionary<string,Construction> positions = reader.Read<Construction>(url);
           // reader.Read(url);
        }

        private void TestSQLCommand()
        {
         //   Debug.Listeners.Add(listener);
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
