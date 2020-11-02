using DataUtilities.DataProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GetPartyDataFromDesignData
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            string _ConnectStr = @"Host=172.16.0.13;Port=5432;Username=postgres;Password=123456a@;Database=tnfims_database";
            string url = @"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\BIMS\BIMS\BIMS\Resources\DesignData.xlsx";
            Console.WriteLine("Starting....");

            Task task = Task.Run(() =>
            {
                TextWriter tw = new StreamWriter("company.csv");
                Dictionary<string, CompanyData> companyDic = new Dictionary<string, CompanyData>();

                ExcelToSqlManipulationEdition excelToSql = ExcelToSqlManipulationEdition.CreateInstance(url, _ConnectStr);
                excelToSql.StartRowInExcel = 4;
                excelToSql.GetRowData<CompanyArr>((t) =>
                {
                    if (!string.IsNullOrWhiteSpace(t.Company1))
                    {
                        t.Company1 = t.Company1.Trim();
                        CompanyData com;
                        if (!companyDic.TryGetValue(t.Company1, out com))
                        {
                            com = new CompanyData();
                            com.Address = t.AddressOfCompany1;
                            com.Phone = t.PhoneOfCompany1;
                            com.CompanyName = t.Company1;
                            companyDic.Add(com.CompanyName, com);
                        }
                        if (!string.IsNullOrWhiteSpace(t.EmOfCompany1))
                        {
                            com.AddAnEmployee(t.EmOfCompany1, t.EmailOfEmp1);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(t.Company2))
                    {
                        t.Company2 = t.Company2.Trim();
                        CompanyData com;
                        if (!companyDic.TryGetValue(t.Company2, out com))
                        {
                            com = new CompanyData();
                            com.Address = t.AddressOfCompany2;
                            com.Phone = t.PhoneOfCompany1;
                            com.CompanyName = t.Company2;
                            companyDic.Add(com.CompanyName, com);
                        }
                        if (!string.IsNullOrWhiteSpace(t.EmOfCompany2))
                        {
                            com.AddAnEmployee(t.EmOfCompany2, t.EmailOfEmp2);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(t.Company3))
                    {
                        t.Company3 = t.Company3.Trim();
                        CompanyData com;
                        if (!companyDic.TryGetValue(t.Company3, out com))
                        {
                            com = new CompanyData();
                            com.CompanyName = t.Company3;
                            companyDic.Add(com.CompanyName, com);
                        }
                       
                    }
                    if (!string.IsNullOrWhiteSpace(t.Company4))
                    {
                        t.Company4 = t.Company4.Trim();
                        CompanyData com;
                        if (!companyDic.TryGetValue(t.Company4, out com))
                        {
                            com = new CompanyData();
                            com.CompanyName = t.Company4;
                            companyDic.Add(com.CompanyName, com);
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(t.Company5))
                    {
                        t.Company5 = t.Company5.Trim();
                        CompanyData com;
                        if (!companyDic.TryGetValue(t.Company5, out com))
                        {
                            com = new CompanyData();
                            com.CompanyName = t.Company5;
                            companyDic.Add(com.CompanyName, com);
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(t.Company6))
                    {
                        t.Company6 = t.Company6.Trim();
                        CompanyData com;
                        if (!companyDic.TryGetValue(t.Company6, out com))
                        {
                            com = new CompanyData();
                            com.CompanyName = t.Company6;
                            companyDic.Add(com.CompanyName, com);
                        }
                    }

                });
                foreach (var dic in companyDic.OrderBy(p=>p.Key))
                {
                    // write a line of text to the file
                    string eminfo = "";
                    if (dic.Value.Employees.Count> 0)
                    {
                        foreach (var em in dic.Value.Employees)
                        {
                            eminfo = $"{em.Name};{em.Email};";
                        }
                        eminfo = eminfo.TrimEnd(';');
                    }
                    string companyInfo = $"{dic.Value.CompanyName};{dic.Value.Address};{dic.Value.Phone};{eminfo}";
                    tw.WriteLine(companyInfo);
                }
                tw.Close();
            });
            Task.Run(() =>
            {

                while (true)
                {
                    Thread.Sleep(200);
                    Console.Write(". ");
                }

            }, token);
            Task.WaitAll(task);
        }
    }
}
