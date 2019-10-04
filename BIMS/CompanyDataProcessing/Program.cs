using CompanyDataProcessing.Model;
using DataUtilities.DataProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.StringProcessingHelper;

namespace CompanyDataProcessing
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
            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\Data.xlsx";

            Console.WriteLine("Starting....");
            Task task = Task.Run(() =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;

               excelToSql.Upload<Company>(
                   (company)=> {
                       if (company == null || string.IsNullOrWhiteSpace(company.Name) || company.Name.Length < 2)
                       {
                           return false;
                       }
                       return true;
                   },
                   (company)=>
                   {
                       string name = JapaneseCharactersAdapter.Instance.ToHalfWidth(company.Name);
                       company.Name = name;
                       return company;
                   });
                    

            }).ContinueWith(continuesTask => {
                Console.WriteLine("Finish....");
                source.Cancel();

            });
            Task.Run(() => {

                while (true)
                {
                    Thread.Sleep(200);
                    Console.Write(". ");
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }

            }, token);
            Task.WaitAll(task);
            #endregion



            Console.Read();

        }
    }
}
