namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	public class PluginInt : Plugin {
		public PluginInt() : base(eType._int, "int") {
		}
		override public bool IsValid(string cell) {
			int valInt;
			if (int.TryParse(cell, out valInt))
				return true;
			return false;
		}
		override public string GetValue(string cell, string className, string sheetName, string columnName) {
			int parseTry;
			if(cell.Trim().Length == 0) {
				return "0";
			}else if (int.TryParse(cell, out parseTry)) {
				return parseTry.ToString();
			} else {
				return cell;
			}
		}
		public override bool IsValidType(Type type) {
			return type == typeof(int);
		}
		override public string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			string template = @"
				{0} {1}; _rows[i].{2} = {0}.TryParse( {3}, out {1}) ? {1} : {4};";
			return string.Format(template, "int", "tmp" + index, attribute, cellVar, "0");
		}
	}
}

