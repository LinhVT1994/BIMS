using CompanyDataProcessing.Model;
using DataUtilities.DataProcessing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=db_tnfims";
        static void Main(string[] args)
        {


           // SplitAddressToColumnsExcel.Execute();
           //   PushListOfCompaniesToTempDatabase.Execute();
           // return;


            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            BingMapDataService bingMapService = new BingMapDataService();
            #region Upload to database
            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\CompaniesMixxing.xlsx";
            Console.WriteLine("Starting....");
            int startStep = 3; 
            int stopStep = 3;
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
                if (startStep == 0)
                {
                    #region Update latitude and longitude using BingAPI
                    ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                    excelToSql.StartRowInExcel = 1;
                    int count = 1;
                    excelToSql.ExecuteTraveling<RegionModel>((com) =>
                    {
                        return !string.IsNullOrWhiteSpace(com.FullAddress);

                    }, (com) =>
                    {
                        try
                        {
                            Debug.WriteLine("Line: " + count++);
                            var resourceSet = bingMapService.SearchLocation(com.FullAddress);
                            if (resourceSet != null && resourceSet.ResourceSets != null && resourceSet.ResourceSets.Count > 0)
                            {
                                var data = resourceSet.ResourceSets[0].Resources.Where(p => p.Confidence.ToLower().Equals("high"))?.ElementAt(0);

                                if (data != null && data.GeocodePoints != null && data.GeocodePoints.Count > 0)
                                {
                                    var pos = data.GeocodePoints[0];
                                    var latitude = pos.Coordinates[0];
                                    var longitude = pos.Coordinates[1];
                                    com.Longitude = longitude.ToString();
                                    com.Latitude = latitude.ToString();
                                }
                                if (data != null && data.Address != null)
                                {
                                    com.Zipcode = data.Address.PostalCode;
                                }

                            }
                            return com;
                        }
                        catch (Exception e)
                        {
                            return null;
                        }

                    }, new Dictionary<string, string>
                  {
                        { "Latitude", "N"},
                        { "Longitude", "O"},
                        { "Zipcode", "M"},
                 });
                    #endregion
                }

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


            string url2 = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\Data.xlsx";

            Console.WriteLine("Starting....");
            Task task2 = Task.Run(() =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;

                excelToSql.UploadIfNotExisted<Company>(
                    (company) =>
                    {
                        if (company == null || string.IsNullOrWhiteSpace(company.Name) || company.Name.Length < 2)
                        {
                            return false;
                        }
                        return true;
                    },
                    (company) =>
                    {
                        string name = JapaneseCharactersAdapter.Instance.ToHalfWidth(company.Name);
                        company.Name = name;
                        return company;
                    });


            }).ContinueWith(continuesTask =>
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
            #endregion
            Console.Read();

        }
    }
}
