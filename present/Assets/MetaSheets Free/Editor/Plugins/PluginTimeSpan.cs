namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	public class PluginTimeSpan : Plugin {
		public PluginTimeSpan() : base(eType._TimeSpan, "TimeSpan") {
		}
		public override bool IsValid(string cell) {
			//IS TIME VALUE?
			if (cell.Contains(":")) {
				string[] array = cell.Split(':');
				if (array.Length == 2 || array.Length == 3) {
					int countValid = 0;
					for (int i = 0; i < array.Length; i++) {
						int parseTry;
						if (int.TryParse(array[i], out parseTry)) {
							if (parseTry >= 0 || parseTry < 60) {
								countValid++;
							} else {
								break;
							}
						} else {
							break;
						}
					}
					if (countValid == array.Length) {
						return true;
					}
				}
			}
			return false;
		}
		public override string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			string[] array = cell.Split(':');
			if(cell.Length == 0) {
				return "new TimeSpan( 0,0,0 )";//Default Value
			} else if (array.Length == 2) {
				return ("new TimeSpan( " + array[0] + "," + array[1] + ", 0 )");
			} else if (array.Length == 3) {
				return "new TimeSpan( " + array[0] + "," + array[1] + "," + array[2] + " )";
			}
			return "new TimeSpan( " + cell + " )";
		}
		public override bool IsValidType(Type type) {
			return type == typeof(TimeSpan);
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			//IS IT 1, 2 or 3 values
			string tempVar = "tmp" + index;
			string template = @"
					if ({0}.Trim() == """") {
						_rows[i].{2} = new TimeSpan();
					} else {
						int[] {1} = Array.ConvertAll<string, int>({0}.Split(':'), int.Parse);
						_rows[i].{2} = ({1}.Length == 3) ? new TimeSpan({1}[0], {1}[1], {1}[2]) : new TimeSpan({1}[0], {1}[1], 0);
					}";
			return StringUtilities.Format(template, cellVar, tempVar, attribute);
		}
	}
}

