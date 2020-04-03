using DataUtilities.DataProcessing;
using DesignDataImporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.StringProcessingHelper;

namespace DesignDataImporter
{
    class Program
    {
       
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=db_boring_data";
        static void Main(string[] args)
        {

            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\Data.xlsx";
            string urlCompaniesList = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\Companies.xlsx";
            #region Executed
            /*
             * 
             Executed("Party", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(urlCompaniesList, _ConnectStr);
                excelToSql.StartRowInExcel = 2;
                excelToSql.Upload<PartyModel>(
                    (item) => {

                        if (string.IsNullOrWhiteSpace(item.PartyName))
                        {
                            return false;
                        }
                        else
                        {
                            return true;

                        }
                    },
                    (item) =>
                    {
                        return item;
                    });

            });
            Executed("DesignRoute",()=> {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.UploadIfNotExisted<DesignRouteModel>(
                    (p) => {
                        return CheckFormatName(p.Name);
                    },
                    (p) =>
                    {
                        p.Name =  p.Name;
                        p.Name =　JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Name);
                        return p;
                    });

            });
            Executed("StructureType", () => {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.UploadIfNotExisted<StructureTypeModel>(
                    (p) => {
                        return CheckFormatName(p.Name);
                    },
                    (p) =>
                    {
                        if (string.IsNullOrWhiteSpace(p.Name))
                        {
                            return p;
                        }
                        p.Name = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Name);
                        return p;
                    });

            });
            Executed("Purpose", () => {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.UploadIfNotExisted<PurposeModel>(
                    (p) => { return CheckFormatName(p.Name); },
                    (p) =>
                    {
                        if (string.IsNullOrWhiteSpace(p.Name))
                        {
                            return p;
                        }
                        p.Name = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Name);
                        return p;
                    });

            });
            Executed("RoofTop", () => {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.UploadIfNotExisted<RooftopModel>(
                    (p) => { return CheckFormatName(p.Name); },
                    (p) =>
                    {
                        if (string.IsNullOrWhiteSpace(p.Name))
                        {
                            return p;
                        }
                        p.Name = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Name);
                        return p;
                    });

            });
            Executed("Scale", () => {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.UploadIfNotExisted<ScaleModel>(
                    (p) => { return CheckFormatName(p.Name); },
                    (p) =>
                    {
                        if (string.IsNullOrWhiteSpace(p.Name))
                        {
                            return p;
                        }
                        p.Name = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Name);
                        return p;
                    });

            });
           
            Executed("Position",() =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.Upload<Position>(
                    (p) => { return true; },
                    (p) =>
                    {
                        return p;
                    });

            });
            Executed("Construction", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.Upload<Constuction>(
                    (p) => {
                        if (string.IsNullOrEmpty(p.ConstructionNo) || 
                            p.ConstructionNo.Length < 4 ||
                           p.StartDate == default(DateTime)
                           ||
                           p.FinishedDay == default(DateTime)
                            )
                        {
                            return false;
                        }
                        return true;

                    },
                    (p) =>
                    {
                        p.Status = 1;
                        return p;
                    });

            });
           
            Executed("ConstructionDetail", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 128;
                excelToSql.Upload<ConstructionDetailModel>(
                    (p) => {
                        if ( p.ConstructionId <= 0)
                        {
                            return false;
                        }
                        return true;

                    },
                    (p) =>
                    {
                        if (string.IsNullOrWhiteSpace(p.OCRComment))
                        {
                            return p;
                        }
                        else
                        {
                            string s = p.OCRComment.Trim();
                            if (s.Length == 1 && s[0] == '-')
                            {
                                p.OCRComment = null;
                            }
                            else if (s.Length == 1 && s[0] == '○')
                            {
                                p.OCRComment = "実装";
                            }
                            else
                            {
                                double data;
                                if (double.TryParse(s, out data))
                                {
                                    p.OCRMin = data;
                                    p.OCRComment = null;
                                }
                                else
                                {
                                  
                                }
                            }
                           
                            
                        }
                        return p;
                    });

            });
           
            Executed("StatementModel", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 128;
                excelToSql.Upload<StatementModel>(
                    (p) => {

                        if (p.ConstructionDetailId <= 0)
                        {
                            return false;
                        }
                        return true;

                    },
                    (p) =>
                    {
                        return p;
                    });

            });
       
            Executed("SuperStructureModel", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 128;
                excelToSql.Upload<SuperStructureModel>(
                    (item) => {

                        if (item.ConstructionDetailId <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            if (item.SumOfAmountSteel <= 0)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                            
                        }
                       

                    },
                    (item) =>
                    {
                        return item;
                    });

            });
               Executed("GroundImprovementModel", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 128;
                excelToSql.Upload<GroundImprovementModel>(
                    (item) => {

                        if (item.ConstructionDetailId <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            if (item.ConstructionDetailId <= 0)
                            {
                              
                                return false;
                            }
                            else
                            {
                                if (item.ImprovementArea <= 0 && item.TagetStrength <= 0)
                                {
                                    return false;
                                }
                                return true;
                            }

                        }


                    },
                    (item) =>
                    {
                        return item;
                    });

            });
   
            #endregion
           
            Executed("IdeaCooperatingOnDesign", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 128;
                excelToSql.Upload<IdeaCooperatingOnDesign>(
                    (item) => {

                        if (item.ConstructionDetailId <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            if (item.ConstructionDetailId <= 0)
                            {

                                return false;
                            }
                            else
                            {
                                if (item.Party == null)
                                {
                                    return false;
                                }
                                return true;
                            }

                        }


                    },
                    (item) =>
                    {
                        item.RoleOfCooperatingId = 8;
                        return item;
                    });

            });
            
            #endregion
            Executed("StructureCooperatingOnDesign", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 128;
                excelToSql.Upload<StructureCooperatingOnDesign>(
                    (item) => {

                        if (item.ConstructionDetailId <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            if (item.ConstructionDetailId <= 0)
                            {

                                return false;
                            }
                            else
                            {
                                if (item.Party == null)
                                {
                                    return false;
                                }
                                return true;
                            }

                        }


                    },
                    (item) =>
                    {
                        item.RoleOfCooperatingId = 9;
                        return item;
                    });

            });
            Executed("RelativeCooperatingOnDesign", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 128;
                excelToSql.Upload<RelativeCooperatingOnDesign>(
                    (item) => {

                        if (item.ConstructionDetailId <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            if (item.ConstructionDetailId <= 0)
                            {

                                return false;
                            }
                            else
                            {
                                if (item.Party == null)
                                {
                                    return false;
                                }
                                return true;
                            }

                        }


                    },
                    (item) =>
                    {
                        item.RoleOfCooperatingId = 10;
                        return item;
                    });

            });
            Executed("ConfirmationExCooperatingOnDesign", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 128;
                excelToSql.Upload<ConfirmationExCooperatingOnDesign>(
                    (item) => {

                        if (item.ConstructionDetailId <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            if (item.ConstructionDetailId <= 0)
                            {

                                return false;
                            }
                            else
                            {
                                if (item.Party == null)
                                {
                                    return false;
                                }
                                return true;
                            }

                        }


                    },
                    (item) =>
                    {
                        item.RoleOfCooperatingId = 11;
                        return item;
                    });

            });

            Executed("CheckCooperatingOnDesign", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 128;
                excelToSql.Upload<CheckCooperatingOnDesign>(
                    (item) => {

                        if (item.ConstructionDetailId <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            if (item.ConstructionDetailId <= 0)
                            {

                                return false;
                            }
                            else
                            {
                                if (item.Party == null)
                                {
                                    return false;
                                }
                                return true;
                            }

                        }


                    },
                    (item) =>
                    {
                        item.RoleOfCooperatingId = 12;
                        return item;
                    });

            });
            */
            #endregion
            Console.Read();
        }
        public static void Executed(string taskName,Action ation, string mark = ".")
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var task = Task.Run(() => {
                Console.WriteLine(taskName + " has started");
            }).ContinueWith((t)=> {

                ation.Invoke();

            }).ContinueWith((t) => {
                source.Cancel();
                Console.WriteLine(taskName+" has finished");
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

        public static  bool CheckFormatName(string name)
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

        public static Task ExecutedInTask(string taskName, Action ation,string mark = ".")
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Task.Run(() => {

                while (true)
                {
                    Thread.Sleep(200);
                    Console.Write(""+ mark + " ");
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }

            }, token);

            var task = Task.Run(() => {
                Console.WriteLine(taskName + " has started");
            }).ContinueWith((t) => {

                ation.Invoke();

            }).ContinueWith((t) => {
                source.Cancel();
                Console.WriteLine(taskName + " has finished");
            });

            Task.WaitAll(task);
            return task;
        }
    }
}
