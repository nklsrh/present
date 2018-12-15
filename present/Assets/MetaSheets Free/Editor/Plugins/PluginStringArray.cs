namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	public class PluginStringArray : Plugin {
		private static string[] illegalChars = new string[] { "\n", "\r", "\t", "\"" };
		public PluginStringArray() : base(eType._stringArray, "string[]", true) {
		}
		public override bool IsValid(string cell) {
			if (cell.Contains(",")) {
				string[] array = cell.Split(',');
				int countValid = 0;
				foreach (string s in array) {
					bool isValid = true;
					foreach (string c in illegalChars) {
						if (s.Contains(c)) {
							isValid = false;
							break;
						}
					}
					if (isValid) {
						countValid++;
					}
				}
				if (countValid == array.Length) {
					return true;
				}
			}
			return false;
		}
		public override string GetValue(string cell, string className, string sheetName, string columnName) {
			if (cell == "") {
				return "new string[]{}";
			} else {
				// Comma spaces we don't want the spaces
				cell = cell.Replace(", ", ",");
				//Remove illegal characters
				foreach (string c in illegalChars) {
					cell = cell.Replace(c, "");
				}
				string[] array = cell.Split(',');
				return "new string[]{\"" + string.Join("\",\"", array) + "\"}";
			}
		}
		public override bool IsValidType(Type type) {
			return type == typeof(string[]);
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			return StringUtilities.Format(@"
				{0} = CleanString({0});
				_rows[i].{1} = ({0} == """") ? new string[]{} : {0}.Split(',');",
				cellVar,
				attribute
			);
		}
	}
}

