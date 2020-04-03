using DataUtilities.DataProcessing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UploadTestData.Model;
using Utilities.StringProcessingHelper;
namespace UploadTestData
{
    class Program
    {
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=db_boring_data";
        private static string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\test_data_in_lab.xlsx";
        private static string urlCement = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\cements.xlsx";
        static void Main(string[] args)
        {
            /*

            Dictionary<string, string> cementInSiteDataSaveMapping = new Dictionary<string, string>()
            {
                 {"cement_id","CF"},
            };
            int count = 0;
            ExecuteTask("CementInSite", () =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 6;
                excelToSql.ExecuteComparing<CementTempSite>(
                    (p) => {
                        if (p == null)
                        {
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(p.Symbol))
                        {
                            return false;
                        }
                        return true;
                    },
                    (p) => {
                        StringBuilder str = new StringBuilder();
                        var symbol = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Symbol);
                        Debug.WriteLine(count++ + "++" + symbol);

                        str.AppendFormat("select * from cement where symbol = '{0}'", symbol.ToUpper());
                        return str.ToString();
                    },
                    cementInSiteDataSaveMapping);

            });

           
            ExecuteTask("Cement", () =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(urlCement, _ConnectStr);
                excelToSql.StartRowInExcel = 2;
                excelToSql.UploadIfNotExisted<CementTemp>(
                    (p) => {
                        if (p == null)
                        {
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(p.Symbol)|| string.IsNullOrWhiteSpace(p.Name))
                        {
                            return false;
                        }
                        return true;
                    },
                    (p) => {
                       
                        var symbol = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Symbol);
                        symbol = symbol.Trim();
                        var name = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Name);
                        p.Name = name;
                        p.Symbol = symbol;
                        return p;

                    });

            });

           
            Dictionary<string, string> cementDataSaveMapping = new Dictionary<string, string>()
            {
                 {"cement_id","CC"},
            };
            int count = 0;
            ExecuteTask("Cement", () =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 6;
                excelToSql.ExecuteComparing<Cement>(
                    (p) => {
                        if (p == null)
                        {
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(p.Symbol))
                        {
                            return false;
                        }
                        return true;
                    },
                    (p) => {
                        StringBuilder str = new StringBuilder();
                        var symbol = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Symbol);
                        Debug.WriteLine(count++ + "++" + symbol);

                        str.AppendFormat("select * from cement where symbol = '{0}'", symbol.ToUpper());
                        return str.ToString();
                    },
                    cementDataSaveMapping);

            });
            count = 0;
            Dictionary<string, string> soilDataSaveMapping = new Dictionary<string, string>()
            {
                 {"soil_type_id","CD"},
            };
            ExecuteTask("SoilType", () =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 6;
                excelToSql.ExecuteComparing<SoilType>(
                    (p) => {
                        if (p == null)
                        {
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(p.Name))
                        {
                            return false;
                        }
                        return true;
                    },
                    (p) => {
                        StringBuilder str = new StringBuilder();
                        var name = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Name);
                        Debug.WriteLine(count++ + "++" + name);
                        if (name.Contains("粘性"))
                        {
                            name = name.Replace("粘性", "粘土質");
                        }
                        Debug.WriteLine(count++ + "++" + name);
                        str.AppendFormat("select * from soil_type where name = '{0}'", name.ToUpper());
                        return str.ToString();
                    },
                    soilDataSaveMapping);

            });

            Dictionary<string, string> conDataSaveMapping = new Dictionary<string, string>()
            {
                {"construction_id","CB"},
            };
            ExecuteTask("Construction", () =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 6;
                excelToSql.ExecuteComparing<ConstructionModel>(
                    (p) => {
                        return true;
                    },
                    (p) => {
                        StringBuilder str = new StringBuilder();
                        str.AppendFormat("select * from construction where construction_no = '{0}'", p.Construction_No);
                        return str.ToString();
                    },
                    conDataSaveMapping);

            });
           

            ExecuteTask("TestingSample", () =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 6;
                excelToSql.Upload<TestingSample>(
                    (p) => {
                        if (p == null || p == default(TestingSample))
                        {
                            return false;
                        }
                        if (p.ConstructionId <= 0 || p.CementTypeId <= 0)
                        {
                            return false;
                        }
                        return true;
                    },
                    (p) => {

                        return p;
                    });

            });
             

            ExecuteTask("ConstructionExecuting", () =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 6;
                excelToSql.ExecuteMultiRecords<ConstructionExecuting>(
                    (p) => {
                        if (p == null || p == default(ConstructionExecuting))
                        {
                            return false;
                        }
                        if (p.CementId <= 0 || p.TestingSampleId <= 0)
                        {
                            return false;
                        }
                        return true;
                    });

            });
            */
            ExecuteTask("TestingSample", () =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 6;
                excelToSql.Upload<TestingSample>(
                    (p) => {
                        if (p == null || p == default(TestingSample))
                        {
                            return false;
                        }
                        if (p.ConstructionId <= 0 || p.CementTypeId <= 0)
                        {
                            return false;
                        }
                        return true;
                    },
                    (p) => {

                        return p;
                    });

            });
            ExecuteTask("MixingResult", () =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 6;
                excelToSql.ExecuteMultiRecords<MixingResult>(
                    (p) => {
                        if (p == null || p == default(MixingResult))
                        {
                            return false;
                        }
                        if (p.CementId <= 0 || p.TestingSampleId <= 0)
                        {
                            return false;
                        }
                        return true;
                    });

            });
        }

        public static void ExecuteTask(string taskName, Action ation, string mark = ".")
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
    }
}
