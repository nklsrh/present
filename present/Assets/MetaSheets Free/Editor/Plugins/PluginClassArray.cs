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
	public class PluginClassArray : PluginClass {
		public PluginClassArray() : base(eType._ClassArray, "", true) {
		}
		public override string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			string[] array_label = columnName.Split(' ');//CONTENT OF THE COLUMN LABEL SPLIT BY SPACE
			if (array_label.Length == 2) {
				//string classNameSheet	= GetName_ClassRow(array_label[0].Replace("[","").Replace("]",""));
				//string varName			= GetName_Variable(columnName);
				string constructor = "new " + MetaSheets.GetName_ClassSheetNameSpace(className) + "." + MetaSheets.GetName_ClassRow(array_label[0]) + "{ {0} }";
				string classSheet   = columnName.Split(' ').First().Replace("[", "").Replace("]", "");
				string[] array = cell.Split(',');//SPLIT CELL-ARRAY ELEMENTS BY ','
				List<string> list = new List<string>();
				bool isError = false;
				foreach (string item in array) {
					string[] array_item = item.Trim().Split('.');//CONTENT OF THE CELL SPLIT BY '.'
					if(item.Trim().Length == 0) {
						list.Add("null");
					}else if(array_item.Length == 2) {
						list.Add(className + "." + item);
					} else if(array_item.Length == 1) {
						list.Add(className + "." + classSheet+"."+item);
					} else {
						//ERROR DOES NOT MATCH UP
						isError = true;
					}
				}
				if (isError) {
					// SOMETHING WENT WRONG, INFORM THE USER
					string format;
					if (array_label.Length >= 1) {
						format = classSheet + ".idA, " + classSheet + ".idB";
					} else {
						format = "mySheetId.idA, mySheetId.idB";
					}
					Copy.copy.help_wrongClassArrayItem.ShowHelp(Help.eImage.cellWrongClassArray, sheetName, cell, format);
				}
				return StringUtilities.Format(constructor, string.Join(", ", list.ToArray()));
			}
			return "null";//DEFAULT VALUE FOR WHEN NO CLASS COULD BE DETERMINED
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			string classSheet	= MetaSheets.GetName_ClassRow( columnName.Split(' ').First().Replace("[","").Replace("]",""));
			return StringUtilities.Format(@"
				List<{0}> tmp{1} = new List<{0}>();
				foreach(string s in {2}.Split(',')) {
					tmp{1}.Add( Data.people[ s.Split('.').Last() ] );
				}
				_rows[i].{3} = tmp{1}.ToArray();",
				classSheet,
				index,
				cellVar,
				attribute
			);
		}
	}
}

