using UnityEngine;
using System.Collections;
using UnityEditor;
///		Meta Sheets Pro
///		Copyright © 2017 renderhjs
///		Version 2.00.0
///		Website www.metasheets.com
public class DMetaSheetsCopy {
	public string copy;
	public string title;
	/// <summary>
	/// Open alert popup
	/// </summary>
	public void ShowAlert(params object[] arguments) {
		EditorUtility.DisplayDialog(Title(arguments), Copy(arguments), "Ok");
	}
	public string Copy(params object[] arguments) {
		return Format(this.copy, arguments);
	}
	public string Title(params object[] arguments) {
		return Format(this.title, arguments);
	}
	/// <summary>
	/// Show the help panel within the MetaSheets Window
	/// </summary>
	/// <param name="arguments">Optional dynamic arguments to parse in the copy</param>
	public void ShowHelp(params object[] arguments) {
		ShowHelp(MetaSheets.Help.eImage.none, arguments);
	}
	/// <summary>
	/// Show the help panel within the MetaSheets Window withthe image of eImage
	/// </summary>
	/// <param name="image">which image to display</param>
	/// <param name="arguments">Optional dynamic arguments to parse in the copy</param>
	public void ShowHelp(MetaSheets.Help.eImage image, params object[] arguments) {
		MetaSheets.Help.ShowHelp(Title(arguments), Copy(arguments), image);
	}
	/// <summary>
	/// Return the ID of this edge
	/// </summary>
	public static implicit operator string(DMetaSheetsCopy item) {
		return item.copy;
	}
	/// <summary>
	/// Helper method to format {0},{1},... variables with arguments
	/// </summary>
	/// <param name="template">The string template that may or may not contain {x} variables</param>
	/// <param name="arguments">The arguments to be inserted as strings into the variables</param>
	/// <returns></returns>
	private string Format(string template, params object[] arguments) {
		string output = template;
		for (int i = 0; i < arguments.Length; i++) {
			output = output.Replace("{" + i.ToString() + "}", arguments[i].ToString());
		}
		return output;
	}
}
