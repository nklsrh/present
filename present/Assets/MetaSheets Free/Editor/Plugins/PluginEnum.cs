namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	public class PluginEnum : Plugin {
		protected static string noneKeyword = "_default";
		public PluginEnum(eType type = eType._eNum, string columnKeyWord = "enum") : base(type, columnKeyWord) {
		}
		override public string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			string prefix = GetEnumPrefix(className, sheetName, columnName);
			return prefix + "." + StringUtilities.GetCamelCase(cell).Split('.').Last();
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			string prefix		= GetEnumPrefix(className, sheetName, columnName);
			return StringUtilities.Format(@"
					_rows[i].{0} = ({1})System.Enum.Parse( typeof({1}), ({2} == """") ? ""{3}"" : GetCamelCase({2}));",
				attribute, prefix, cellVar, noneKeyword
			);
		}
		public override string GetTypeName(string cellValue, string className, string sheetName, string columnName) {
			return WrapArray(GetEnumPrefix(className, sheetName, columnName)) + "";
		}
		private string WrapArray(string input) {
			if (this.type == eType._eNumArray && !input.Contains("[]")) {
				return input + "[]";
			}
			return input;
		}
		protected string GetEnumPrefix(string className, string sheetName, string columnName) {
			string classExtend  = MetaSheets.GetName_ClassRowExtends(sheetName);
			string nameVariable = MetaSheets.GetName_Variable(columnName);
			bool doesExtendingVarExist  = MetaSheets.DoesVariableExist(classExtend, nameVariable);
			if (doesExtendingVarExist) {
				System.Type _type = MetaSheets.GetTypeExtended(classExtend, nameVariable);
				if (_type.DeclaringType == null && _type.FullName.Contains("+")) {
					//Return e.g. DBase+myEnumTypes[], replace + with .
					return _type.FullName.ToString().Replace("+", ".").Replace("[]","");
				} else {
                    Debug.Log("type; " + classExtend + " : " + nameVariable);
					return _type.DeclaringType.ToString() + "." + _type.Name.Replace("[]", "");
				}
			} else {
				string classRowName     = MetaSheets.GetName_ClassRow(sheetName);
				string nameENum         = MetaSheets.GetName_eNum(columnName);
				return classRowName + "." + nameENum.Replace("[]", "");
			}
		}
		private static string[] illegalChars = new string[] { ".", "-", "|", "\"", "'" };//ANY ENUM TYPE SHOULD NOT CONTAIN THESE
#region Check For Enums
		public static bool IsEnum(string[] cells) {
			return IsEnumValues(cells);
		}
		public static bool IsEnumArray(string[] cells) {
			List<string> values = new List<string>();
			int countArrayMax = 0;
			foreach (string cell in cells) {
				string[] array = cell.Split(',');
				countArrayMax = Math.Max(countArrayMax, array.Length);
				foreach (string item in array) {
					if(item.Trim().Length > 0) {
						values.Add(item);
					}
				}
			}
			if (countArrayMax <= 1) {
				//All cells contain only 1 element, might as well consider enum instead of enum[]
				return false;
			}
			return IsEnumValues(values.ToArray());
		}
		private static bool IsEnumValues(string[] values) {
			int i, j;
			for (i = 0; i < values.Length; i++) {
				//Check against illegal Characters
				for (j = 0; j < illegalChars.Length; j++) {
					if (values[i].Contains(illegalChars[j])) {
						return false;
					}
				}
				values[i] = StringUtilities.GetCamelCase( values[i] );
			}
			//COUNT DUPLICATES....
			List<string> uniqueIds = new List<string>();
			List<int> uniqueCounts = new List<int>();
			int countDoublesTotal = 0;
			int countMaxUniqueCounts = 0;
			int countMaxStringLength = 0;
			foreach (string cell in values) {
				//FIRST CHARACTER CAN NOT BE A NUMBER
				if (cell.Length > 0) {
					int parseTry;
					if (int.TryParse(cell.Substring(0, 1), out parseTry)) {
						return false;
					}
				}
				if (cell.Length > 0) {
					countMaxStringLength = Mathf.Max(countMaxStringLength, cell.Length);
					if (!uniqueIds.Contains(cell)) {
						uniqueIds.Add(cell);
						uniqueCounts.Add(0);
					} else {
						uniqueCounts[uniqueIds.IndexOf(cell)]++;
						countDoublesTotal++;
						countMaxUniqueCounts = Mathf.Max(countMaxUniqueCounts, uniqueCounts[uniqueIds.IndexOf(cell)]);
					}
				}
			}
			if (countMaxStringLength < 24) {//NOT MORE THAN 24 characters
				if (countMaxUniqueCounts >= 1) {//AT LEAST 2 of ONE GROUP ARE THE SAME
					if (countDoublesTotal >= 1) {//AT LEAST 2 TOTAL COPIES OF SOMETHING
						return true;
					}
				}
			}
			return false;
		}
#endregion
	}
}

