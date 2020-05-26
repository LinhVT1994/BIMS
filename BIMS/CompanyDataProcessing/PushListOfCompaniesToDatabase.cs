using CompanyDataProcessing.Model;
using DataUtilities.DataProcessing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompanyDataProcessing
{
    public class PushListOfCompaniesToDatabase
    {
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=db_boring_data";
        public static void Execute()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            BingMapDataService bingMapService = new BingMapDataService();
            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\companies_japan.xlsx";

            Console.WriteLine("Starting....");
            int startStep = 1;
            int stopStep = 1;
            // Step 
            /* Read L: Fulldetail (Bing...)
             *      B: Company name
             *      F: Detail address
             *      G Phone number
            // Update: M: Zipcode
                       N: Lat
                       O: Lon
                       P: RegionId
                       Q: PositionId
            */
            Task updateParty = Task.Run(() =>
            {

            }).ContinueWith(p =>
            {
                if (startStep == 1 && stopStep >= 1)
                {
                    ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                    excelToSql.StartRowInExcel = 1;
                    excelToSql.ExecuteComparing<RegionModel>(
                          (reg) =>
                          {
                              if (reg == null || string.IsNullOrWhiteSpace(reg.Zipcode))
                              {
                                  return false;
                              }
                              return true;
                          },
                       (reg) =>
                       {
                           StringBuilder str = new StringBuilder();
                           str.AppendFormat("select * from regions where zip_code = '{0}'", reg.Zipcode);
                           return str.ToString();
                       },
                        new Dictionary<string, string>
                        {
                        { "region_id", "P"},
                        });
                }

            }).ContinueWith(t =>
            {
                if (startStep == 2 && stopStep >= 2)
                {
                    int count = 0;
                    ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                    excelToSql.StartRowInExcel = 1;
                    excelToSql.Upload<PositionModel>((pos) =>
                    {
                        Debug.WriteLine("Line: " + count++);
                        if (pos == null || pos.RegionId <= 0)
                        {
                            return false;
                        }
                        return true;
                    },
                          ((p) =>
                          {
                              return p;
                          }));
                }

            }).ContinueWith((t) =>
            {
                if (startStep == 3 && stopStep >= 3)
                {
                    int count = 0;
                    ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                    excelToSql.StartRowInExcel = 1;
                    excelToSql.Upload<PartyModel>(
                          (pos) =>
                          {
                              Debug.WriteLine("Line: " + count++);
                              if (pos == null || pos.PositionId <= 0)
                              {
                                  return false;
                              }
                              return true;
                          }, ((p) =>
                          {
                              return p;
                          })
                          );
                }
            }).ContinueWith(t =>
            {
                Console.WriteLine("Finish....");
                source.Cancel();
            });
            Task.Run(() =>
            {

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
            Console.Read();
            return;
        }
    }
}
