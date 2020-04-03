using ReadInfomationFromEstimationFormApp.Controlers;
using ReadInfomationFromEstimationFormApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadInfomationFromEstimationFormApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the start row: ");
            int start = int.Parse(Console.ReadLine());
           
            Console.WriteLine("Enter the end row: ");
            int end = int.Parse(Console.ReadLine());

            EstimationFormWriter reader = new EstimationFormWriter();
            try
            {
                string dir =Directory.GetCurrentDirectory();
                reader.Open(dir+"\\documents\\summary.xlsx");
                reader.StartRowInExcel = start;
                reader.MaxOfRows = end;
                reader.UpdateData();
               
                Console.WriteLine("All of tasks have just finished, Press something to end this program");

            }
            catch (Exception e)
            {
                Console.WriteLine("Had some errors occurred");
            }
            finally
            {
                Console.WriteLine("Closed and saved file");
                reader.CloseExcelFile();
            }
            // var data = EstimationFormReader.ReadData<EstimationForm>(@"C:\Users\TUAN-LINH\Desktop\SynchronousProjects\test.xls");
          
            try
            {
            }
            catch (Exception)
            {

                throw;
            }
          
        }
    }
}
