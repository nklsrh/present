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
	public class PluginColor : Plugin {
		private Regex regexColor = new Regex("^[a-fA-F0-9]+$");
		public PluginColor(eType type = eType._Color, string columnKeyWord = "Color", bool isArray = false) : base(type, columnKeyWord, isArray) {
		}
		//string[] validCharacters = ("0,1,2,3,4,5,6,7,8,9,#,a,b,c,d,e,f").Split(',');
		override public bool IsValid(string cell) {
			cell = Trim(cell).ToLower();
			string hex = cell.Replace("#", "");
			if (regexColor.IsMatch(hex) || cell.IndexOf("#") == 0) {//(cell.IndexOf("#") == 0 && cell.Length%2 == 1) || (cell.Contains("#") == false && cell.Length % 2 == 0)
				if (hex.Length == 6 || hex.Length == 8) {
					byte r;
					byte g;
					byte b;
					if (byte.TryParse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out r)) {
						if (byte.TryParse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out g)) {
							if (byte.TryParse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b)) {
								return true;
							}
						}
					}
				}
			}
			return false;
		}
		override public string GetValue(string cell, string className, string sheetName, string columnName) {
			Color color;
			if(cell.Trim().Length == 0) {
				return "Color.black";//Default Color
			}
			if (TryParseColor(Trim(cell), out color)) {
				return "new Color(" + color.r + "f," + color.g + "f," + color.b + "f," + color.a + "f)";
			}
			return cell;
		}
		protected bool TryParseColor(string input, out Color color) {
			input = input.Trim();
			string hex = input.Replace("#", "");
			if (regexColor.IsMatch(hex) || input.IndexOf("#") == 0) {
				if (hex.Length == 6 || hex.Length == 8) {
					byte b0;
					byte b1;
					byte b2;
					byte b3;
					if (byte.TryParse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b0)) {
						if (byte.TryParse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b1)) {
							if (byte.TryParse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b2)) {
								if (input.Length == 9) {
									if (byte.TryParse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b3)) {
										color = new Color((float)b1 / 255f, (float)b2 / 255f, (float)b3 / 255f, (float)b0 / 255f);
										return true;
									}
								} else {
									color = new Color((float)b0 / 255f, (float)b1 / 255f, (float)b2 / 255f);
									return true;
								}
							}
						}
					}
				}
			}
			color = Color.black;
			return false;
		}
		public override bool IsValidType(Type type) {
			return type == typeof(Color);
		}
		public override string FormatReload(int index, string cellVar, string className, string sheetName, string columnName) {
			string attribute    = MetaSheets.GetName_Variable(columnName);
			string tempVar		= "tmp" + index;
			return StringUtilities.Format(@"
			Color {1};
			TryParseColor( CleanString({0}) , out {1});
			_rows[i].{2} = {1};",
				cellVar,
				tempVar,
				attribute
			);
		}
		public static string formatReloadTryParseColorMethod = @"
		private bool TryParseColor(string input, out Color color){
			input = input.Trim();
			System.Text.RegularExpressions.Regex regexColor = new System.Text.RegularExpressions.Regex(""^[a-fA-F0-9]+$"");
			string hex = input.Replace(""#"","""");
			if (regexColor.IsMatch(hex) || input.IndexOf(""#"") == 0){
				if (hex.Length == 6 || hex.Length == 8){
					byte b0; 
					byte b1; 
					byte b2; 
					byte b3;
					if (byte.TryParse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b0)){
						if (byte.TryParse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b1)){
							if (byte.TryParse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b2)){
								if (input.Length == 9){
									if (byte.TryParse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b3)){
										color = new Color((float)b1/255f, (float)b2/255f, (float)b3/255f, (float)b0/255f);
										return true;
									}
								}else{
									color = new Color((float)b0/255f, (float)b1/255f, (float)b2/255f);
									return true;
								}
							}	
						}
					}
				}
			}
			color = Color.black;
			return false;
		}			
		";
	}
}

