 -------------------------------------------------------------------------
 -------------------------------------------------------------------------
 -----------------------Some sql query sample.-----------------------------
 -------------------------------------------------------------------------
 -------------------------------------------------------------------------
 --1.　位置名が　'山形県天皇市一日町四丁目2-3'ある工事のデータを取得する。
SELECT con.construction_id, con.name "工事名", pos.name "位置"
FROM
	position pos
    INNER JOIN
	construction con
	USING (position_id)
WHERE pos.name = '山形県天皇市一日町四丁目2-3'
 -------------------------------------------------------------------------
 --2.　工事名が「ダイナム山形天皇」のボーリングのデータを取得
SELECT rt.result_id, rt.name,started_day,finished_day,kbm, water_elevation, water_elevation_measuring_date
	FROM 
 		boring_test boring 
 	 	INNER JOIN construction con 
 			USING (construction_id)
  		INNER JOIN result rt
 	 		USING (boring_test_id)
WHERE con.name = 'ダイナム山形天皇'
 -------------------------------------------------------------------------
 --３.　工事名が「ダイナム山形天皇」の貫入のデータを取得
SELECT rs.result_id,rs.name,from_depth, first,penetration_on_first, second, penetration_on_second, third, penetration_on_third
FROM spt_result spt
	 INNER JOIN result rs 
		USING (result_id)
WHERE result_id in (SELECT rt.result_id
						FROM 
							boring_test boring 
							INNER JOIN construction con 
								USING (construction_id)
							INNER JOIN result rt
								USING (boring_test_id)
					WHERE con.name = 'ダイナム山形天皇')
ORDER BY spt.result_id , spt.from_depth;

 -------------------------------------------------------------------------
 --４.　工事名が「ダイナム山形天皇」の土質分類のデータを取得
SELECT rs.result_id "結果のID",rs.name "ボーリングの名前",soil.from_depth "深さから",soil.to_depth "深さまで",
type.name "土のタイプ", type.symbol "土のタイプの記号", soil.description "記事",soil.color_description "色調",
soil.relative_density "相対密度", soil.relative_consistency "相対稠度"
FROM soil_result soil
	 INNER JOIN result rs 
		USING (result_id)
	 INNER JOIN soil_type type
	 	USING(soil_type_id)
WHERE result_id in (SELECT rt.result_id
						FROM 
							boring_test boring 
							INNER JOIN construction con 
								USING (construction_id)
							INNER JOIN result rt
								USING (boring_test_id)
					WHERE con.name = 'ダイナム山形天皇')
ORDER BY soil.result_id , soil.from_depth DESC;
 -------------------------------------------------------------------------
--５.　工事名が「ダイナム山形天皇」の一般の試験結果を取得
SELECT temple.executing_id "実装ID",
       temple.sample_no "試料の記号",
	   temple.from_depth "深さから",
	   temple.to_depth "深さまで",
	   temple.conducting_date "実装日",
	   gen.dry_density "乾燥密度",
	   gen.wet_density "湿潤密度",
	   gen.soil_particle "土粒子の密度",
	   gen.natural_water_content "自然含水化",
	   gen.void_ratio "間隙比",
	   gen.dryness "乾和度"
  FROM general gen
   INNER JOIN 
			(SELECT exe.executing_id,sample.sample_no,from_depth,to_depth, exe.conducting_date
			FROM soil_sample sample
				 INNER JOIN result rs 
					USING (result_id)
				 INNER JOIN executing exe
					USING(soil_sample_id)
			WHERE result_id in (SELECT rt.result_id
									FROM 
										boring_test boring 
										INNER JOIN construction con 
											USING (construction_id)
										INNER JOIN result rt
											USING (boring_test_id)
								WHERE con.name = 'ダイナム山形天皇')
			ORDER BY sample.soil_sample_id) as temple
	USING(executing_id);
 -------------------------------------------------------------------------