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
       
        private static string _ConnectStr = @"Host=172.16.0.13;Port=5432;Username=postgres;Password=123456a@;Database=tnfims_database";
        static void Main(string[] args)
        {

            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\2020.05.28TNFIMSData.xlsx";
            string designData = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\DesignData.xlsx";
            string urlCompaniesList = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\Companies.xlsx";

            Executed("Update Design Data", () =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.EndAtLine = 1358;
                excelToSql.Upload<UsingTechnique>(
                    (item) => {

                        if (item.ConstructionDetailId <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            return item.IsExecuted;
                        }


                    },
                    (item) =>
                    {
                        item.TechniqueId = 8;
                        return item;
                    });
            });

            return;
            Executed("SuperStructureModel", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
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
            Executed("StatementModel", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
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
            Executed("GroundImprovementModel", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.Upload<GroundImprovementModel>(
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
            Executed("Design Main Person", () => {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.UploadIfNotExisted<DesignPerson>(
                    (p) => {
                        if (string.IsNullOrWhiteSpace(p.DesignerName) || p.DesignerName.Equals("0"))
                        {
                            return false;
                        }
                        if (p.ConDesignId == 0 || p.DesignerId == 0)
                        {
                            return false;
                        }
                        return true;
                    },
                    (p) =>
                    {
                        p.IsMain = false;
                        return p;
                    });

            });
            Executed("Update ConDesign", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.EndAtLine = 1358;
                excelToSql.UpdateByPrimaryKey<ConDesign>((p)=> {
                    if (p.ConstructionId == 0)
                    {
                        return false;
                    }
                    return true;
                });
            });
           
            Executed("ConstructionDetail", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.Upload<ConstructionDetailModel>(
                    (p) => {
                        if (p.ConstructionId <= 0)
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
            Executed("Rooftop", () => {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 3;
                excelToSql.UploadIfNotExisted<RooftopModel>(
                    (p) => {
                        return CheckFormatName(p.Name);
                    },
                    (p) =>
                    {
                        p.Name = p.Name;
                        p.Name = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Name);
                        return p;
                    });

            });
          
          
           
            Executed("Construction", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 3;
                excelToSql.EndAtLine = 1358;
                excelToSql.UploadExcelFromDB<DesignRouteModel>();
            });
            Executed("ConstructionDetail", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 3;
                excelToSql.Upload<ConstructionDetailModel>(
                    (p) => {
                        if (p.ConstructionId <= 0)
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
            Executed("Construction", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 3;
                excelToSql.EndAtLine = 1358;
                excelToSql.UploadExcelFromDB<ConModel>();
            });
            Executed("Construction", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(@"C:\Users\TUAN-LINH\Desktop\C#Programming\ConstructionData.xlsx", _ConnectStr);
                excelToSql.StartRowInExcel = 1;
                excelToSql.Upload<Constuction>(
                    (p) => {
                        if (p.ConstructionId <= 0)
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
            Executed("ConstructionDetail", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(designData, _ConnectStr);
                excelToSql.StartRowInExcel = 3;
                excelToSql.Upload<ConstructionDetailModel>(
                    (p) => {
                        if (p.ConstructionId <= 0)
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

            /*
           
            */
            /*
           
            
            */
            #region Update Data 
            /*
              Executed("StructureType", () => {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 3;
                excelToSql.EndAtLine = 1358;
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
                excelToSql.StartRowInExcel = 3;
                excelToSql.EndAtLine = 1358;
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

              Executed("Scale", () => {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 3;
                excelToSql.EndAtLine = 1358;

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
                  Executed("Position", () =>
            {

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 3;
                excelToSql.EndAtLine = 1358;
                excelToSql.Upload<Position>(
                    (p) => {
                        if (p != null && !string.IsNullOrWhiteSpace(p.ConstructioNo))
                        {
                            return p.ConstructioNo.Length > 0 ? true : false;
                        }
                        else
                        {
                            return false;
                        }

                    },
                    (p) =>
                    {
                        if (!string.IsNullOrWhiteSpace(p.Name))
                        {
                            p.Name = JapaneseCharactersAdapter.Instance.ToHalfWidth(p.Name);
                            var data = MatchRegions(p.Name);
                            p.Name = data[3];
                        }
                        return p;
                    });

            });

            */


            #endregion
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
           ;
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
        public static string[] MatchRegions(string address)
        {
            string prefecture = "";
            string ward = "";
            string district = "";
            string moreDetail = "";

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }
            int level = 0;
            for (int i = 0; i < address.Count(); i++)
            {
               
                if (level == 0)
                {
                    if (address[i] == '県' ||
                        address[i] == '道' ||
                        address[i] == '都' || 
                        address[i] == '府')
                    {
                        level = 1;
                    }
                    prefecture += address[i];

                }
                else if (level == 1)
                {
                    var isCityIncluded = address.Contains('市');
                    var isWardIncluded = address.Contains('区');

                    var isGunIncluded = address.Contains('郡');
                    var isTownIncluded = address.Contains('町');
                    var isMuraIncluded = address.Contains('村');
                    char splitChar; 
                    if (isCityIncluded || isWardIncluded)
                    {
                        if (isCityIncluded && isWardIncluded)
                        {
                            splitChar = '区';
                        }
                        else if (isCityIncluded)
                        {
                            splitChar = '市';
                        }
                        else
                        {
                            splitChar = '区';
                        }
                       
                    }
                    else
                    {
                        if (isGunIncluded && isTownIncluded)
                        {
                            splitChar = '町';
                        }
                        else if (isGunIncluded && isMuraIncluded)
                        {
                            splitChar = '村';
                        }
                        else
                        {
                            splitChar = '郡';
                        }
                    }
                    if (address[i] == splitChar)
                    {
                        level = 2;
                    }
                    ward += address[i];
                }
                else if (level == 2)
                {
                    if (i + 1 < address.Length)
                    {
                        if (Char.IsNumber(address[i + 1]))
                        {
                            level = 3;
                        }
                        district += address[i];
                    }
                }
                else
                {
                    moreDetail += address[i];
                }
            }
            return new string[4]
            {
                prefecture,
                ward,
                district,
                moreDetail
            };

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
