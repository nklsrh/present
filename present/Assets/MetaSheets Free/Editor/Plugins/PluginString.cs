namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	public class PluginString : Plugin {
		public PluginString() : base(eType._string, "string") {
		}
		override public bool IsValid(string cell) {
			//Strings are always valid
			return true;
		}
		override public string GetValue(string cell, string className, string sheetName, string columnName) {
			//Escape \ \n \r characters and put int quotation marks
			return "\"" + cell.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\n") + "\"";
		}
		override public string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			//ALSO SPLIT NEW LINES WHEN RELOADING
			return string.Format(@"
				_rows[i].{0} = ({1} == null) ? """" : {1}.Replace(""\\\"""", ""\"""").Replace(""\\n"", ""\n"").Replace(""\\r"", ""\r"").Replace(""\r"",""\n"");",
				attribute, cellVar
			);
		}
		public override bool IsValidType(Type type) {
			return type == typeof(string);
		}
	}
}
