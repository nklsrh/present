namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	public class PluginIntArray : Plugin {
		public PluginIntArray() : base(eType._intArray, "int[]", true) {
		}
		public override bool IsValid(string cell) {
			//IS INT ARRAY
			if (cell.Contains(",")) {
				string[] array = cell.Split(',');
				int countInteger = 0;
				int result;
				foreach (string s in array) {
					if (s.Contains(".") == false && int.TryParse(s, out result)) {
						countInteger++;
					}
				}
				if (countInteger == array.Length) {
					return true;
				}
			}
			return false;
		}
		public override string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			if (cell == "") {
				return "new int[]{}";
			} else {
				List<string> values = new List<string>();
				foreach (string s in cell.Split(',')) {
					float parsed;
					if (s.Trim().Length == 0) {
						values.Add("0");
					} else if (float.TryParse(s, out parsed)) {
						values.Add(parsed.ToString());
					} else {
						values.Add(s.Trim());
					}
				}
				return "new int[]{" + string.Join(",", values.ToArray()) + "}";
			}
		}
		public override bool IsValidType(Type type) {
			return type == typeof(int[]);
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			string tempVar = "tmp" + index;
			return StringUtilities.Format(@"
				{0} = CleanString({0});	
				string[] {1} = {0}.Split(',');
				_rows[i].{2} =  ({0} == """") ? new int[]{} : Array.ConvertAll<string, int>({1}, ParseInt);",
				cellVar,
				tempVar,
				attribute
			);
		}
	}
}

