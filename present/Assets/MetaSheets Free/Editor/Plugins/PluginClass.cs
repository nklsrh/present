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
	public class PluginClass : Plugin {
		public PluginClass(eType type = eType._Class, string columnKeyWord = "", bool isCommaValue = false) : base(type, columnKeyWord, isCommaValue) {
		}
		public override string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			string[] array_cell = cell.Split('.');//CONTENT OF THE CELL SPLIT BY '.'
			string[] array_label = columnName.Split(' ');//CONTENT OF THE COLUMN LABEL SPLIT BY SPACE
			if(cell.Length == 0) {
				return "null";
			}else if (array_cell.Length == 2) {
				//HAS A SHEET REFERENCE AND VALUE, LETS ASSUME ITS ALL GOOD
				return className + "." + cell;
			}else if(array_cell.Length == 1) {
				//ONLY HAS A VALUE, FIND THE SHEET REFERENCE BY COLUMN NAME
				string classSheet = columnName.Split(' ').First();
				return className + "." + classSheet + "." + cell;
			}
			// SOMETHING WENT WRONG, INFORM THE USER
			string format;
			if (array_label.Length >= 1) {
				format = array_label[0] + ".myId";
			} else {
				format = "mySheetId.myId";
			}
			Copy.copy.help_wrongClassItem.ShowHelp(Help.eImage.cellWrongClass, sheetName, className, cell, format);
			return className + "." + cell;
		}
		public override string GetTypeName(string cellValue, string className, string sheetName, string columnName) {
			string[] arrayLabel = columnName.Split(' ');
			string[] arrayValue = cellValue.Trim().Split('.');
			string[] arrayTitle = sheetName.Trim().Split(':');
			if (arrayLabel.Length == 2) {
				string classVariable = arrayLabel[0];//THE CLASS NAME PART OF THE COLUMN LABEL
				return MetaSheets.GetName_ClassSheetNameSpace(className) + "." + MetaSheets.GetName_ClassRow(classVariable);
			} else if (arrayValue.Length == 2 && arrayTitle.Length == 2 && arrayLabel.Length == 1) {
				string varName = columnName;
				string varClassName = arrayTitle[1];
				System.Type typeVar = MetaSheets.GetTypeVarBase(varClassName, varName);
				if (typeVar != null) {
					return typeVar.ToString();
				}
			}
			return "";
			//GetVariable_Class: people.item_00 : people[] who
			//} else if (type == eType._eNumArray) {
			//	string prefix = GetEnumPrefix(columnName, sheetName);
			//	if (prefix != "") {
			//		return prefix+"[]";
			//	}
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			string classSheet   = columnName.Split(' ').First();
			return StringUtilities.Format(@"
				_rows[i].{0} = {1}.{2}[ {3}.Split('.').Last() ];",
				attribute,
				className,
				classSheet,
				cellVar
			);
		}
	}
}

