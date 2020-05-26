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

    class PushListOfCompaniesToTempDatabase
    {
        public static void Preprocessing(Company com)
        {
            if (com != null && !string.IsNullOrWhiteSpace(com.Name))
            {
                com.Name = com.Name.Replace(" ", "");
                com.Name = com.Name.Replace("　", "");
            }
        }
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=TempDatabase";
        public static void Execute()
        {
            List<Company> companies = new List<Company>();
         
            string url3 = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\CompaniesMixxing.xlsx";
            ExcelToSqlManipulationEdition excelToSql2 = ExcelToSqlManipulationEdition.CreateInstance(url3, _ConnectStr);
            excelToSql2.StartRowInExcel = 1;
            excelToSql2.Upload<Company>(
                  (com) =>
                  {
                      if (com == null || string.IsNullOrWhiteSpace(com.Zipcode) || string.IsNullOrWhiteSpace(com.Name))
                      {
                          return false;
                      }
                      else
                      {
                          Preprocessing(com);
                          var data = companies.Where(p =>
                          {
                              if (string.IsNullOrWhiteSpace(p.Name) ||
                                  string.IsNullOrWhiteSpace(p.Zipcode) ||
                                  string.IsNullOrWhiteSpace(com.Zipcode)  || 
                                  string.IsNullOrWhiteSpace(com.Name))
                              {
                                  return false;
                              }
                              else
                              {
                                  if (p.Name.Equals(com.Name) && p.Zipcode.Equals(com.Zipcode))
                                  {
                                      return true;
                                  }
                                  return false;
                              }
                         
                          });
                          if (data == null || data.Count() <= 0)
                          {
                              companies.Add(com);
                          }
                      
                      }
                      return true;
                  },
               (reg) =>
               {
                   return reg;
               });

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"data.txt", true))
            {
                string str = "";
                int count = 0;
                foreach (var com in companies)
                {
                    var data = ParseAddress.MatchRegions(com.Address);
                    com.Prefecture = data.ElementAt(0);
                    com.City = data.ElementAt(1);
                    com.Distric = data.ElementAt(2);
                    com.Detail = data.ElementAt(3);
                    str = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14}", ++count, com.Name, com.Prefecture, com.City, com.Distric, com.Detail, com.Phone, "", com.Zipcode, "", "", com.Address, com.Zipcode, com.Latitude, com.Longitude);
                    file.WriteLine(str);
                }
            }
            

            return;

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            BingMapDataService bingMapService = new BingMapDataService();
            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\companies_japan1.xlsx";

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

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 1;
                excelToSql.Upload<Company>(
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
                       return reg;
                   });
            }).ContinueWith(p =>
            {

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
