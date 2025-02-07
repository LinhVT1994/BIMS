﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataUtilities.Attributes;
using DataUtilities.Model;

namespace UploadTestData.Model
{
    /**
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/7
    */
    [SqlParameter("testing_sample")]
    class TestingSample : Element
    {
        private int    _TestingSampleId;
        private string _Name;
        private double _NaturalWaterContentRatio;
        private double _NaturalWetDensity;
        private string _Color;
        private string _Description;
        private double _TagetStrength;
        private ConstructionModel _Construction;
        [Required, PrimaryKey, AutoIncrement, SqlParameter("testing_sample_id"), ExcelTemporaryStorage("CE")]
        public int TestingSampleId
        {
            get
            {
                return _TestingSampleId;
            }
            set
            {
                _TestingSampleId = value;
            }
        }
        [Required,ExcelColumn("H"), SqlParameter("name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        [Required, ExcelColumn("R"), SqlParameter("natural_water_content_ratio")]
        public double NaturalWaterContentRatio
        {
            get
            {
                return _NaturalWaterContentRatio;
            }
            set
            {
                _NaturalWaterContentRatio = value;
            }
        }
      
        [Required, ExcelColumn("S"), SqlParameter("natural_wet_density")]
        public double NaturalWetDensity
        {
            get
            {
                return _NaturalWetDensity;
            }
            set
            {
                _NaturalWetDensity = value;
            }
        }
        [Required, ExcelColumn("M"), SqlParameter("color")]
        public string Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
            }
        }
        [Required, ExcelColumn("N"), SqlParameter("containing_materials")]
        public string ContainingMaterial
        {
            get;
            set;
        }
        [Required, ExcelColumn("O"), SqlParameter("desctiption")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
        [Required, ExcelColumn("J"), SqlParameter("taget_strength")]
        public double TagetStrength
        {
            get
            {
                return _TagetStrength;
            }
            set
            {
                _TagetStrength = value;
            }
        }

        [Required, ExcelColumn("CB"), SqlParameter("construction_id")]
        public int ConstructionId
        {
            get;
            set;
        }

        [Required, ExcelColumn("CD"), SqlParameter("soil_type_id")]
        public int CementTypeId
        {
            get;
            set;
        }
    }
}
