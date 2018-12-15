namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	public enum eType {
		_Vector2,
		_Vector3,
		_bool,
		_float,
		_int,
		_string,
		_Color,
		_ColorArray,
		_intArray,
		_floatArray,
		_stringArray,
		_boolArray,
		_dateTime,
		_TimeSpan,
		_eNum,
		_eNumArray,
		_Class,
		_ClassArray,
	}
	public class Plugin {
		public readonly eType type;
		public readonly string columnKeyWord;
		public readonly bool isCommaValue;
		public Plugin(eType type, string columnKeyWord, bool isCommaValue = false) {
			this.type = type;
			this.columnKeyWord = columnKeyWord;
			this.isCommaValue = isCommaValue;
		}
		public virtual string GetTypeName(string cellValue, string className, string sheetName, string columnName) {
			return columnKeyWord;
		}
		public virtual bool IsValid(string cell) {
			return false;
		}
		public virtual string GetValue(string cell, string className, string sheetName, string columnName) {
			return "null" + String.Format("", type.ToString(), cell);
		}
		public virtual bool IsValidType(Type type) {
			return false;
		}
		public virtual string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			return StringUtilities.Format(@"
				Debug.LogError(""Reload not implemented for {0} or type {1}"");", attribute, type.ToString());
		}
		protected string Trim(string cell) {
			return cell.Replace(" ", "").Replace("\t", "").Trim();
		}
	}
}
