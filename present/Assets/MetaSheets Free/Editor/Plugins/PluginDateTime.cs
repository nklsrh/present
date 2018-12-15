namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using UnityEngine;
	public class PluginDateTime : Plugin {
		private Regex regexNumber = new Regex("^[0-9]+$");
		public PluginDateTime() : base(eType._dateTime, "DateTime") {
		}
		public override bool IsValid(string cell) {
			cell = Trim(cell);
			if (cell.Contains("/")) {
				string[] array = cell.Split('/');
				if(array.Length == 3) {
					int l0 = array[0].Length;
					int l1 = array[1].Length;
					int l2 = array[2].Length;
					//First pair 2 or 4 length, or last item. YYYY/MM/DD or DD/MM/YYYY
					if ((l0 <= 2 || l0 == 4) && l1 <= 2 && (l2 <= 2 || l2 == 4)) {
						if (regexNumber.IsMatch(array[0]) && regexNumber.IsMatch(array[1]) && regexNumber.IsMatch(array[2])) {
							return true;
						}
					}
				}
			}
			return false;
		}
		override public string GetValue(string cell, string className, string sheetName, string columnName) {
			cell = Trim(cell);
			string[] array = cell.Split('/');
			if(cell.Trim().Length == 0) {
				return "new DateTime()";//DEFAULT RETURN VALUE
			}else if(array.Length == 3) {
				if(array[0].Length == 4) {
					//Format: YYYY/MM/DD
					return StringUtilities.Format("new DateTime({0},{1},{2})", array[0], array[1], array[2]);
				} else if(array[2].Length == 4) {
					//Format: DD/MM/YYYY
					return StringUtilities.Format("new DateTime({2},{1},{0})", array[0], array[1], array[2]);
				}
			}
			return "new DateTime( "+cell+" )";
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			return StringUtilities.Format(@"
				Debug.Log("">> ""+{2});
				if( {2}.Trim().Length > 0 ) {
					int[] tmp{0} = Array.ConvertAll( {2}.Split('/'), int.Parse);
					if(tmp{0}.Length == 3 && tmp{0}[0].ToString().Length == 4){
						_rows[i].{1} = new DateTime( tmp{0}[0], tmp{0}[1], tmp{0}[2] );//YYYY/MM/DD
					}else if(tmp{0}.Length == 3 && tmp{0}[2].ToString().Length == 4){
						_rows[i].{1} = new DateTime( tmp{0}[2], tmp{0}[1], tmp{0}[0] );//DD/MM/YYYY
					}else{
						Debug.LogError(""Wrong format for DateTime cell '{2}'"");
					}
				}
				", 
				index,
				attribute, 
				cellVar
			);
		}
	}
}

