namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	public class PluginFloatArray : Plugin {
		public PluginFloatArray() : base(eType._floatArray, "float[]", true) {
		}
		public override bool IsValid(string cell) {
			if (cell.Contains(",")) {
				string[] array = cell.Split(',');
				int countFloat = 0;
				float result;
				foreach (string s in array) {
					if (float.TryParse(s, out result)) {
						countFloat++;
					}
				}
				if (countFloat == array.Length) {
					return true;
				}
			}
			return false;
		}
		public override string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			if (cell == "") {
				return "new float[]{}";
			} else {
				List<string> values = new List<string>();
				foreach (string s in cell.Split(',')) {
					float parsed;
					if (s.Trim().Length == 0) {
						values.Add("0f");
					} else if (float.TryParse(s, out parsed)) {
						values.Add(parsed.ToString() + "f");
					} else {
						values.Add(s.Trim());
					}
				}
				return "new float[]{" + string.Join(",", values.ToArray()) + "}";
			}
		}
		public override bool IsValidType(Type type) {
			return type == typeof(float[]);
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			string tempVar = "tmp" + index;
			return StringUtilities.Format(@"			
				{0} = CleanString({0});
				string[] {1} = {0}.Split(',');
				_rows[i].{2} = ({0} == """") ? new float[]{} : Array.ConvertAll<string, float>({1}, ParseFloat);",
				cellVar,
				tempVar,
				attribute
			);
		}
	}
}

