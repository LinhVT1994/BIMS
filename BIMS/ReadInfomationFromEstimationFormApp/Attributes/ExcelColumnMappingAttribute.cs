using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReadInfomationFromEstimationFormApp.Attributes
{
    public class Offset
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Offset(int x, int y)
        {
            Row = x;
            Col = y;
        }
        public Offset()
        {
            Row = 0;
            Col = 0;
        }
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class DirectionAttribute : Attribute
    {
        public Direction Direction
        {
            get;
            set;
        }
        public DirectionAttribute(Direction dir)
        {
            Direction = dir;
        }
        public static Direction? GetDirection(Type type, string propertyName)
        {

            PropertyInfo[] properties = type.GetProperties();
            Tuple<string, Offset> rs;
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(propertyName))
                {
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(DirectionAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {

                        return ((DirectionAttribute)attributes[0]).Direction;
                    }
                }

            }
            return null;
        }
    }
    [AttributeUsage(AttributeTargets.Property)] // only use this attribute for properties.
  
    public class ExcelColumnMappingAttribute : Attribute
    {
        public string Heading
        {
            get;
            set;
        }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }


        public ExcelColumnMappingAttribute(string heading, int offsetX, int offsetY)
        {
            Heading = heading;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }
        public ExcelColumnMappingAttribute(string heading)
        {
            Heading = heading;
            OffsetX = 0;
            OffsetY = 0;
        }
        public static List<string> GetListOfHeading(Type type)
        {

            PropertyInfo[] properties = type.GetProperties();
            List<string> rs = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ExcelColumnMappingAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {

                    string heading = ((ExcelColumnMappingAttribute)attributes[0]).Heading.ToString();
                    rs.Add(heading.Split('.')[1]);

                }

            }
            return rs;
        }
        public static Tuple<string, Offset> GetExcelColumnMapping(Type type,string propertyName)
        {
           
            PropertyInfo[] properties = type.GetProperties();
            Tuple<string, Offset> rs;
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(propertyName))
                {
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ExcelColumnMappingAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {

                        string heading = ((ExcelColumnMappingAttribute)attributes[0]).Heading.ToString();
                        int offsetX = ((ExcelColumnMappingAttribute)attributes[0]).OffsetX;
                        int offsetY = ((ExcelColumnMappingAttribute)attributes[0]).OffsetY;
                        Offset offset = new Offset(offsetX, offsetY);
                        return new Tuple<string, Offset>(heading, offset);
                    }
                }
               
            }
            return null;
        }
    }

}
