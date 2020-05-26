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
    public class SplitAddressToColumnsExcel
    {
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=db_boring_data";
        public static void Execute()
        {

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\companies_japan2.xlsx";
            Console.WriteLine("Starting....");
            BingMapDataService bingMapService = new BingMapDataService();
            Task updateParty = Task.Run(() =>
            {
                #region Update latitude and longitude using BingAPI
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 2;
                int count = 1;
                excelToSql.ExecuteTraveling<Company>(
                    com => 
                    {
                        Debug.WriteLine("+Line" + count++);
                        return !string.IsNullOrWhiteSpace(com.Address);
                    },
                    (com) =>
                    {
                        try
                        {
                            var data = ParseAddress.MatchRegions(com.Address);
                            com.Prefecture = data.ElementAt(0);
                            com.City = data.ElementAt(1);
                            com.Distric = data.ElementAt(2);
                            com.Detail = data.ElementAt(3);

                            var resourceSet = bingMapService.SearchLocation(com.Address);
                            if (resourceSet != null && resourceSet.ResourceSets != null && resourceSet.ResourceSets.Count > 0)
                            {
                                var bingRs = resourceSet.ResourceSets[0].Resources.Where(p => p.Confidence.ToLower().Equals("high"))?.ElementAt(0);

                                if (bingRs != null && bingRs.GeocodePoints != null && bingRs.GeocodePoints.Count > 0)
                                {
                                    var pos = bingRs.GeocodePoints[0];
                                    var latitude = pos.Coordinates[0];
                                    var longitude = pos.Coordinates[1];
                                    com.Longitude = longitude.ToString();
                                    com.Latitude = latitude.ToString();
                                }
                                if (data != null && bingRs.Address != null)
                                {
                                    com.Zipcode = bingRs.Address.PostalCode;
                                }

                            }
                            return com;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                     
                    }, 
                    new Dictionary<string, string>() {
                        {"Prefecture", "C" },
                        {"City", "D" },
                        {"Distric", "E" },
                        {"Detail", "F" },
                        {"Latitude", "N"},
                        {"Longitude", "O"},
                        {"Zipcode", "M"},
                    });
                #endregion

            }).ContinueWith((t)=> {
                Console.WriteLine("Finish Splitting addresses");
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
        }
    }
}
