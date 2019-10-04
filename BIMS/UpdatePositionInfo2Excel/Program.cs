using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UpdatePositionInfo2Excel.Model;
using DataUtilities.DataProcessing;

namespace UpdatePositionInfo2Excel
{
    class Program
    {
        private static string _Url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\DatabaseResources\postition.xlsx";
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=TempDatabase";
        static void Main(string[] args)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            #region Upload to database
            /*
            Console.WriteLine("Starting....");
            Task task = Task.Run(() =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url, _ConnectStr);
                excelToSql.StartRowInExcel = 2;
                excelToSql.Upload<Position>();
            }).ContinueWith(continuesTask=> {
                Console.WriteLine("Finish....");
            });
            Task.WaitAll(task);
            */
            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\DatabaseResources\JapanRegions.xlsx";

            Console.WriteLine("Starting....");
            Task task = Task.Run(() =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 2;
                Dictionary<string, string> updatingMap = new Dictionary<string, string>
                {
                    {"latitude","H"},
                    {"longitude","I"},
                };

               excelToSql.ExecuteComparing<JapanRegion>(
                   (region) =>
                            {
                                if (region == null || string.IsNullOrWhiteSpace(region.Postoffice))
                                {
                                    return false;
                                }
                                return true;
                            },
                            (region)=> {
                                StringBuilder str = new StringBuilder();
                                str.AppendFormat("select * from position where postoffice like '%{0}%'", region.Postoffice);
                                return str.ToString();
                            }, 
                            updatingMap);

            }).ContinueWith(continuesTask => {
                source.Cancel();
                Console.WriteLine("Finish....");
                
            });
            Task.Run(() => {

                while (true)
                {
                    Thread.Sleep(200);
                    Console.Write(". ");
                }

            }, token);
            Task.WaitAll(task);
            #endregion



            Console.Read();
        
        }
    }
}
