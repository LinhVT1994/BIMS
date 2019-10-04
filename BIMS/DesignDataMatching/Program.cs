using DataUtilities.DataProcessing;
using DesignDataMatching.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.StringProcessingHelper;

namespace DesignDataMatching
{
    class Program
    {
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=TempDatabase";
        static void Main(string[] args)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
          
            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\Data2.xlsx";

            Console.WriteLine("Starting....");
            Task task = Task.Run(() =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                Dictionary<string, string> updatingMap = new Dictionary<string, string>
                {
                    {"construction_name","D"},
                    {"start_date","E"},
                    {"finish_date","F"},
                    {"implementation_area","G"},
                    {"volume","H"},
                    {"amount_of_money","I"},
                    {"business_suporter","K"},
                    {"contractor","M"},
                    {"owner","N"},
                    {"partner","O"},
                    {"partner1","P"},
                    {"partner2","Q"},
                    {"partner3","R"},
                    {"partner4","S"},
                    {"partner5","T"},
                    {"full_address","AB"},
                    {"structure","AD"},
                    {"scale","AF"},
                    {"purpose","AH"},
                };

                excelToSql.ExecuteComparing<DesignModel>(
                    (region) =>
                    {
                        if (region == null || string.IsNullOrWhiteSpace(region.Symbol))
                        {
                            return false;
                        }
                        return true;
                    },
                             (region) => {
                                 StringBuilder str = new StringBuilder();
                                 str.AppendFormat("select * from designmodel where symbol = '{0}'", region.Symbol);
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

            /*
            Console.WriteLine("Starting....");
            Task task = Task.Run(() =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 187;
                excelToSql.Upload<DesignModel>((data) =>
                {
                    if (data==null || string.IsNullOrWhiteSpace(data.Symbol))
                    {
                        return false;
                    }
                    return true;
                }, 
                (data) =>
                {
                    string constructionName = string.IsNullOrEmpty(data.ConstructionName) ? string.Empty : JapaneseCharactersAdapter.Instance.ToHalfWidth(data.ConstructionName);

                    string partner = string.IsNullOrEmpty(data.Partner)? string.Empty : JapaneseCharactersAdapter.Instance.ToHalfWidth(data.Partner);
                    string partner1 = string.IsNullOrEmpty(data.Partner1) ? string.Empty : JapaneseCharactersAdapter.Instance.ToHalfWidth(data.Partner1);
                    string partner2 = string.IsNullOrEmpty(data.Partner2) ? string.Empty : JapaneseCharactersAdapter.Instance.ToHalfWidth(data.Partner2);
                    string partner3 = string.IsNullOrEmpty(data.Partner3) ? string.Empty : JapaneseCharactersAdapter.Instance.ToHalfWidth(data.Partner3);
                    string partner4 = string.IsNullOrEmpty(data.Partner4) ? string.Empty : JapaneseCharactersAdapter.Instance.ToHalfWidth(data.Partner4);
                    string partner5 = string.IsNullOrEmpty(data.Partner5) ? string.Empty : JapaneseCharactersAdapter.Instance.ToHalfWidth(data.Partner5);

                    data.ConstructionName = constructionName;
                    data.Partner = partner;
                    data.Partner1 = partner1;
                    data.Partner2 = partner2;
                    data.Partner3 = partner3;
                    data.Partner4 = partner4;
                    data.Partner5 = partner5;

                    return data;
                });

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
            Console.Read();
            */
        }
    }
}
