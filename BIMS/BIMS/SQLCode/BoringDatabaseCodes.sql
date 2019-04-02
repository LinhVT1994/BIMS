
-----------------------------------------------------------------------|
------------------------------------------------------------------------|
/*	                                                                    |
    Author: LinhVT               										|
    Created day: 01-10-2018       										|
    Edited day: 03-10-2018        										|
    Reasons for editing:          										|
                              										    |
  1. 3/10 Add some parameters for some tables below.				    |
			symbol varchar(10); in "soil_type" table				    |
			to_depth double;	  in "soil_result" table			    |
			latidude＝＞latitude	  in "position" table				   |
  2.10/10 Add removing the exiting tables.                         		|
  3.                              										|
  4.                              										|
	Finished day: 03-10-2018      										|
*/                                										
------------------------------------------------------------------------|

-------------------------------------------------------------------------
-------------------------------------------------------------------------
-------------------------Drop existing tables.---------------------------
----------------------A5. quality_testing----------------------
DROP TABLE IF EXISTS
   quality_testing
;

----------------------A4. construction_executing----------------------
DROP TABLE IF EXISTS
   construction_executing
;

----------------------A3. mixing_result----------------------
DROP TABLE IF EXISTS
   mixing_result
;
----------------------A2. cement----------------------
DROP TABLE IF EXISTS
   cement
;
----------------------A1. testing_sample----------------------
DROP TABLE IF EXISTS
   testing_sample
;
----------------------18. soil_classification（一般）----------------------
DROP TABLE IF EXISTS
   soil_classification
;
----------------------17. particle_size----------------------
DROP TABLE IF EXISTS
   particle_size
;

----------------------16. general（一般）----------------------
DROP TABLE IF EXISTS
   general
;

----------------------15. shear----------------------
DROP TABLE IF EXISTS
   shear
;
----------------------14. consolidation----------------------
DROP TABLE IF EXISTS
   consolidation
;
----------------------13. consistency_varcharistic----------------------
DROP TABLE IF EXISTS
   consistency_varcharistic
;
----------------------12. executing----------------------
DROP TABLE IF EXISTS
   executing
;
----------------------11. soil_sample----------------------
DROP TABLE IF EXISTS
   soil_sample
;
----
----------------------10. soil_result（土質の試験の結果）----------------------
DROP TABLE IF EXISTS
   soil_result
;
--------------------------9. spt_result（SPT試験の結果）----------------------
DROP TABLE IF EXISTS
   spt_result
;
--------------------------8. result（ボーリングの試験の結果）---------------------
DROP TABLE IF EXISTS
   result
;

--------------------------7. using_tool（用具使用）--------------------------
DROP TABLE IF EXISTS
   using_tool
;
--------------------------6. boring_test（ボーリング試験）----------------------
DROP TABLE IF EXISTS
   boring_test
;
--------------------------5. soil_type（土のタイプ）---------------------------
DROP TABLE IF EXISTS
   soil_type
;

DROP VIEW IF EXISTS
SearchingFullText
;
--------------------------4. construction（工事）---------------------------
DROP TABLE IF EXISTS
   construction
;
--------------------------3. tool（用具）-----------------------------------
DROP TABLE IF EXISTS
   tool
;
--------------------------7.1 tool_type（用具使用）--------------------------
DROP TABLE IF EXISTS
   tool_type
;
--------------------------2. party（実装機関）-------------------------------
DROP TABLE IF EXISTS
   party
;

--------------------------1. position（工事の位置）---------------------------
DROP TABLE IF EXISTS
   position
;
DROP TABLE IF EXISTS
   "regions"
;

/*
-------------------------------------------------------------------------
-- Create a new database.
CREATE DATABASE db_boring_data  -- create a new data base
with ENCODING = 'UTF-8'         -- use utf-8 for encoding
OWNER = "TuanLinh"              -- person who created this database.
TEMPLATE = template1            -- use template1 as a template.

-------------------------------------------------------------------------
-------------------------------------------------------------------------
*/
CREATE TABLE regions(
    region_id SERIAL,
    region_name varchar(255),
    region_name_roman varchar(255),
	region_level int,
	region_group_code int,
	zip_code  varchar(10),
	region_parent_id int,
	constraint pk_region primary key(region_id)
);

 -- 1.Create a "position" table with its properties
 CREATE TABLE "position"(
	position_id SERIAL , 
	region_id int,
	name varchar(200),        -- Detail
    latitude  varchar(50),    -- 緯度
    longitude  varchar(50),   -- 軽度
	constraint pk_position primary key(position_id)
);

 -- 2. Create a "party" table and its properties.
CREATE TABLE "party"(
	party_id SERIAL,
	name varchar(200),	 -- 会社名
    address varchar(200),		-- 住所名前
	phone  varchar(12),         -- 携帯電話番号 081804228150アドレス
	constraint pk_party primary key("party_id")
);
 --3.1. Create "tool_type" with its properties.
create TABLE "tool_type"(
	tool_type_id SERIAL,
	name varchar(200),
	constraint pk_tool_type primary key("tool_type_id")
);
 --3. Create "tool" with its properties.
CREATE TABLE "tool"(
	tool_id SERIAL,      
	name varchar(200),        -- 用具の名前
	tool_type_id integer,
	constraint pk_tool primary key(tool_id)
);


 --4. Create "construction" table and its properties.
CREATE TABLE "construction"(
	construction_id SERIAL,
	construction_no varchar(20),
	name varchar(200),		-- 事名
	position_id integer,      -- 位置のIｄ
	constraint pk_construction primary key(construction_id)
);

 --5. create "soil_type" table and its poperties.
CREATE TABLE "soil_type"(
	soil_type_id SERIAL,
	name varchar(500),--　土のタイプの名前
	symbol varchar(10),--　記号
	description text,--　説明
	image_url varchar(500),--　写真のアドレス
	constraint pk_soil_type primary key(soil_type_id)
);

 --6. Create "using_tool" with its properties.
CREATE TABLE "using_tool"(
	using_tool_id SERIAL,      
	tool_id integer,        -- 使う用具のId
    boring_test_id  integer,  -- どんなタイプ
	constraint pk_using_tool primary key(using_tool_id)
);
 --7. create "boring_test" table and its poperties.
CREATE TABLE "boring_test"(
	boring_test_id SERIAL,
	name varchar(500),--　調査名
	construction_id integer, --　工事名
	order_party_id integer, --　発注機関
	order_conducing_id integer, --　調査業者名
	constraint pk_boring_test primary key(boring_test_id)
);
 --8. create "result" table and its poperties.
CREATE TABLE "result"(
	result_id SERIAL,
	name varchar(100),
	boring_test_id integer, -- ボーリングの試験のId
	slope_of_ground double precision,     -- 地盤勾配
	direction varchar(100),  -- 方向
	angle varchar(20),       -- 角度
	started_day date,          -- 開始日
	finished_day date,         -- 完成日
	kbm double precision,      -- kbm
	water_elevation double precision, -- 孔内水立
	water_elevation_measuring_date date, -- 制定月日
	constraint pk_result primary key(result_id)
);

 --9. Create "spt_result" table and its properties.
CREATE TABLE "spt_result"(
	spt_result_id SERIAL,
	from_depth double precision,    --　深さから
	-- to_depth double precision,        --　深さまで
	first smallint,		 -- 一番目の１０センチのＳＰＴ
	penetration_on_first double precision,
	second smallint,	 -- 二番目の１０センチのＳＰＴ
	penetration_on_second double precision,
	third smallint,		 -- 三番目の１０センチのＳＰＴ　　　
	penetration_on_third double precision,
	result_id integer,
	constraint pk_spt_result primary key(spt_result_id)
);

 --10. create "soil_result" table and its poperties.
CREATE TABLE "soil_result"(
	soil_result_id SERIAL,
	from_depth double precision,   --　深さから
	to_depth double precision,   --　深さまで
	soil_type_id integer,	--　土のタイプのIｄ
	relative_density varchar(200),
	relative_consistency varchar(200),
	description text,	--　説明（土はどんな状態か）
	color_description text,	--　説明（土の色はどう）
	result_id integer,         --  結果のid  
	constraint pk_soil_result primary key(soil_result_id)
);
 -------------------------------------------------------------------------
 /*
 	
	Editor: LinhVT
	- Edited date: 12/10/2018
	- Add some tables for soil tested results.
	- Add some records for testing.
 */
 --11. create "soil_sample" (試料) table and its poperties.
 CREATE TABLE "soil_sample"(
	soil_sample_id SERIAL,
	sample_no varchar(50),
	from_depth double precision,   --　深さから
	to_depth double precision,   --　深さまで
	result_id integer,
	constraint pk_soil_sample primary key(soil_sample_id)
);

--12. create "executing"(実装) table and its poperties.
 CREATE TABLE "executing"(
	executing_id SERIAL,	
	soil_sample_id int,   --
	conducting_date date,
 	constraint pk_executing primary key(executing_id) 
);
 --13. create "consistency_varcharistic"(ンシステンシー　特性) table and its poperties.
 CREATE TABLE "consistency_varcharistic"(
	id SERIAL,
	liquid_limit double precision,   --　液性限界
	plasticity_limit double precision,   --　塑性限界
	plasticity_index double precision,   --　塑性指数
	executing_id integer,
	constraint pk_consistency_varcharistic primary key(id)
);

 --14. create "consolidation"(圧密) table and its poperties.
 CREATE TABLE "consolidation"(
	id SERIAL,
	compression_index double precision,   -- 圧縮指数
	consolidation_yield_stress double precision,   --　圧密降伏応力
	test_method varchar(100), -- 試験方法
	executing_id integer,
 	constraint pk_consolidation primary key(id) 
);


 --15. create "shear"(せん断) table and its poperties.
 CREATE TABLE "shear"(
	id SERIAL,
	total_stress_c double precision,  
	total_stress_c1 double precision, 
	affective_stress_c double precision,  
	affective_stress_c1 double precision, 
	executing_id integer,
 	constraint pk_shear primary key(id) 
);


 --16. create "general"(一般) table and its poperties.
 CREATE TABLE "general"(
	id SERIAL,
	dry_density double precision,   -- 湿潤密度
	wet_density double precision,   --　乾燥密度
	soil_particle double precision,   -- 土粒子の密度
	natural_water_content double precision, --　自然含水化
	void_ratio double precision,   --　間隙比
	dryness double precision,     --　乾和度
	executing_id integer,
 	constraint pk_general primary key(id) 
);

 --17. create "particle_size"(粒度) table and its poperties.
 CREATE TABLE "particle_size"(
	id SERIAL,
	stone double precision,   -- 石分
	gravel double precision,   --　礫分
	sand double precision,   -- 砂分
	silt double precision, --　泥土、シルト
	clay double precision,   --　粘土
	max_size double precision,     --　最大粒径
	equal_coefficient double precision,     --　均等係数
	particle_size_50persent double precision,     --　50 % 粒径 
	particle_size_10persent double precision,     --　10 % 粒径
	executing_id integer,
 	constraint pk_particle_size primary key(id) 
);
--18. create "soil_classification"(土の分類) table and its poperties.
 CREATE TABLE "soil_classification"(
	id SERIAL,	
	soil_type_id int,  
	conducting_date date,
	executing_id integer,
 	constraint pk_soil_classification primary key(id) 
);


--A1. create "testing_sample"(土の分類) table and its poperties.
 CREATE TABLE "testing_sample"(
	testing_sample_id SERIAL,	
	soil_type_id int,  
	name varchar(50),
	natural_water_content_ratio double precision,
	natural_wet_density double precision,
	color varchar(200),
	desctiption text,
	taget_strength double precision,
	construction_id integer,
 	constraint pk_testing_sample primary key(testing_sample_id) 
);

--A2. create "cement"(セメント) table and its poperties.
 CREATE TABLE "cement"(
	cement_id SERIAL,	
	symbol varchar(20),
	name varchar(200),
 	constraint pk_cement primary key(cement_id) 
);

--A3. create "mixing_result"(セメントと土を混ぜる試験) table and its poperties.
 CREATE TABLE "mixing_result"(
	mixing_result_id SERIAL,	
	cement_amount double precision,
	archived_strength double precision,
	water_content_ratio double precision,
	wet_density double precision,
	cement_id integer,
	testing_sample_id integer,
 	constraint pk_mixing_result primary key(mixing_result_id) 
);

--A4. create "construction_executing"(現場で実装) table and its poperties.
 CREATE TABLE "construction_executing"(
	construction_executing_id SERIAL,	
	cement_amount double precision,
	archived_strength double precision,
	cement_id integer,
	testing_sample_id integer,
 	constraint pk_construction_executing primary key(construction_executing_id) 
);
--A5. create "quality_testing"(品質管理) table and its poperties.
 CREATE TABLE "quality_testing"(
	quality_testing_id SERIAL,	
	name varchar(200),
	archived_strength_7day double precision,
	archived_strength_28day double precision,
	construction_executing_id integer,
 	constraint pk_quality_testing primary key(quality_testing_id) 
);

 -------------------------------------------------------------------------
 -------------------------------------------------------------------------
alter table "regions"
add constraint pk_region_parent
foreign key (region_parent_id)
references regions(region_id);

alter table "position"
add constraint pk_region_position
foreign key (region_id)
references regions(region_id);

-- add a foreign key constraint to an existing rate.

alter table "construction"
add constraint fk_position_of_construction
foreign key (position_id) references position(position_id);

alter table "soil_result" 
add constraint fk_type_soil_detail
foreign key (soil_type_id) references soil_type(soil_type_id),
add constraint fk_result_of_soil_classing
foreign key (result_id) references result(result_id);

alter table "spt_result" 
add constraint fk_spt_result
foreign key (result_id) references result(result_id);

alter table "using_tool"
add constraint fk_using_tools
foreign key (tool_id) references tool(tool_id),
add constraint fk_using_for_boring_test
foreign key (boring_test_id) references boring_test(boring_test_id);

alter table "tool" 
add constraint fk_tool_type
foreign key (tool_type_id) references tool_type(tool_type_id);

alter table "result" 
add constraint fk_result_boring_test
foreign key (boring_test_id) references boring_test(boring_test_id);

alter table "boring_test" 
add constraint fk_party_ordered
foreign key (order_party_id) references party(party_id),
add constraint fk_party_conduced
foreign key (order_conducing_id) references party(party_id),
add constraint fk_boring_test_of_construction
foreign key (construction_id) references construction(construction_id);

alter table "consolidation"
add constraint fk_consolidation_executing
foreign key (executing_id)
references executing(executing_id);

alter table  "consistency_varcharistic"
add constraint fk_consistency_varcharistic_executing
foreign key (executing_id)
references executing(executing_id);

alter table "shear"
add constraint fk_shear_executing
foreign key (executing_id)
references executing(executing_id);

alter table "general"
add constraint fk_general_executing
foreign key (executing_id)
references executing(executing_id);

alter table "particle_size"
add constraint fk_particle_size_executing
foreign key (executing_id)
references executing(executing_id);

alter table "soil_classification"
add constraint fk_soil_classification_executing
foreign key (executing_id)
references executing(executing_id),
add constraint fk_soil_classification_soil_type
foreign key (soil_type_id) references soil_type(soil_type_id);

alter table "soil_sample"
add constraint fk_soil_sample
foreign key (result_id)
references result(result_id);
-----------------------------------------------

alter table "testing_sample"
add constraint fk_testing_sample_construction
foreign key (construction_id)
references construction(construction_id);

alter table "mixing_result"
add constraint fk_mixing_result_sample
foreign key (testing_sample_id)
references testing_sample(testing_sample_id),
add constraint fk_mixing_result_cement
foreign key (cement_id)
references cement(cement_id);


alter table "construction_executing"
add constraint fk_construction_executing_cement
foreign key (cement_id)
references cement(cement_id);

alter table "quality_testing"
add constraint fk_quality_testing_executing
foreign key (construction_executing_id)
references construction_executing(construction_executing_id);


 CREATE VIEW SearchingFullText AS
		select construction_id,construction.name as construction_name,construction.construction_no, position.name address, construction.name || ' '|| construction.construction_no ||' '|| string_agg(position.name, ' ') as doc
		from "position"
		inner join construction using(position_id) 
		group by construction_id,construction.name,construction.construction_no, position.name
		order by construction_id;

insert into "tool_type"("tool_type_id", "name")
 values
	(1,'試錐機'),
	(2,'ハンマー落下'),
	(3,'ポンプ');
DROP TABLE IF EXISTS
   "users"
;
DROP TABLE IF EXISTS
   "permission_group"
;
DROP TABLE IF EXISTS
   "department"
;

CREATE TABLE department(
    department_id SERIAL,
    department_name varchar(255),
	constraint pk_department primary key(department_id)
);

CREATE TABLE "users"(
    user_id SERIAL,
    user_name varchar(255),
    user_email varchar(255),
	user_password varchar(400),
    department_id int,
	permission_group_id int,
	constraint pk_user primary key(user_id)
);

CREATE TABLE permission_group(
    permission_group_id SERIAL,
    permission_group_name varchar(255),
	description text,
	allow_boring_data_management BOOLEAN,
	allow_test_result_in_lab_management BOOLEAN,
	allow_quality_of_tnf_management BOOLEAN,
	allow_others_management BOOLEAN,
	allow_view BOOLEAN,
	allow_user_management BOOLEAN,
	allow_report_generator BOOLEAN,
	allow_system_setting_management BOOLEAN,
	constraint pk_permission_group primary key(permission_group_id)
);

alter table "users"
add constraint fk_user_department
foreign key (department_id)
references department(department_id);

alter table "users"
add constraint fk_user_permission_group
foreign key (permission_group_id)
references permission_group(permission_group_id);
insert into "users"(user_name,user_email,user_password,department_id,permission_group_id)
VALUES
('Admin','vulinh.hust@gmail.com','123456a@',null,null);