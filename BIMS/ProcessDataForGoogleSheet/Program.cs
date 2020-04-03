using DataUtilities.DataProcessing;
using ProcessDataForGoogleSheet.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.StringProcessingHelper;

namespace ProcessDataForGoogleSheet
{
    class Program
    {
        private static string _ConnectStr = @"Host=localhost;Port=5432;Username=postgres;Password=vutuanlinh;Database=db_boring_data";
        public static int count = 0;
        static void Main(string[] args)
        {
            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\ProcessDataForGoogleSheet\Map.xlsx";
            ExcelToSqlManipulationEdition reader = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
            reader.StartRowInExcel = 2;
            
            var results = reader.ExecuteDataGetting<Company>((p,rowData) => {

                try
                {
                    count = count + 1;
                    Console.WriteLine(count);
                    if (rowData == null || p == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (!string.IsNullOrWhiteSpace(rowData[0]?.ToString()))
                    {
                        
                        p.Name = rowData[0]?.ToString();
                    }
                    else
                    {
                        return;
                    }

                 
                    if (!string.IsNullOrWhiteSpace(rowData[1]?.ToString()))
                    {
                        p.Address = rowData[1]?.ToString();
                    }

                    p.Phone = rowData[2]?.ToString();
                    p.StructureSystem = rowData[3]?.ToString();
                    p.OrderReceived = rowData[4]?.ToString();
                    p.Executed = rowData[5]?.ToString();
                    p.RequestForQuotation = rowData[6]?.ToString();
                    p.Place1 = rowData[32]?.ToString();
                    p.Place2 = rowData[33]?.ToString();
                    p.Place3 = rowData[34]?.ToString();
                    List<string> list = new List<string>();
                    for (int i = 7; i < 31; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(rowData[i]?.ToString()))
                        {

                            string s = rowData[i]?.ToString();

                            var sEdit = "";
                            foreach (var item in s.Split(new char[] { ' ', '　' }))
                            {
                                if (!string.IsNullOrWhiteSpace(item))
                                {
                                    sEdit = sEdit + " " + item;
                                }
                            }

                            if (!list.Contains(sEdit))
                            {
                                list.Add(sEdit);
                            }

                        }
                    }
                    p.Employee = list;
                }
                catch (Exception e)
                {

                    throw e;
                }
                

            },(p)=> true);

            Dictionary<string, Company> dic = new Dictionary<string, Company>();

            foreach (var rs in results)
            {
                string key = RemoveSpaces(rs.Name) + "-" + RemoveSpaces(rs.Address);
                
                if (dic.ContainsKey(key))
                {
                    Company temp = null;
                    dic.TryGetValue(key, out temp);
                    if (temp != null)
                    {
                        List<string> employee = new List<string>();
                        if (temp.Employee != null && temp.Employee.Count > 0)
                        {
                            foreach (var item in temp.Employee)
                            {
                                if (!employee.Contains(item))
                                {
                                    employee.Add(item);
                                }
                            }
                        }
                        if (rs!=null && rs.Employee!=null && rs.Employee.Count > 0)
                        {
                            foreach (var item in rs.Employee)
                            {
                                if (!employee.Contains(item))
                                {
                                    employee.Add(item);
                                }
                            }
                        }
                      
                        rs.Employee = employee;
                        if (string.IsNullOrWhiteSpace(temp.Phone))
                        {
                            rs.Phone = temp.Phone;
                        }
                        if (string.IsNullOrWhiteSpace(temp.RequestForQuotation))
                        {
                            rs.RequestForQuotation = temp.RequestForQuotation;
                        }
                        if (string.IsNullOrWhiteSpace(temp.Executed))
                        {
                            rs.Executed = temp.Executed;
                        }
                        if (string.IsNullOrWhiteSpace(temp.Place1))
                        {
                            rs.Place1 = temp.Place1;
                        }
                        if (string.IsNullOrWhiteSpace(temp.Place2))
                        {
                            rs.Place1 = temp.Place2;
                        }
                        if (string.IsNullOrWhiteSpace(temp.Place3))
                        {
                            rs.Place1 = temp.Place3;
                        }
                        if (string.IsNullOrWhiteSpace(temp.StructureSystem))
                        {
                            rs.StructureSystem = temp.StructureSystem;
                        }
                        dic[key] = rs;
                    }
                    else
                    {
                        dic[key] = rs;
                    }
                }
                else
                {
                    dic.Add(key, rs);
                }
            }
           
            using (StreamWriter outputFile = new StreamWriter("data.txt"))
            {
                foreach (var pair in dic)
                {
                    var company = pair.Value;
                    string emp = "";
                    int endLine = 40;
                    if (company.Employee != null && company.Employee.Count > 0)
                    {
                        foreach (var item in company.Employee)
                        {
                            emp = emp + item + ";";
                        }
                        endLine = endLine - company.Employee.Count;
                    }
                    string space = "";
                    for (int i = 0; i < endLine; i++)
                    {
                        space = space + ";";
                    }
                    string s = string.Join(";", company.Name, company.Address, company.Phone, company.StructureSystem, company.OrderReceived, company.Executed, company.RequestForQuotation, emp, space, company.Place1, company.Place2, company.Place3);

                    outputFile.WriteLine(s);

                }
            }


        }
        public static string RemoveSpaces(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return "";
            }
            var sEdit = "";
            foreach (var item in s.Split(new char[] { ' ', '　' }))
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    sEdit = sEdit + " " + item;
                }
            }
            sEdit = sEdit.Trim();
            return sEdit;
        }
    }
}
