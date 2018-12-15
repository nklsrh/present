namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	public class PluginBoolArray : PluginBool {
		public PluginBoolArray() : base(eType._boolArray, "bool[]", true) {
		}
		public override bool IsValid(string cell) {
			if (cell.Contains(",")) {
				string[] array = cell.Split(',');
				int countBool = 0;
				foreach (string s in array) {
					string trim = s.Trim().ToLower();
					if (trim == "true" || trim == "false" || trim == "1" || trim == "0") {
						countBool++;
					}
				}
				if (countBool == array.Length && countBool > 1) {
					return true;
				}
			}
			return false;
		}
		public override string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			if (cell == "") {
				return "new bool[]{}";
			} else {
				string[] array = cell.Split(',');
				List<string> values = new List<string>();
				foreach (string s in array) {
					bool parsed = false;
					if(s.Trim().Length == 0) {
						values.Add("false");
					}else if(TryParse(s.Trim(), out parsed)) {
						values.Add(parsed.ToString().ToLower());
					} else {
						values.Add(s.Trim());
					}
				}
				return "new bool[]{" + string.Join(",", values.ToArray()) + "}";
			}
		}
		public override bool IsValidType(Type type) {
			return type == typeof(bool[]);
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			string tempVar		= "tmp" + index;
			return StringUtilities.Format(@"
				{0} = CleanString({0});
				string[] {1} = {0}.Split(',');
				_rows[i].{2} = ({0} == """") ? new bool[] { } : Array.ConvertAll<string, bool>({1}, ParseBool);",
				cellVar,
				tempVar,
				attribute
			);
		}
	}
}

