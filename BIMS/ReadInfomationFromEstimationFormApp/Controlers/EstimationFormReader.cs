using ReadInfomationFromEstimationFormApp.Attributes;
using ReadInfomationFromEstimationFormApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ReadInfomationFromEstimationFormApp.Controlers
{
    public class EstimationFormWriter
    {
        public string Url = null;
        private Microsoft.Office.Interop.Excel.Application xlApplication = null;
        private Excel.Worksheet xlworkSheet = null;
        Excel.Workbook _XlWorkBook = null;
        private int startRowInExcel = 1;
        public bool IsOpened = false;
        public int StartRowInExcel
        {
            get
            {
                return startRowInExcel;
            }
            set
            {
                startRowInExcel = value;
            }
        }
        private Excel.Range xlRange;
        private int numbOfRows = 0;
        private int numbOfColumns = 0;
        public int MaxOfRows
        {
            get
            {
                return numbOfRows;
            }
            set
            {
                numbOfRows = value;
            }
        }
        public EstimationFormWriter()
        {
            xlApplication = new Microsoft.Office.Interop.Excel.Application();
            xlApplication.Visible = false;
            xlApplication.DisplayAlerts = false;
        }
        public EstimationFormWriter(string url) : this()
        {
            try
            {
                Url = url;
                Open(Url);
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }
        public bool Open(string url)
        {
            Url = url;
            _XlWorkBook = xlApplication.Workbooks.Open(Url);
            if (!SelectWorkSheet())
            {
                return false;
            }
            // 工事物件基本情報 工事物件基本情報
            xlworkSheet.Unprotect();
            xlRange = xlworkSheet.UsedRange;
            numbOfRows = xlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row;
            numbOfColumns = xlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Column;
            IsOpened = true;
            return true;
        }
        public string GetNewestFiles(string dir)
        {
            if (!Directory.Exists(dir))
            {
                return null;
            }
            var files = Directory.GetFiles(dir).OrderByDescending(d => new FileInfo(d).CreationTime);

            if (files.Count() <= 0)
            {
                return null;
            }
            List<string> rs = new List<string>();
            foreach (var item in files)
            {
                rs.Add(item);
            }
            return rs[0];
        }
        public List<string> GetFiles(string url)
        {
            if (!Directory.Exists(url))
            {
                return null;
            }
            var files = Directory.GetFiles(url, "*.xls*");
            var files1 = Directory.GetFiles(url)?.OrderByDescending(d => new FileInfo(d).CreationTime);
            if (files1.Count() <= 0)
            {
                return null;
            }
            List<string> rs = new List<string>();
            foreach (var item in files1)
            {
                rs.Add(item);
            }
            return rs;
        }
        private string GetValueInCell(int row, int col)
        {
            string s = null;
            try
            {
                Excel.Range cell = xlworkSheet.Cells[row, col];
                if (cell.Value != null)
                {
                    s = xlworkSheet.Cells[row, col].Value.ToString();
                    return s;
                }
                return s;
            }
            catch (Exception)
            {
                return s;
            }
        }
        public void UpdateData()
        {
            Console.WriteLine("Start processing data..........");
            string dir = Directory.GetCurrentDirectory();
            string rootUrl = dir + @"\data\";
            int numbOfProcessedRows = 0;
            object lockedNumb = new object();
            object lockWriting = new object();
            for (int row = startRowInExcel; row < MaxOfRows; row++)
            {
                var no = GetValueInCell(row, 1);
                if (string.IsNullOrWhiteSpace(no))
                {

                }
                Console.WriteLine("# Task: {0} started", no);

                string estimationPart = rootUrl + no + @"\Estimations\";
                var lastFilesOfEstimation = GetFiles(estimationPart);
                if (lastFilesOfEstimation == null || lastFilesOfEstimation.Count <= 0)
                {
                    continue;
                }
                bool isSaved = false;
                foreach (var file in lastFilesOfEstimation)
                {
                    Console.WriteLine("   ->#file {0} is being processed", Path.GetFileName(file));
                    EstimationFormReader reader = new EstimationFormReader();
                    if (reader.Open(file))
                    {
                        if (!isSaved)
                        {
                            var rs = reader.ReadData<EstimationForm>();
                            if (rs.ConstructionNo == null || 
                                string.IsNullOrWhiteSpace(rs.ConstructionNo) || 
                                (!rs.ConstructionNo.Equals(no)))
                            {
                                if (!string.IsNullOrWhiteSpace(rs.ConstructionNo) && !rs.ConstructionNo.Equals(no))
                                {
                                    Utilities.WriteToTextFile(rs);
                                }
                                reader.CloseExcelFile(false);
                                Console.WriteLine("   ->#checkformat: file {0} is not correct", Path.GetFileName(file));
                                continue;
                            }

                            Console.WriteLine("   ->#read data: file {0}  is being read", Path.GetFileName(file));
                            lock (lockWriting)
                            {
                                WriteData(rs, row);
                            }
                            isSaved = true;
                            reader.CloseExcelFile(false);
                        }
                        else
                        {
                            reader.CloseExcelFile(false);
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("   ->#file {0} is not correct format or cant open", Path.GetFileName(file));
                    }

                }
                Console.WriteLine("# Task: {0} finished", no);
                lock (lockedNumb)
                {
                    numbOfProcessedRows = numbOfProcessedRows + 1;
                }
                Console.WriteLine("Executed..........{0}", numbOfProcessedRows * 100.0 / numbOfRows);
            }
            Console.WriteLine("Processing data finished..........");
        }
        public bool SelectWorkSheet()
        {
            if (_XlWorkBook.Sheets.Count > 0)
            {
                foreach (var ws in _XlWorkBook.Sheets)
                {
                    string name = ((Microsoft.Office.Interop.Excel.Worksheet)ws).Name;
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        continue;
                    }
                    if (name.Contains("Data") || name.Contains("data"))
                    {
                        xlworkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ws;
                        return true;
                    }
                }
            }
            return false;
        }

        public void WriteData(EstimationForm data, int row)
        {
            var requiredProperties = RequiredAttribute.GetRequiredPropertiesName(typeof(EstimationForm));
            var dicOfCol = ExcelColumnAttribute.ColumnNamesMapping(data);
            foreach (var pro in requiredProperties)
            {
                if (dicOfCol.ContainsKey(pro))
                {
                    var excelColumn = dicOfCol[pro];
                    PropertyInfo pInfo = typeof(EstimationForm).GetProperty(pro);
                    var rs = pInfo.GetValue(data);
                    SetValueInCell(row, excelColumn, rs);
                }
            }

        }
         public void CloseExcelFile(bool isSaved = true)
        {
            _XlWorkBook.Close(isSaved);
            
            xlApplication.Quit();
        }
        private void SetValueInCell(int row, string columnName, object value)
        {
            try
            {
                Excel.Range cell = xlworkSheet.Cells[row, columnName];
                xlworkSheet.Cells[row, columnName].Value2 = value.ToString();
            }
            catch (Exception)
            {

            }
        }
    }
    public class Group
    {
        public int PosRow { get; set; }
        public int PosCol { get; set; }
        public string Name
        {
            get;
            set;
        }
        public int LimitRow
        {
            get;
            set;
        }
        public int LimitColumn
        {
            get;
            set;
        }
        public Group(string name,int limitRow,int limitCol)
        {
            Name = name;
            LimitRow = limitRow;
            LimitColumn = limitCol;
        }

    }
    public class EstimationFormReader
    {
        public string Url = null;
        private Microsoft.Office.Interop.Excel.Application xlApplication = null;
        private Excel.Worksheet xlworkSheet = null;
        Excel.Workbook _XlWorkBook = null;
        private int startRowInExcel = 1;
        public int StartRowInExcel
        {
            get
            {
                return startRowInExcel;
            }
            set
            {
                startRowInExcel = value;
            }
        }
     
        private Excel.Range xlRange;
        private int numbOfRows = 0;
        private int numbOfColumns = 0;
        public EstimationFormReader()
        {
            xlApplication = new Microsoft.Office.Interop.Excel.Application();
            xlApplication.Visible = false;
            xlApplication.DisplayAlerts = false;
        }
        public EstimationFormReader(string url):this()
        {
            try
            {
                Url = url;
                Open(Url);
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }
        private string GetValueInCell(int row,int col)
        {
            string s = null;
            try
            {
                Excel.Range cell = xlworkSheet.Cells[row, col];
                if (cell.Value != null)
                {
                    s = xlworkSheet.Cells[row, col].Value.ToString();
                    return s;
                }
                return s;
            }
            catch (Exception)
            {
                return s;
            }
        }
        public bool Open(string url)
        {
            Url = url;
            _XlWorkBook = xlApplication.Workbooks.Open(Url);
            if (!SelectWorkSheet())
            {
                return false;
            }
            xlworkSheet.Unprotect();
            xlRange = xlworkSheet.UsedRange;
            numbOfRows = xlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row;
            numbOfColumns = xlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Column;
            return true;
        }

        public bool SelectWorkSheet()
        {
            if (_XlWorkBook.Sheets.Count > 0)
            {
                foreach (var ws in _XlWorkBook.Sheets)
                {
                    string name = ((Microsoft.Office.Interop.Excel.Worksheet)ws).Name;
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        continue;
                    }
                    if (name.Contains("工事物件基本情報") || name.Contains("基本情報") || name.Contains("工事物件"))
                    {
                        xlworkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ws;
                        return true;
                    }
                }
            }
            return false;
        }
        public EstimationForm ReadData<T>() where T : EstimationForm
        {
            try
            {
                EstimationForm record = new EstimationForm();
                List<string> data = new List<string>();
                Dictionary<string, Tuple<int, int>> postitionOfGroupsHeading = new Dictionary<string, Tuple<int, int>>();

                for (int row = startRowInExcel; row < numbOfRows; row++)
                {
                    int index = -1;
                    for (int col = 0; col < numbOfColumns; col++)
                    {
                        string val = GetValueInCell(row, col);
                        if (IsContainAGroupName(val, out index))
                        {
                            GetGroups().ElementAt(index).PosCol = col;
                            GetGroups().ElementAt(index).PosRow = row;
                        }
                    }
                }

                var requiredProperties = RequiredAttribute.GetRequiredPropertiesName(typeof(EstimationForm));
                foreach (var propertyName in requiredProperties)
                {
                    Tuple<string, Offset> map = ExcelColumnMappingAttribute.GetExcelColumnMapping(typeof(EstimationForm), propertyName);
                    var listOfHeadings = ExcelColumnMappingAttribute.GetListOfHeading(typeof(EstimationForm));
                    var direction = DirectionAttribute.GetDirection(typeof(EstimationForm), propertyName);
                    var name = map.Item1.ToString();
                    var offset = map.Item2 as Offset;
                    var groupName = name.Split('.')[0];
                    var heading = name.Split('.')[1];
                    var group = groups.Where(g => g.Name.Equals(groupName))?.First();
                    if (group == null)
                    {
                        continue;
                    }
                    for (int r = group.PosRow; r < group.PosRow + group.LimitRow; r++)
                    {
                        for (int c = group.PosCol; c < group.PosCol + group.LimitColumn; c++)
                        {
                            string val = GetValueInCell(r, c);
                            if (val == null || string.IsNullOrWhiteSpace(val))
                            {
                                continue;
                            }
                            if (val.Contains("税込") || val.Contains("当り"))
                            {
                                continue;
                            }
                            List<string> conditions = new List<string>();
                            if (heading.Contains("?"))
                            {
                                foreach (var item in heading.Split('?'))
                                {
                                    conditions.Add(item);
                                }
                            }
                            else
                            {
                                conditions.Add(heading);
                            }
                            foreach (var condition in conditions)
                            {
                                if (val.Contains(condition))
                                {
                                    if (direction != null)
                                    {
                                        if (direction == Direction.Right)
                                        {
                                            string valTemp = "";
                                            for (int cTemp = c + 1; cTemp < group.PosCol + group.LimitColumn; cTemp++)
                                            {
                                                var curentValue = GetValueInCell(r, cTemp);
                                                if (!string.IsNullOrEmpty(curentValue))
                                                {
                                                    if (listOfHeadings.Count(s => curentValue.Contains(s)) > 0)
                                                    {
                                                        valTemp = "";
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        valTemp = curentValue;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrWhiteSpace(valTemp))
                                            {
                                                PropertyInfo propertyInfo = record.GetType().GetProperty(propertyName);
                                                propertyInfo.SetValue(record, valTemp);
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {
                                        PropertyInfo propertyInfo = record.GetType().GetProperty(propertyName);
                                        string returnedValue = GetValueInCell(r + offset.Row, c + offset.Col);
                                        propertyInfo.SetValue(record, returnedValue);
                                    }
                                    break;
                                }
                            }

                        }
                    }
                }
                return record;
            }
            catch (Exception e)
            {
                CloseExcelFile();
                return null;
            }
          

        }
        public void CloseExcelFile(bool isSaved = true)
        {
            var rootDir = Path.GetDirectoryName(Url);
            var nameofFile = Path.GetFileNameWithoutExtension(Url);

          // xlworkSheet.SaveAs(rootDir +"/"+nameofFile + ".csv", Excel.XlFileFormat.xlCSV);
            _XlWorkBook.Saved = true;
            
            _XlWorkBook.Close();
           
            xlApplication.Quit();
            GC.Collect();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApplication);
            GC.WaitForPendingFinalizers();
        }
        private void killExcel()
        {
            System.Diagnostics.Process[] PROC = System.Diagnostics.Process.GetProcessesByName("EXCEL");
            foreach (System.Diagnostics.Process PK in PROC)
            {
                if (PK.MainWindowTitle.Length == 0)
                {
                    PK.Kill();
                }
            }
        }
        public static EstimationForm ReadData<T>(string url)
        {
            EstimationFormReader reader = new EstimationFormReader();
            reader.Open(url);
            var rs = reader.ReadData<EstimationForm>();
            return rs;
        }
        bool IsContainAGroupName(string rowStr, out int index)
        {
            if (rowStr == null || string.IsNullOrWhiteSpace(rowStr))
            {
                index = -1;
                return false;
            }
            for (int i = 0; i < GetGroups().Count; i++)
            {
                var group = GetGroups().ElementAt(i);
                if (rowStr.Contains(group.Name))
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }
        public List<Group> GetGroups()
        {
            if (groups == null)
            {
                groups = new List<Group>()
                {
                    new Group("業務情報",4,15),
                    new Group("施主情報",2,15),
                    new Group("物件情報",5,15),
                    new Group("受注情報",5,15),
                    new Group("外注情報",4,15),
                    new Group("施工情報",9,9),
                    new Group("測定結果",4,8),
                    new Group("社用車情報",4,15),
                };
            }
            return groups;
        }
        private List<Group> groups;
    }
}
