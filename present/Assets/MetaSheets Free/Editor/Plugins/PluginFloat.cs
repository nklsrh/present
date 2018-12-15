namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	public class PluginFloat : Plugin {
		public PluginFloat() : base(eType._float, "float") {
		}
		public override bool IsValid(string cell) {
			float valFloat;
			if (float.TryParse(cell, out valFloat)) {
				return true;
			}
			return false;
		}
		public override string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			float parseTry;
			if(cell.Length == 0) {
				return "0f";
			}else if (float.TryParse(cell, out parseTry)) {
				return parseTry.ToString() + "f";
			} else {
				return cell;
			}
		}
		public override bool IsValidType(Type type) {
			return type == typeof(float);
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			string template = @"
				{0} {1}; _rows[i].{2} = {0}.TryParse( {3}, out {1}) ? {1} : {4};";
			return StringUtilities.Format(template, "float", "tmp" + index, attribute, cellVar, "0f");
		}
	}
}

