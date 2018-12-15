namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	public class PluginColorArray : PluginColor {
		public PluginColorArray() : base(eType._ColorArray, "Color[]", true) {
		}
		public override bool IsValid(string cell) {
			//IS COLOR ARRAY
			string[] array = cell.Trim().Split(',');
			int countColor = 0;
			foreach (string s in array) {
				if (base.IsValid(s)) {
					countColor++;
				}
			}
			//Debug.Log("Do we autodetect if this is an color array? "+countColor+", "+array.Length);
			if (countColor == array.Length) {
				return true;
			}
			return false;
		}
		public override string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			if (cell == "") {
				return "new Color[]{}";
			} else {
				string[] array = cell.Trim().Split(',');
				List<string> values = new List<string>();
				foreach (string s in array) {
					Color parsed = Color.black;
					if(s.Trim().Length == 0) {
						values.Add("Color.Black");
					}else if(TryParseColor(s, out parsed)) {
						values.Add(string.Format("new Color({0}f, {1}f, {2}f, {3}f)", parsed.r, parsed.g, parsed.b, parsed.a));
					} else {
						values.Add(s.Trim());
					}
				}
				return "new Color[]{" + string.Join(",", values.ToArray()) + "}";
			}
		}
		public override bool IsValidType(Type type) {
			return type == typeof(Color[]);
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute	= MetaSheets.GetName_Variable(columnName);
			string tempVar1		= "tmp" + index;
			string tempVar2		= "tmp" + index+"b";
			return StringUtilities.Format(@"
					Color {0};
					string[] {1} = CleanString({2}).Split(',');
					_rows[i].{3} = new Color[{1}.Length];
					for (j = 0; j < {1}.Length; j++) {
						if(TryParseColor({1}[j], out {0})){
							_rows[i].{3}[j] = {0};
						}else{
							_rows[i].{3}[j] = Color.black;
						}
					}",
				tempVar1,
				tempVar2,
				cellVar,
				attribute
			);
		}
	}
}

