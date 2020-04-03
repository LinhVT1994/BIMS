using DataUtilities.DataProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UpdateExcelFileForVNTakeuchi.Models;

namespace UpdateExcelFileForVNTakeuchi
{
    class Program
    {
        private static string _Url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\TNFIMSData.xlsx";
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=db_boring_data";
        static void Main(string[] args)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            #region Upload to GeneralInfo
            /*
            Console.WriteLine("Starting....");
            Task task = Task.Run(() =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                Dictionary<string, string> updatingMap = new Dictionary<string, string>
                {
                    {"started_day","C"},
                    {"finished_day","D"},
                    {"zipcode","I"},
                    {"prefecture","E"},
                    {"wardorcity","F"},
                    {"area","G"},
                    {"detail_of_position","H"},
                    {"latitude","K"},
                    {"longitude","J"},
                };

                 excelToSql.ExecuteComparing<GeneralConstructionInfoModel>(
                    (construction) =>
                    {
                        if (construction == null || string.IsNullOrWhiteSpace(construction.No))
                        { 
                            
                            return false;
                        }
                        Console.WriteLine(construction.Name);
                        return true;
                    },
                             (region) => {
                                 StringBuilder str = new StringBuilder();
                                 string s = @"select construction_no as ""construction_no"",con.started_day, con.finished_day,con.name as ""name"", zipcode,prefecture,wardOrCity,area, pos_full.name as detail_of_position, pos_full.latitude,pos_full.longitude
                                                from construction con
                                                inner
                                                join
                                                (select position_id, zipcode, prefecture,wardOrCity,area, pos.name, pos.latitude, pos.longitude from position pos
                                                inner join(select area_tb.region_id, prefecture_tb.region_name as prefecture, ward_tb.region_name as wardOrCity, area_tb.region_name as area, area_tb.zip_code as zipcode
                                                from regions as area_tb
                                                inner join regions as ward_tb
                                                On area_tb.region_parent_id = ward_tb.region_id
                                                inner join regions as prefecture_tb
                                                On ward_tb.region_parent_id = prefecture_tb.region_id) as fullregion
                                                On fullregion.region_id = pos.region_id) as pos_full
                                                On pos_full.position_id = con.position_id";
                                 str.Append(s);
                                 str.AppendFormat(" where construction_no = '{0}'", region.No);
                                 return str.ToString();
                             },
                             updatingMap);

            }).ContinueWith(continuesTask => {
                source.Cancel();
                Console.WriteLine("Finish....");

            });
            Console.Read();
            */
            #endregion
            #region Upload to GeneralInfo
            Console.WriteLine("Starting....");
            Task task = Task.Run(() =>
            {
                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(_Url, _ConnectStr);
                excelToSql.StartRowInExcel = 3;
                Dictionary<string, string> updatingMap = new Dictionary<string, string>
                {
                    {"structure_type","C"},
                    {"scale","D"},
                    {"purpose","E"},
                    {"rooftop","F"},
                    {"design_route","G"},
                    {"movable_loading","H"},
                    {"snowfall_amount","I"},
                    {"total_floor_area","J"},
                    {"total_construction_area","K"},
                    {"platform","M"},
                    {"crushed_stone","N"},
                    {"ocr_min","O"},
                    {"fem_analysic","P"},
                    {"around_situation","Q"},
                    {"has_embankment_plan","R"},
                    {"is_narrow_land_boundary","S"},
                    {"is_more_2terms","T"},
                    {"has_burial_property_below","U"},
                    {"freezer_temperature","V"},
                    {"is_govemment_construction","W"},
                    {"is_over_200_think","X"},
                };

                 excelToSql.ExecuteComparing<GeneralConstructionInfoModel>(
                    (construction) =>
                    {
                        if (construction == null || string.IsNullOrWhiteSpace(construction.No))
                        { 
                            
                            return false;
                        }
                        Console.WriteLine(construction.Name);
                        return true;
                    },
                             (region) => {
                                 StringBuilder str = new StringBuilder();
                                 string s = @"select con.construction_no, 
                                                structure_type.name as structure_type,
                                                scale.name as scale,
                                                purpose.name as purpose,
                                                rooftop.name as rooftop,
                                                design_route.name as design_route,
                                                cond.movable_loading,
                                                cond.snowfall_amount,
                                                cond.total_floor_area,
                                                cond.total_construction_area,
                                                cond.platform,
                                                cond.crushed_stone,
                                                cond.ocr_min,
                                                cond.fem_analysic,
                                                cond.around_situation,
                                                cond.has_embankment_plan,
                                                cond.is_narrow_land_boundary,
                                                cond.is_more_2terms,
                                                cond.has_burial_property_below,
                                                cond.freezer_temperature,
                                                cond.is_govemment_construction,
                                                cond.is_over_200_think
                                                from construction as con,construction_detail as cond,structure_type,purpose,design_route,rooftop, scale
                                                where con.construction_id = cond.construction_id 
                                                and structure_type.structure_type_id = cond.structure_type_id
                                                and purpose.purpose_id = cond.purpose_id
                                                and rooftop.rooftop_id=cond.rooftop_id
                                                and design_route.design_route_id=cond.design_route_id
                                                and scale.scale_id = cond.scale_id";
                                 str.Append(s);
                                 str.AppendFormat(" and construction_no = '{0}'", region.No);
                                 return str.ToString();
                             },
                             updatingMap);

            }).ContinueWith(continuesTask => {
                source.Cancel();
                Console.WriteLine("Finish....");

            });
            Console.Read();
            #endregion

        }
    }
}
