using DataUtilities.DataProcessing;
using SoilTypeUpdate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.StringProcessingHelper;

namespace SoilTypeUpdate
{
    class Program
    {
        private static string _Url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\soil_type.xlsx";
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=db_boring_data";
        static void Main(string[] args)
        {
            Executed("SoilType", () => {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url, _ConnectStr);
                excelToSql.StartRowInExcel = 1;
                excelToSql.UploadIfNotExisted<SoilType>(
                    (p) => { return CheckFormatName(p.Name); },
                    (p) =>
                    {
                       
                        return p;
                    });

            });
        }
        public static void Executed(string taskName, Action ation, string mark = ".")
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var task = Task.Run(() => {
                Console.WriteLine(taskName + " has started");
            }).ContinueWith((t) => {

                ation.Invoke();

            }).ContinueWith((t) => {
                source.Cancel();
                Console.WriteLine(taskName + " has finished");
            });

            Task.Run(() => {

                while (true)
                {
                    Thread.Sleep(200);
                    Console.Write("" + mark + " ");
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }

            }, token);

            Task.WaitAll(task);
        }
        public static bool CheckFormatName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            else
            {
                name = name.Trim();
                if (name.Length == 1 && (name[0] == '-' || name[0] == '?'))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
