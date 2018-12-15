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
	public class PluginEnumArray : PluginEnum {
		public PluginEnumArray() : base(eType._eNumArray, "enum[]") {
		}
		override public string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			string prefix = GetEnumPrefix(className, sheetName, columnName);
			string[] array = cell.Split(',');
			List<string> list = new List<string>();
			foreach(string item in array) {
				if(item.Trim().Length > 0) {
					list.Add(prefix + "." + StringUtilities.GetCamelCase(item).Split('.').Last() );
				}
			}
			return StringUtilities.Format("new {0}[]{{1}}", prefix, string.Join(",", list.ToArray()) );
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute		= MetaSheets.GetName_Variable(columnName);
			string prefix			= GetEnumPrefix(className, sheetName, columnName);
			string varA = string.Format("tmp{0}", index);
			return StringUtilities.Format(@"
				List<{1}> {2} = new List<{1}>();
				foreach(string val in columns[{0}].Split(',')) {
					if(val.Trim().Length > 0) {
						{2}.Add(({1})System.Enum.Parse(typeof({1}), GetCamelCase(val)));
					}
				}
				_rows[i].{3} = {2}.ToArray();		//prefix: '"+ prefix+ "' columnName: '"+ columnName+"'",
				index,
				prefix,
				varA,
				attribute
			);
		}
	}
}

