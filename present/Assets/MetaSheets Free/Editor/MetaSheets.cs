namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Text.RegularExpressions;
	using SimpleJSONMetaSheets;
	using System.Reflection;
	public class MetaSheets : EditorWindow {
		#region VARS
		public const string VERSION = "2.00.0";
		public static Configuration configuration = new Configuration("","");
		private const float loadTimeout = 45f;//DEFAULT TIMEOUT WHEN LOADING THE INDEX 
		#endregion
		void OnEnable() {
			if(window != null && this != window) {
				window.Close();
			}
			ResetApplication();
		}
		private void OnDestroy() {
			window = null;
		}
		/// <summary>
		/// Menu item to Initialise the main window
		/// </summary>
		[MenuItem("Assets/MetaSheets", false, 251)]
		public static void InitMenuItem() {
			ResetApplication();
		}
		/// <summary>
		/// Build a specific document. Can be used from Editor scripts.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="documentKey"></param>
		/// <param name="doAddReloadCode"></param>
		public static void Build(string className, string documentKey, bool doAddReloadCode = false) {
			Build(new Configuration(className, documentKey, doAddReloadCode), null);
		}
	/// <summary>
	/// Manual Build of a specific document
	/// </summary>
	/// <param name="configuration">Configuration settings for the document and class</param>
	public static void Build(Configuration configuration, EditorWindow window) {
			if (!Application.isPlaying) {
				LoadIndex(configuration, window);
			} else {
				Copy.copy.popup_cantBuildWhileRunning.ShowAlert();
			}
		}
		/// <summary>
		/// Reset Unity Sheet
		/// </summary>
		private static void ResetApplication() {
			//This is called when the script recompiles
			Load();
			EditorUtility.ClearProgressBar();
			UserInterface.InitWindow(Window);
		}
		void OnGUI() {
			if (UserInterface.OnGUI_EditorWindow(Window, configuration)) {
				Configuration.Cache = configuration;
			}
		}
#region SAVE AND LOAD SETTINGS
		public static void Save() {
			Configuration.Cache = configuration;
			Settings.Save(configuration);
		}
		private static void Load() {
			configuration = Configuration.Cache;
		}
#endregion
#region LOAD JSON
		/// <summary>
		/// Load the first indx json that contains all the sub sheets of the spreadsheet to load
		/// </summary>
		private static void LoadIndex(Configuration configuration, EditorWindow window) {
			EditorUtility.DisplayProgressBar("Loading", "Fetching sheets", 0.1f);
			string url = configuration.DocumentURL;
#if UNITY_WEBPLAYER || UNITY_WEBGL
			if (configuration.proxyURL.Length > 0){//USE PROXY INSIDE UNITY IF WEBPLAYER, AS IT WILL BLOCK GOOGLE WITHOUT CROSSDOMAIN
				url = configuration.proxyURL + ""+ WWW.EscapeURL(url);
			}
#endif
			float time = (float)EditorApplication.timeSinceStartup;
			WWW request = new WWW(url);
			while (request.isDone != true) {
				float t = (float)EditorApplication.timeSinceStartup - time;
				if (t >= loadTimeout) {
					Copy.copy.help_timedOutIndex.ShowHelp(loadTimeout);
					EditorUtility.ClearProgressBar();
					request.Dispose();
					return;
				}
			}
			if (request.error != null && request.error.Contains("Bad Request")) {
				Copy.copy.help_400BadRequest.ShowHelp(Help.eImage.copyUrl, request.error);
				EditorUtility.ClearProgressBar();
				return;
			}
			if (request.text.Contains("<!DOCTYPE html>") && request.text.IndexOf("<!DOCTYPE html>") <= 10) {
				Copy.copy.help_notAccessable.ShowHelp(Help.eImage.publish);
				EditorUtility.ClearProgressBar();
				return;
			}
			//Clean up JS Commands And Comments
			JSONNode json = JSON.Parse(request.text);
			configuration.documentTitle = json["feed"]["title"]["$t"].Value;
			if (window  != null && window == Window) {
				Save();
			}
			int numSheets = int.Parse(json["feed"]["openSearch$totalResults"]["$t"].Value);
			List<string> documentGIDs = new List<string>();
			List<string> documentTitles = new List<string>();
			for (int i = 0; i < numSheets; i++) {
				string title = json["feed"]["entry"][i]["title"]["$t"].Value;
				//Skip sheets that are commented out
				if (title.IndexOf("//") != 0) {
					string gid = json["feed"]["entry"][i]["id"]["$t"].Value;
					gid = gid.Substring(gid.LastIndexOf("/") + 1);
					documentGIDs.Add(gid);
					documentTitles.Add(title);
				}
			}
			EditorUtility.DisplayProgressBar("Loading", "Fetching amount of pages", 0.3f);
			LoadSheets(configuration, documentGIDs.ToArray(), documentTitles.ToArray(), window);
		}
		/// <summary>
		/// Load the individual sheets
		/// </summary>
		/// <param name="documentGIDs">Document GI ds.</param>
		private static void LoadSheets(Configuration configuration, string[] documentGIDs, string[] documentTitles, EditorWindow window) {
			string[] jsonDocs = new string[documentGIDs.Length];
			for (int i = 0; i < documentGIDs.Length; i++) {
				string url = "https://spreadsheets.google.com/feeds/cells/" + configuration.documentKey + "/" + documentGIDs[i] + "/public/basic?alt=json-in-script";
#if UNITY_WEBPLAYER || UNITY_WEBGL
				if (configuration.proxyURL.Length > 0){//USE PROXY INSIDE UNITY IF WEBPLAYER, AS IT WILL BLOCK GOOGLE WITHOUT CROSSDOMAIN
					url = configuration.proxyURL + ""+ WWW.EscapeURL(url);
				}
#endif
				float time = (float)EditorApplication.timeSinceStartup;
				WWW request = new WWW(url);
				while (request.isDone != true) {
					float t = (float)EditorApplication.timeSinceStartup - time;
					if (t >= loadTimeout) {
						Copy.copy.help_timedOutSheet.ShowHelp(documentTitles[i], loadTimeout);
						EditorUtility.ClearProgressBar();
						request.Dispose();
						return;
					}
					float pA = (float)(i + request.progress) / (float)documentGIDs.Length;//% chunk of this page
					float pSub = Mathf.Min(1f, t / 2f) * 1f / (float)documentGIDs.Length;//Relative time offset of this page
					float pTotal = 0.3f + pA * 0.7f + pSub;
					pTotal = Mathf.Clamp(pTotal, 0f, 1f);
					EditorUtility.DisplayProgressBar("Loading " + Mathf.RoundToInt(pTotal * 100) + "%", "Sheet " + documentTitles[i] + "", pTotal);
				}
				jsonDocs[i] = GetJson(request.text);//Clean up JS commands and comments
			}
			EditorUtility.ClearProgressBar();
			ParseJsonPages(configuration, documentGIDs, jsonDocs, window);
		}
		private static string GetJson(string inp) {
			int A = inp.IndexOf("{");
			int B = inp.LastIndexOf("}");
			if (A != -1 && B != -1) {
				return inp.Substring(A, B - A + 1);
			}
			return inp;
		}
#endregion
#region PARSE JSON
		private static List<string[][]> all_Cells;
		private static string[] all_SheetNames;
		private static string[] all_SheetGIDs;
		/// <summary>
		/// Parse multiple JSON files that were loaded, one json file = one sheet
		/// </summary>
		private static void ParseJsonPages(Configuration configuration, string[] documentGIDs, string[] jsonDocs, EditorWindow window) {
			int j;
			int k;
			string classes = "";
			all_SheetNames	= new string[jsonDocs.Length];
			all_Cells		= new List<string[][]>();
			all_SheetGIDs	= documentGIDs;
			bool errorExceededRowsColumns = false;
			for (int i = 0; i < jsonDocs.Length; i++) {
				JSONNode json = JSON.Parse(jsonDocs[i]);
				string title = json["feed"]["title"]["$t"].Value;
				JSONNode entries = json["feed"]["entry"];
				int maxCols = 1;
				int maxRows = 1;
				string[][] cells = new string[4000][];//Maximum rows
				for (j = 0; j < cells.Length; j++) {
					cells[j] = new string[columnNames.Length];//Maximum columns
				}
				for (j = 0; j < entries.Count; j++) {
					string cellId = entries[j]["title"]["$t"].Value;        //A1, B1, A2,...
					string cellContent = entries[j]["content"]["$t"].Value; //The cell value
					int[] colRow = GetColumnRow(cellId);
					maxCols = Mathf.Max(maxCols, colRow[0] + 1);
					maxRows = Mathf.Max(maxRows, colRow[1] + 1);
					if (colRow[1] < cells.Length && colRow[0] < cells[0].Length) {//Fits inside an constructor array
						cells[colRow[1]][colRow[0]] = cellContent;//Assign value
					} else {
						errorExceededRowsColumns = true;
					}
				}
				maxCols = Mathf.Min(maxCols, cells[0].Length);
				maxRows = Mathf.Min(maxRows, cells.Length);
				//Trim Array
				string[][] cellsTrimmed = new string[maxRows][];
				for (j = 0; j < maxRows; j++) {
					cellsTrimmed[j] = new string[maxCols];
					System.Array.Copy(cells[j], cellsTrimmed[j], maxCols);
				}
				cells = cellsTrimmed;
				//Remove comented out columns
				List<int> keepColumns = new List<int>();
				for (j = 0; j < cells[0].Length; j++) {
					if (cells[0][j] != null) {
						if (cells[0][j].IndexOf("//") != 0) {
							keepColumns.Add(j);
						}
					}
				}
				if (keepColumns.Count != cells[0].Length) {
					for (j = 0; j < cells.Length; j++) {
						List<string> row = new List<string>();
						for (k = 0; k < keepColumns.Count; k++) {
							row.Add(cells[j][keepColumns[k]]);
						}
						cells[j] = row.ToArray();
					}
				}
				//Remove commented out rows
				List<int> keepRows = new List<int>();
				for (j = 0; j < cells.Length; j++) {
					if (cells[j][0] == null || cells[j][0] == "") {
						//Skip this row if all cells of this row are empty or null
						int count = 0;
						for (k = 0; k < cells[j].Length; k++) {
							if (cells[j][k] == null || cells[j][k] == "") {
								count++;
							}
						}
						if (count >= cells[j].Length) {
							continue;
						}
					}
					if (cells[j].Length == 0 || cells[j][0] == null) {
						//The first cell is empty or null, but others of this row are not
						Copy.copy.help_wrongSheetSetup.ShowHelp(j + 1, title, Help.eImage.emptyId);
					}
					if (cells[j].Length > 0 && cells[j][0] != null && cells[j][0].IndexOf("//") != 0) {
						keepRows.Add(j);
					}
				}
				if (keepRows.Count != cells.Length) {
					string[][] cellsCopy = new string[keepRows.Count][];
					for (j = 0; j < keepRows.Count; j++) {
						cellsCopy[j] = cells[keepRows[j]];
					}
					cells = cellsCopy;
				}
				//Clean up null references
				for (j = 0; j < cells.Length; j++) {
					for (k = 0; k < cells[j].Length; k++) {
						if (cells[j][k] == null) {
							cells[j][k] = "";
						}
					}
				}
				//Make available to other methods outside of this scope
				all_Cells.Add(cells);
				all_SheetNames[i] = StringUtilities.GetCamelCase(title);
			}
			if (errorExceededRowsColumns) {
				Copy.copy.help_notAllParsedPro.ShowHelp(columnNames.Length);
			}
			//Start a new loop
			for (int i = 0; i < all_SheetNames.Length; i++) {
				string[][] cells = all_Cells[i];
				if (cells.Length == 0) {
					Copy.copy.help_noContentCells.ShowHelp(Help.eImage.example, all_SheetNames[i]);
					return;
				} else if (cells.Length == 1) {
					Help.ShowHelp("Aborting", "", Help.eImage.noContentRow);
					Copy.copy.help_noContentRows.ShowHelp(Help.eImage.noContentRow, all_SheetNames[i]);
					return;
				} else {
					int maxCols = cells[0].Length;
					int maxRows = cells.Length;
					JSONNode json = JSON.Parse(jsonDocs[i]);
					string updated = json["feed"]["updated"]["$t"].Value;
					string title = all_SheetNames[i];
					//Find class type for each column set
					eType[] typesCols = new eType[maxCols];
					for (j = 0; j < maxCols; j++) {
						string[] colArray = new string[maxRows - 1];
						for (k = 0; k < colArray.Length; k++) {
							colArray[k] = cells[k + 1][j];
						}
						typesCols[j] = GetTypeColumn(title, cells[0][j], colArray);
					}
					//Get Row item Class
					string code_classRow = GetCode_Row(configuration, title, typesCols, cells);
					//Get Sheet item Class
					string code_classSheet = GetCode_Sheet(configuration, i, title, typesCols, cells, updated);
					string template = @"
namespace {0}{
	{1}
	{2}
}";
					classes += StringUtilities.Format(template,
						 GetName_ClassSheetNameSpace(configuration.className),
						 code_classRow,
						 code_classSheet
					);
				}
			}
			classes = GetCode_Main(configuration, all_Cells) + classes;
			string header = @"
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
";
			if (!configuration.doAddReloadCode) {//No Reload Code
				classes = header +"\n" + classes;
			} else {
				header += "using SimpleJSONMetaSheets;\n";
				header += "using System.Linq;\n";
				header += StringUtilities.Format(@"
public class {0} : MonoBehaviour{
	public static void Load(GameObject go, int idx, string label, IEnumerator ieNumerator){
		{0} cmp = go.AddComponent<{0}>();
		cmp.label = label; cmp.idx = idx;
		cmp.StartCoroutine( ieNumerator );
	}
	public string label;
	public int idx;
}", GetName_classLoader(configuration.className));
				classes = header+ "\n" + classes;
			}
			//Add a namespace if the title contains 'namespace{ Document Title }'
			if (configuration.documentTitle.Contains("{") && configuration.documentTitle.Contains("}")) {
				string nameSpace = configuration.documentTitle.Substring(0, configuration.documentTitle.IndexOf("{"));
				classes = "namespace " + StringUtilities.GetPascalCase(nameSpace) + "{\n" + classes.Replace("\n", "\n\t") + "\n}";
			}
			//Unify Line Endings
			classes = Regex.Replace(classes, @"\r\n|\n\r|\n|\r", "\r\n");
			//Save a *.cs file
			StreamWriter writer;
			string path;
			string[] pathExisting = Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + "/Assets/", configuration.className + ".cs", SearchOption.AllDirectories);
			if (pathExisting.Length >= 1) {
				path = pathExisting[0];
			} else {
				path = System.IO.Directory.GetCurrentDirectory() + "/Assets/" + configuration.className + ".cs";
			}
			FileInfo fileInfo = new FileInfo(path);
			if (!fileInfo.Exists) {
				writer = fileInfo.CreateText();
			} else {
				fileInfo.Delete();
				writer = fileInfo.CreateText();
			}
			writer.Write(classes);
			writer.Close();
			if (window != null) {
				window.ShowNotification(new GUIContent(Copy.copy.notification_codeGenerated.Copy(configuration.className)));
			} else {
				Debug.Log(Copy.copy.notification_codeGenerated.Copy(configuration.className));
			}
			AssetDatabase.Refresh();
			if (pathExisting.Length == 0) {
				//Show popup of newly created Class file
				Copy.copy.popup_newDataFile.ShowAlert(configuration.className);
			}
		}
#endregion
#region GENERATE CLASS
		/// <summary>
		/// Gets the main class structure as sourcecode
		/// </summary>
		private static string GetCode_Main(Configuration configuration, List<string[][]> all_Cells) {
			int i;
			string clsInitSheetsData = "";
			string cls = "public class " + configuration.className + "{";
			cls += "\n\t//Document URL: " + configuration.DocumentURL +"\n";
			for (i = 0; i < all_SheetNames.Length; i++) {
				string clsSheetName = GetName_ClassSheet(all_SheetNames[i]);
				cls += "\n\t//Sheet " + clsSheetName;//Comment of the 'sheet name'
				cls += "\n\tpublic static " + GetName_ClassSheetNameSpace(configuration.className) + "." + clsSheetName + " " + GetName_Variable(all_SheetNames[i]) + " = new " + GetName_ClassSheetNameSpace(configuration.className) + "." + clsSheetName + "();";
				clsInitSheetsData += StringUtilities.Format("{0}.Init(); ", GetName_Variable(all_SheetNames[i]) );
			}
			//Init each sheet structure from here to prevent compile errors
			cls += StringUtilities.Format(@"
	static {0}(){
		//Static constructor that initialises each sheet data
		{1}
	}",
				configuration.className, clsInitSheetsData
			);
			if (configuration.doAddReloadCode) {
				cls += "\n\tpublic static void Reload(System.Action onCallLoaded){";
				cls += "\n\t\tint countLoaded = 0;";
				for (i = 0; i < all_SheetNames.Length; i++) {
					cls += "\n\t\t" + GetName_Variable(all_SheetNames[i]) + ".Reload(()=>{countLoaded++;if(countLoaded==" + all_SheetNames.Length + " && onCallLoaded != null){onCallLoaded();}});";
				}
				cls += "\n\t}";
			}
			cls += "\n}\n\n";
			return cls;
		}
		/// <summary>
		/// Gets the row class that is used for one each individual row item to represent data
		/// </summary>
		private static string GetCode_Row(Configuration configuration, string sheetName, eType[] typesCols, string[][] cells) {
			int i;
			string clsRowName = GetName_ClassRow(sheetName);
			string code = "public class " + clsRowName + "{";
			string clsRowExtends = GetName_ClassRowExtends(sheetName);
			if (clsRowExtends != "") {//Extending to a base class, e.g. mySheet:BaseClass'
				code = code.Replace("{", ":" + clsRowExtends + "{");
			}
			//Public Variables
			for (i = 0; i < cells[0].Length; i++) {//For each column in the spreadsheet
				Plugin plugin = plugins[typesCols[i]];
				int j;
				bool doesExtendingVarExist = DoesVariableExist(clsRowExtends, GetName_Variable(cells[0][i])); 
				if (plugin.type == eType._eNum || plugin.type == eType._eNumArray) {
					//Generate Enum Type and Variable
					if (!doesExtendingVarExist || clsRowExtends == "") {//Not extending (if extending, no need to declare)
						string enumName = GetName_eNum(cells[0][i]);
						//Enum with type items
						code += "\n\t\tpublic enum " + enumName + "{";
						//Collect eNum values
						List<string> cellsColumn = new List<string>();
						for (j = 1; j < cells.Length; j++) {
							cellsColumn.Add(cells[j][i]);
						}
						string[] enumValues = GetEnumValues( cellsColumn.ToArray() );
						//Propagate eNum values
						j = 0;
						foreach(string value in enumValues) {
							string enumVarValue = StringUtilities.GetCamelCase(value);
							if (enumVarValue == "NULL") {
								enumVarValue += "=-1";//Replace the value 'NULL' with -1
							}
							code += "\n\t\t\t" + enumVarValue;
							if (j < cells.Length - 1) {
								code += ",";
							}
							j++;
						}
						code += "\n\t\t}";
						if(typesCols[i] == eType._eNum) {
							code += "\n\t\tpublic " + enumName + " " + GetName_Variable(cells[0][i]) + ";";
						} else if(typesCols[i] == eType._eNumArray) {
							code += "\n\t\tpublic " + enumName + "[] " + GetName_Variable(cells[0][i]) + ";";
						}
					}
				} else {
					//Regular Variable (sample value from first row [1][i] (not header [0][i]))
					string variableTypeName = plugin.GetTypeName(cells[1][i], configuration.className, sheetName, cells[0][i]);
					if (!doesExtendingVarExist || clsRowExtends == "") {//Is not extending
						code += "\n\t\tpublic " + variableTypeName + " " + GetName_Variable(cells[0][i]) + ";";
					}
				}
			}
			//Get save variable names
			string[] varNames = new string[cells[0].Length];
			for (i = 0; i < cells[0].Length; i++) {
				varNames[i] = GetName_Variable(cells[0][i], varNames);
			}
			//Constructor
			code += "\n\n\t\tpublic " + clsRowName + "(){}";
			code += "\n\n\t\tpublic " + clsRowName + "(";
			for (i = 0; i < varNames.Length; i++) {
				Plugin plugin = plugins[typesCols[i]];
				code += plugin.GetTypeName(cells[1][i], configuration.className, sheetName, cells[0][i]) + " " + varNames[i];
				if (i < varNames.Length - 1) {
					code += ", ";
				}
			}
			code += "){";
			for (i = 0; i < varNames.Length; i++) {
				code += "\n\t\t\tthis." + varNames[i] + " = " + varNames[i] + ";";
			}
			code += "\n\t\t}";
			code += "\n\t}";
			return code;
		}
		/// <summary>
		/// Gets the class code for one sheet of a spreadsheet
		/// </summary>
		private static string GetCode_Sheet(Configuration configuration, int idxPage, string sheetName, eType[] typesCols, string[][] cells, string updated) {
			int i;
			int j;
			string clsSheetName = GetName_ClassSheet(sheetName);
			string clsRowName = GetName_ClassRow(sheetName);
			string code = "public class " + clsSheetName + ": IEnumerable{";
			//Parse TimeStamp e.g.		2014-04-11T01:59:14.682Z
			string date_A = updated.Split('T')[0];//2014-04-11
			string date_B = updated.Split('T')[1];//01:59:14.682Z
			date_B = date_B.Substring(0, date_B.IndexOf("."));
			int date_year = int.Parse(date_A.Split('-')[0]);
			int date_month = int.Parse(date_A.Split('-')[1]);
			int date_day = int.Parse(date_A.Split('-')[2]);
			int date_hour = int.Parse(date_B.Split(':')[0]);
			int date_minute = int.Parse(date_B.Split(':')[1]);
			int date_sec = int.Parse(date_B.Split(':')[2]);
			code += "\n\t\tpublic System.DateTime updated = new System.DateTime(" + date_year + "," + date_month + "," + date_day + "," + date_hour + "," + date_minute + "," + date_sec + ");";
			for (i = 0; i < cells[0].Length; i++) {
				cells[0][i] = cells[0][i].Replace("\n", "").Replace("\r", "").Trim();
			}
			//A public variable containing all labels
			code += "\n\t\tpublic readonly string[] labels = new string[]{\"" + string.Join("\",\"", cells[0]) + "\"};";
			string codeInitArray = "new " + clsRowName + "[]{";
			for (i = 0; i < cells.Length - 1; i++) {
				string[] columns = cells[i + 1];
				codeInitArray += "\n\t\t\t\t\tnew " + clsRowName + "(";
				for (j = 0; j < columns.Length; j++) {
					if (columns[j] == "#REF!") {
						Copy.copy.help_refError.ShowHelp(Help.eImage.refError, sheetName);
					}
					codeInitArray += GetVariable_Value(configuration.className, columns[j], cells[0][j], sheetName, typesCols[j]);
					if (j < columns.Length - 1) {
						codeInitArray += ",";
					}
				}
				codeInitArray += ")";
				if (i < cells.Length - 2) {
					codeInitArray += ",";
				}
			}
			codeInitArray += "\n\t\t\t\t};";
			//Add each row item
			code += StringUtilities.Format(@"
		private {0}[] _rows = new {0}[{1}];
		public void Init() {
			_rows = {2}
		}
			", clsRowName, cells.Length - 1, codeInitArray);
			string templateEnum = @"
		public IEnumerator GetEnumerator(){
			return new SheetEnumerator(this);
		}
		private class SheetEnumerator : IEnumerator{
			private int idx = -1;
			private {0} t;
			public SheetEnumerator({0} t){
				this.t = t;
			}
			public bool MoveNext(){
				if (idx < t._rows.Length - 1){
					idx++;
					return true;
				}else{
					return false;
				}
			}
			public void Reset(){
				idx = -1;
			}
			public object Current{
				get{
					return t._rows[idx];
				}
			}
		}";
			code += StringUtilities.Format(templateEnum, clsSheetName);
			//Add '.Length' getter
			code += @"
		/// <summary>
		/// Length of rows of this sheet
		/// </summary>
		public int Length{ get{ return _rows.Length; } }";
			//Add '[int]' array indexer
			code += StringUtilities.Format(@"
		/// <summary>
		/// Access row item by index
		/// </summary>
		public {0} this[int index]{
			get{
				return _rows[index];
			}
		}", 
			clsRowName);
			//Add [string] array indexer
			if (typesCols[0] == eType._string) {
				code += StringUtilities.Format(@"
		/// <summary>
		/// Access row item by first culumn string identifier
		/// </summary>
		public {0} this[string id]{
			get{
				for (int i = 0; i < _rows.Length; i++) {
					if( _rows[i].{1} == id){ return _rows[i]; }
				}
				return null;
			}
		}", 
				clsRowName, GetName_Variable(cells[0][0]));
			}
			//Contains key method, e.g. first column of sheet is of type string
			if (typesCols[0] == eType._string) {
				code += StringUtilities.Format(@"
		/// <summary>
		/// Does an item exist with the following key?
		/// </summary>
		public bool ContainsKey(string key){
			for (int i = 0; i < _rows.Length; i++) {
				if( _rows[i].{0} == key){ return true; }
			}
			return false;
		}",
				GetName_Variable(cells[0][0]));
			}
			//Add to Array modifier
			code += StringUtilities.Format(@"
		/// <summary>
		/// List of items
		/// </summary>
		/// <returns>Returns the internal array of items.</returns>
		public {0}[] ToArray(){
			return _rows;
		}", 
			clsRowName);
			//Add 'Random()' method
			code += @"
		/// <summary>
		/// Random item
		/// </summary>
		/// <returns>Returns a random item.</returns>
		public " + clsRowName + @" Random() {
			return _rows[ UnityEngine.Random.Range(0, _rows.Length) ];
		}";
			code += @"
		//Specific Items
";
			//Get unique variable names
			List<string> vars = new List<string>();
			for (i = 0; i < cells.Length - 1; i++) {
				string[] row = cells[i + 1];
				string varName = GetName_Variable(StringUtilities.GetCamelCase(row[0]), vars.ToArray(), "labels", "_rows", "updated", "Length", "ToArray", "HasKey", "Random", "Reload", "Length", "GetEnumerator", "iCaroutineLoad");
                vars.Add(varName);
			}
			//Only generate type strings if the first column is a string
			if (typesCols[0] == eType._string) {
				for (i = 0; i < cells.Length - 1; i++) {
					string varName = vars[i];
					code += "\n\t\tpublic " + clsRowName + " " + varName + "{\tget{ return _rows[" + i.ToString() + "]; } }";
				}
			}
			code += "\n";
			if (configuration.doAddReloadCode) {
				code += GetCode_Reload(configuration, idxPage, sheetName, typesCols, cells);
			}
			code += "\n\t}";
			return code;
		}
		/// <summary>
		/// Generates the optional reload code to debug and tweak in realtime
		/// </summary>
		private static string GetCode_Reload(Configuration configuration, int idxPage, string sheetName, eType[] typesCols, string[][] cells) {
			string cls = "";
			string url = "https://spreadsheets.google.com/feeds/cells/" + configuration.documentKey + "/" + all_SheetGIDs[idxPage] + "/public/basic?alt=json-in-script";
			if (configuration.proxyURL != "") {
				url = configuration.proxyURL + "" + WWW.EscapeURL(url);
			}
			//'Reload(onCallLoaded)' Method
			string template = @"
		/// <summary>
		/// Reload this sheet live from the server
		/// </summary>
		public void Reload(System.Action onCallLoaded){
			GameObject go = new GameObject(""UnityReload"");
			{1}.Load(go, {2}, ""{3}"", iCaroutineLoad(go,""{4}"", onCallLoaded) );
		}";
			cls += StringUtilities.Format(
				template,
				sheetName,
				GetName_classLoader( configuration.className),
				idxPage,
				sheetName,
				url
			);
			//Reload IEnumerator
			template = "\n" + @"#region Reload
		private IEnumerator iCaroutineLoad(GameObject go, string url, System.Action onCallLoaded){
			WWW request = new WWW(url);
			yield return request;
			ParseLoaded(request.text, go, onCallLoaded);
		}
		private void ParseLoaded(string jsonData, GameObject go, System.Action onCallLoaded){
			int i;int j;
			JSONNode entries = JSON.Parse( GetJson(jsonData) )[""feed""][""entry""];
			string[][] cells = new string[ entries.Count ][];
			for (i = 0; i < cells.Length; i++) {
				cells[i] = new string[ entries.Count ];
			}
			for (i = 0; i < entries.Count; i++) {
				string nTitle 	= entries[i][""title""][""$t""].Value;
				string nContent 	= entries[i][""content""][""$t""].Value;
				int[] colRow = GetColumnRow(nTitle);
				cells[ colRow[1] ][ colRow[0] ] = nContent;//Assign value
			}
			//Strip empty rows & commented rows
			List<string[]> keepRows = new List<string[]>();
			for (i = 0; i < cells.Length; i++) {
				if (cells[i].Length > 0 && cells[i][0] != null && string.Join("""",  cells[i] ) != """" && cells[i][0].IndexOf(""//"") != 0){//ALL COMBINED CELLS ARE NOT EMPTY
	                keepRows.Add( cells[i] );
				}
			}
			cells = new string[keepRows.Count][];
			for (i = 0; i < cells.Length; i++) {
				cells[i] = keepRows[i];
			}
			//Strip commented columns
			List<int> keepColumns = new List<int>();
			for (i = 0; i < cells[0].Length; i++) {
				if ( cells[0][i] != null){
					if ( cells[0][i].IndexOf(""//"")!=0){
						keepColumns.Add( i );
					}
				}
			}
			if (keepColumns.Count != cells[0].Length){
				for (i = 0; i < cells.Length; i++) {
					List<string> row = new List<string>();
					for (j = 0; j < keepColumns.Count; j++) {
						row.Add( cells[i][ keepColumns[j] ] );
					}
					cells[i] = row.ToArray();
				}
			}
			if ((cells.Length-1) != _rows.Length) {
				//LOADED DATA AND INITIALLY COMPILED ROWS COUNT DO NOT MATCH 
				Debug.LogWarning(""MeteSheets: Row mismatch, loaded data contains "" + (cells.Length - 1) + "" rows wheras the compiled code expects "" + _rows.Length + "" rows"");
				List<{0}> _rowsList = new List<{0}>(_rows);
				if ((cells.Length - 1) < _rows.Length) {
					//REMOVE ROW ITEMS
					int count = _rows.Length - (cells.Length - 1);
					for (i = 0; i < count; i++) {
						_rowsList.RemoveAt(_rowsList.Count - 1);
					}
				} else if ((cells.Length - 1) > _rows.Length) {
					//ADD NEW ROW ITEMS
					int count = (cells.Length - 1) - _rows.Length;
					for (i = 0; i < count; i++) {
						_rowsList.Add(new {0}());
					}
				}
				_rows = _rowsList.ToArray();
			}
			for (i = 0; i < _rows.Length; i++) {
				//NOW PARSE AND ASSIGN LOADED STRING VALUES
				if(i > cells.Length - 1) {
					continue;
				}
				{1}
			}
			GameObject.Destroy(go);
			if (onCallLoaded!=null){
				onCallLoaded();
			}
		}
		//Converts a string to bool
		private bool ParseBool(string inp) {
			bool result;
			if (bool.TryParse(inp, out result)) {
				return result;
			}
			return false;
		}
		//Converts a string to float
		private float ParseFloat(string inp){
			float result;
			if (float.TryParse (inp, out result)) {
				return result;
			}
			return 0f;
		}
		//Converts a string to int
		private int ParseInt(string inp){
			int result;
			if (int.TryParse (inp, out result)) {
				return result;
			}
			return 0;
		}
			";
			cls += StringUtilities.Format(template,
				GetName_ClassRow(sheetName),
				GetCode_Reload_Parsing(configuration, idxPage, sheetName, typesCols, cells)
			);
			//Get Helper Methods
			cls += @"
		//Supports A-CZ = 104 COLUMNS
		private static string[] columnNames = (""A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK,AL,AM,AN,AO,AP,AQ,AR,AS,AT,AU,AV,AW,AX,AY,AZ,BA,BB,BC,BD,BE,BF,BG,BH,BI,BJ,BK,BL,BM,BN,BO,BP,BQ,BR,BS,BT,BU,BV,BW,BX,BY,BZ,CA,CB,CC,CD,CE,CF,CG,CH,CI,CJ,CK,CL,CM,CN,CO,CP,CQ,CR,CS,CT,CU,CV,CW,CX,CY,CZ"").Split(new char[]{','});
		private int[] GetColumnRow(string id)
		{
			string A = System.Text.RegularExpressions.Regex.Replace(id, @""[\d]"","""");//THE ALHABETICAL PART
			string B = System.Text.RegularExpressions.Regex.Replace(id, @""[^\d]"","""");//THE ROW INDEX
			int idxCol = System.Array.IndexOf<string>( columnNames, A );
			int idxRow = System.Convert.ToInt32( B )-1;//STARTS AT 0
			return new int[2]{idxCol, idxRow};
		}
		private string GetJson(string inp){
			int A = inp.IndexOf(""{"");
			int B = inp.LastIndexOf(""}"");
			if (A != -1 && B != -1){
				return inp.Substring( A, B-A+1);
			}
			return inp;
		}
		private string GetCamelCase(string inp){
			inp = inp.Trim();
			if (inp.Length == 0)
				return """";
			string append;
			string[] words = inp.Split(' ');
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < words.Length; i++) {
				string s = words[i];
				if (s.Length > 0){
					string firstLetter = s.Substring(0, 1);
					string rest = s.Substring(1, s.Length - 1);
					if (i==0) {//DON'T MODIFY FIRST CHARACTER
						append = firstLetter.ToLower() + rest;
					}else{
						append = firstLetter.ToUpper() + rest;
					}
					if(append.Trim().Length > 0) {
						sb.Append(append.Trim());
						sb.Append("" "");
					}
				}
			}	
			return (sb.ToString().Substring(0, sb.ToString().Length - 1)).Replace("" "","""");
		}
		private string CleanString(string inp){
			return CleanString(inp, new string[]{"" "",""\t"",""\n"",""\r""} );
		}
		private string CleanString(string inp, string[] remove){
			return System.Text.RegularExpressions.Regex.Replace( inp, string.Join(""|"", remove), """").Trim();
		}
		";
			cls += PluginColor.formatReloadTryParseColorMethod;
			cls += "\n\t#endregion";
			return cls;
		}
		/// <summary>
		/// Generates the optional reload code to debug and tweak in realtime
		/// </summary>
		private static string GetCode_Reload_Parsing(Configuration configuration, int idxPage, string sheetName, eType[] typesCols, string[][] cells) {
			int j;
			string cls = "";
			int k = 0;
			cls += StringUtilities.Format(@"
				_rows[i] = new {0}.{1}();", GetName_ClassSheetNameSpace(configuration.className), GetName_ClassRow(sheetName));
			cls += StringUtilities.Format(@"
				string[] columns = cells[(i+1)];");
			cls += StringUtilities.Format(@"
				for (j = 0; j < {0}; j++) {
					if (columns[j] == null){
						columns[j] = """";
					}
				}",
				cells[1].Length
			);
			for (j = 0; j < cells[1].Length; j++, k++) {
				Plugin plugin       = plugins[typesCols[j]];
				string attribute	= GetName_Variable(cells[0][j]);
				string variableType = plugin.GetTypeName(cells[0][j], configuration.className,sheetName, cells[0][j]);// GetName_variableType(configuration.className, typesCols[j], "", cells[0][j], sheetName);//E.G. int[]
				string cellString	= "columns[" + j + "]";
				//List type and variable name in Comment
				cls += StringUtilities.Format("\n"+@"				//Variable '{0}' OF TYPE '{1}'",
					attribute, variableType
				);
				cls += plugin.FormatReload(k, cellString, configuration.className, sheetName, cells[0][j]);
			}
			return cls;
		}
#endregion
#region Get Variable Type
		private static Dictionary<eType, Plugin> plugins = new Dictionary<eType, Plugin>() {
			{eType._eNum,       new PluginEnum() },
			{eType._eNumArray,  new PluginEnumArray() },
			{eType._Color,      new PluginColor() },
			{eType._ColorArray,	new PluginColorArray() },
			//Vector2, Vector3 are variants of ',' arrays. Detect before plain arrays
			{eType._Vector2,    new PluginVector2() },
			{eType._Vector3,    new PluginVector3() },
			{eType._dateTime,   new PluginDateTime() },
			{eType._TimeSpan,   new PluginTimeSpan() },
			{eType._Class,      new PluginClass() },
			{eType._ClassArray, new PluginClassArray() },
			{eType._boolArray,  new PluginBoolArray() },
			{eType._intArray,   new PluginIntArray() },
			{eType._floatArray, new PluginFloatArray() },
			{eType._stringArray,new PluginStringArray() },
			//Basic types
			{eType._bool,		new PluginBool() },
			{eType._int,        new PluginInt() },
			{eType._float,      new PluginFloat() },
			{ eType._string,     new PluginString() },
		};
		/// <summary>
		/// Return the variable type, supported types are int, float, bool and string
		/// </summary>
		public static eType GetPluginType(string cell) {
			foreach(KeyValuePair<eType, Plugin> plugin in plugins) {
				if (plugin.Value.IsValid(cell)) {
					return plugin.Key;
				}
			}
			return eType._string;
		}
		/// <summary>
		/// Return the total variable type e.g. the full set of values of one row
		/// </summary>
		public static eType GetTypeColumn(string sheetName, string columnName, string[] cells) {
			//Check if column title enforces a certain type 'type varName'
			string[] columnNameArray = columnName.Split(new char[] { ' ' });
			if (columnNameArray.Length >= 2) {
				string compareColumnType = StringUtilities.GetCamelCase(columnNameArray[0]).ToLower();
				foreach (KeyValuePair<eType, Plugin> pair in plugins) {
					if (compareColumnType == pair.Value.columnKeyWord.ToLower()) {
						return pair.Key;
					}
				}
				//Check if a column enforces a class reference of another tab
				foreach (string titleRaw in all_SheetNames) {
					string title = titleRaw;
					//Strip the part that extends a class
					if (title.Contains(":")) {
						title = title.Split(':')[0];
					}
					//Contains the title in the type
					if (compareColumnType.IndexOf(title.ToLower()) == 0) {
						if (compareColumnType.Contains("[]") && compareColumnType.Length == (title.Length + 2)) {
							//Array Class
							return eType._ClassArray;
						} else if (compareColumnType == title.ToLower()) {
							//Single Class
							return eType._Class;
						}
					}
				}
			}
			//Check if extends to an existing variable in the base class
			if (sheetName != "" && sheetName.Contains(":") && sheetName.Split(':').Length == 2) {
				string className	= GetName_ClassRowExtends(sheetName);
				string varName		= GetName_Variable(columnName);
				if (DoesVariableExist(className, varName)) {
					System.Type type = GetTypeVarBase(className, varName);
					foreach(KeyValuePair<eType, Plugin> plugin in plugins) {
						if (plugin.Value.IsValidType(type)) {
							return plugin.Key;
						}
					}
				}
			}
			//Collect the count of each type
			Dictionary<eType, int> typeCount = new Dictionary<eType, int>();
			int countTypeEmpty= 0;
			foreach (eType type in plugins.Keys) {
				typeCount[type] = 0;
			}
			foreach (string cell in cells) {
				if(cell != "") {
					typeCount[GetPluginType(cell)]++;
				} else {
					countTypeEmpty++;
				}
			}
			List<string> output = new List<string>();
			foreach(KeyValuePair<eType, int> set in typeCount) { 
				if(set.Value > 0) {
					output.Add( set.Key+" = "+set.Value );
				}
			}
			//Check if all of them match, aka a clear match
			int countMaxCount = 0;
			eType countMaxType = eType._string;
			foreach (KeyValuePair<eType, int> set in typeCount) {
				if (set.Value > countMaxCount) {
					countMaxCount = set.Value;
					countMaxType = set.Key;
				}
				if (set.Value == (cells.Length - countTypeEmpty) && set.Value > 0) {//COULD MAP CLEARLY
					if (set.Key == eType._string || set.Key == eType._stringArray) {
						if (PluginEnum.IsEnumArray(cells)) {
							return eType._eNumArray;
						}
					}
					if (set.Key == eType._string) {
						//IF ITS AN OBVIOUS STRING CHECK IF IT COULD BE AN ENUM
						if (PluginEnum.IsEnum(cells)) {
							return eType._eNum;
						}
					}
					return set.Key;
				}
			}
			//Check if all matches are arrays, if so return first array type (Highest priority)
			int countDetected = 0;
			int countDetectedArrays = 0;
			foreach (KeyValuePair<eType, int> set in typeCount) {
				if (set.Value > 0) {
					countDetected++;
					if (plugins[set.Key].isCommaValue) {
						countDetectedArrays++;
					}
				}
			}
			if(countDetected == countDetectedArrays) {
				foreach (KeyValuePair<eType, int> set in typeCount) {
					if (set.Value > 0)
						return set.Key;
				}
			}
			//Check if it is all numbers, thream them as floats
			if (countMaxType == eType._float) {
				if (typeCount[eType._string] == 0 && typeCount[eType._bool] == 0) {
					return eType._float;
				}
			} else if (countMaxType == eType._int) {
				if (typeCount[eType._string] == 0 && typeCount[eType._bool] == 0) {
					if (typeCount[eType._float] > 0) {//ONE OF THEM IS A FLOAT, CONVERT ALL INTEGERS TO FLOATS
						return eType._float;
					} else {
						return eType._int;
					}
				}
			}
			//Return default type
			return eType._string;
		}
		/// <summary>
		/// Converts the value of a cell into its C# value
		/// </summary>
		/// <returns>The variable type_ value.</returns>
		/// <param name="cell">The value from the spreadsheet cell</param>
		/// <param name="type">The type of the value we want it to be casted into</param>
		/// <param name="sheetName">The sheet title</param>
		public static string GetVariable_Value(string className, string cell, string columnName, string sheetName, eType type) {
			foreach(KeyValuePair<eType, Plugin> plugin in plugins) {
				if(type == plugin.Key) {
					return plugin.Value.GetValue(cell, className, sheetName, columnName);
				}
			}
			return null;
		}
#endregion
#region Extended Classes/ Variables
		/// <summary>
		/// Returns true if a variable already exists in a class to extend
		/// </summary>
		public static bool DoesVariableExist(string className, string varName) {
			//className variable can not be empty
			if (className == "" || varName == "") {
				return false;
			}
			System.Reflection.MemberInfo memberInfo = GetMemberInfo(className, varName);
			if (memberInfo != null) {
				return true;
			}
			return false;
		}
		/// <summary>
		/// Get the MemberInfo of a variable from a known class
		/// </summary>
		/// <returns>MemberInfo of the variable</returns>
		public static System.Reflection.MemberInfo GetMemberInfo(string className, string varName) {
			if (className == "" || varName == "") {
				return null;
			}
			System.Type type = GetType(className);
			if (type != null) {
				foreach (System.Reflection.MemberInfo m in type.GetMembers()) {
					if (m.Name == varName) {
						//Found the public variable we are looking for
						return m;
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Get the Type of the className
		/// </summary>
		private static System.Type GetType(string className) {
			if(className == "") {
				return null;
			}
			System.Type type = System.Type.GetType(className);
			if (type == null) {
				// If we still haven't found the proper type, we can enumerate all of the 
				// loaded assemblies and see if any of them define the type
				Assembly currentAssembly = Assembly.GetExecutingAssembly();
				AssemblyName[] referencedAssemblies = currentAssembly.GetReferencedAssemblies();
				foreach (AssemblyName assemblyName in referencedAssemblies) {
					Assembly assembly = Assembly.Load(assemblyName);// Load the referenced assembly
					if (assembly != null) {
						// See if this assembly defines the named type
						type = assembly.GetType(className);
						if (type != null) {
							break;
						}
					}
				}
			}
			return type;
		}
		/// <summary>
		/// Get the Type of a variable in a known class
		/// </summary>
		/// <param name="className"></param>
		/// <param name="varName"></param>
		/// <returns></returns>
		public static System.Type GetTypeExtended(string className, string varName) {
			System.Reflection.MemberInfo member = GetMemberInfo(className, varName);
			if (member != null) {
				switch (member.MemberType) {
					case MemberTypes.Field:
						return ((FieldInfo)member).FieldType;
					case MemberTypes.Property:
						return ((PropertyInfo)member).PropertyType;
					case MemberTypes.Event:
						return ((EventInfo)member).EventHandlerType;
					default:
						return null;
				}
			}
			return null;
		}
		/// <summary>
		/// Return the base-type of a variable in a class
		/// </summary>
		public static System.Type GetTypeVarBase(string className, string varName) {
			System.Type typeVarBase = GetTypeExtended(className, varName);
			if(typeVarBase != null) {
				System.Reflection.FieldInfo fieldInfo = typeVarBase.GetField(varName);
				if (fieldInfo != null) {
					return fieldInfo.FieldType;
				}
			}
			return null;
		}
#endregion
#region Helpers
		/// <summary>
		/// Get a safe eNum name
		/// </summary>
		public static string GetName_eNum(string varName) {
			return StringUtilities.GetCamelCase("e " + GetName_Variable(varName));//ENUM NAME, GetName_Variable will remove var types in front of it
		}
		public static string[] GetEnumValues(params string[] cells) {
			List<string> unique = new List<string>();
			foreach(string cell in cells) {
				string[] array = cell.Split(',');
				foreach(string item in array) {
					string val = item.Trim();
					if (val.Length > 0 && !unique.Contains(val)) {
						unique.Add(val);
					}
				}
			}
			return unique.ToArray();
		}
		/// <summary>
		/// Get a safe row class name (array item of a sheet)
		/// </summary>
		public static string GetName_ClassRow(string title) {
			string n = StringUtilities.GetCamelCase(title);
			//Remove extending class part (if exists)
			if (n.Contains(":")) {
				n = n.Substring(0, n.IndexOf(":"));
			}
			n = n.Substring(0, 1).ToUpper() + "" + n.Substring(1);
			return n;
		}
		/// <summary>
		/// Get a the extending class name for a row class
		/// </summary>
		public static string GetName_ClassRowExtends(string sheetName) {
			string n = StringUtilities.GetCamelCase(sheetName);
			if (n.Contains(":")) {
				return n.Substring(n.IndexOf(":") + 1);
			}
			return "";
		}
		/// <summary>
		/// Get a safe sheet class name
		/// </summary>
		private static string GetName_ClassSheet(string title) {
			string n = StringUtilities.GetCamelCase(title);
			if (n.Contains(":")) {
				n = n.Substring(0, n.IndexOf(":"));
			}
			n = n.Substring(0, 1).ToUpper() + "" + n.Substring(1);
			return "Sheet" + n;
		}
		/// <summary>
		/// Get a safe sheet class namespace name
		/// </summary>
		public static string GetName_ClassSheetNameSpace(string className) {
			return StringUtilities.GetPascalCase(className) + "Types";
		}
		/// <summary>
		/// Get a safe class loader name
		/// </summary>
		private static string GetName_classLoader(string className) {
			string n = className + "_Loader";
			return StringUtilities.GetPascalCase(n);
		}
		public static string GetName_Variable(string inp) {
			//No existing variables names yet
			return GetName_Variable(inp, new string[] { });
		}
		/// <summary>
		/// Rethrieve the variable name of a column label
		/// </summary>
		/// <param name="inp"></param>
		/// <param name="existingNames"></param>
		/// <param name="additionalExistingVars"></param>
		/// <returns></returns>
		private static string GetName_Variable(string inp, string[] existingNames, params string[] additionalExistingVars) {
			int i;
			//Strip new line, return and tab characters
			inp = System.Text.RegularExpressions.Regex.Replace(inp, @"\t|\n|\r", "").Trim();
			if (inp == "") {//Can't be empty
				inp = "unkown";
			}
			//Compare with a lookup table of optional enforced types and strip if found
			string[] columnNameArray = inp.Split(new char[] { ' ' });
			if (columnNameArray.Length >= 2) {
				string compareColumnType = StringUtilities.GetCamelCase(columnNameArray[0]).ToLower();
				foreach (KeyValuePair<eType, Plugin> pair in plugins) {
					if (compareColumnType == pair.Value.columnKeyWord.ToLower()) {
						inp = inp.Substring(inp.IndexOf(columnNameArray[0]) + columnNameArray[0].Length);
						inp = StringUtilities.GetCamelCase(inp);
						break;
					}
				}
				//Is it a sheet Class?
				eType variableType = GetTypeColumn("", inp, new string[] { });
				if (variableType == eType._Class || variableType == eType._ClassArray) {
					inp = columnNameArray[1];
				}
			}
			//Convert input to CamelCase
			inp = StringUtilities.GetCamelCase(inp);
			if (inp.Contains(":")) {
				inp = inp.Substring(0, inp.IndexOf(":"));
			}
			//Can't start with a number
			int parseTry;
			if (int.TryParse(inp.Substring(0, 1), out parseTry)) {
				inp = "_" + inp;
			}
			//Only ASCII characters
			inp = Regex.Replace(inp, @"[^\u0000-\u007F]", string.Empty);
			//Remove illegal characters
			char[] illegalChars = new char[] { '/', '\\', '#', '-', '.', '?', '\'', ',', '<', '>', ' ', '[', ']', '{', '}', '(', ')' };
			for (i = 0; i < illegalChars.Length; i++) {
				inp = inp.Replace(illegalChars[i].ToString(), "");
			}
			//Prevent protected keywords
			//Based on these keywords for C#: http://msdn.microsoft.com/en-us/library/x53a06bb.aspx
			//All these variable names should be avoided
			string[] protectedVars = new string[] { "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while", "add", "alias", "ascending", "async", "await", "descending", "dynamic", "from", "get", "global", "group", "into", "join", "let", "orderby", "partial", "partial", "remove", "select", "value", "var", "where", "yield"};
			//Add additional existing variables to protected vars
			if (additionalExistingVars.Length > 0) {
				string[] protectedVarsCopy = new string[protectedVars.Length + additionalExistingVars.Length];
				protectedVars.CopyTo(protectedVarsCopy, 0);
				additionalExistingVars.CopyTo(protectedVarsCopy, protectedVars.Length);
				protectedVars = protectedVarsCopy;
			}
			//Every illegal variable gets a '_' character pre-pended
			for (i = 0; i < protectedVars.Length; i++) {
				if (inp == protectedVars[i]) {
					inp = "_" + inp;
					break;
				}
			}
			//If it doesn't exist yet in the other array
			if (System.Array.IndexOf<string>(existingNames, inp) == -1) {
				return inp;
			} else {
				//This variable already exists, append a number
				for (i = 0; i < existingNames.Length; i++) {
					int nr = (i + 1);
					string newName = inp + "" + nr.ToString("D2");
					if (System.Array.IndexOf<string>(existingNames, newName) == -1) {
						return newName;
					}
				}
			}
			return inp;
		}
		private static EditorWindow window;
		public static EditorWindow Window {
			get {
				if(window == null) {
					window = (EditorWindow)EditorWindow.GetWindow<MetaSheets>();
				}
				return window;
			}
		}
#endregion
		//Supports A-CZ = 104 Columns
		private static string[] columnNames = ("A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK,AL,AM,AN,AO,AP,AQ,AR,AS,AT,AU,AV,AW,AX,AY,AZ,BA,BB,BC,BD,BE,BF,BG,BH,BI,BJ,BK,BL,BM,BN,BO,BP,BQ,BR,BS,BT,BU,BV,BW,BX,BY,BZ,CA,CB,CC,CD,CE,CF,CG,CH,CI,CJ,CK,CL,CM,CN,CO,CP,CQ,CR,CS,CT,CU,CV,CW,CX,CY,CZ").Split(new char[] { ',' });
		/// <summary>
		/// Get a cell lookup array for the row and column of the cell
		/// </summary>
		/// <param name="id">The id of a cell to interpret, e.g. A1, B1, A2 etc.</param>
		/// <returns>Returns a int[] array with 2 values. [0] stands for the column, [1] stands for the row</returns>
		private static int[] GetColumnRow(string id) {
			string A = Regex.Replace(id, @"[\d]", "");//Alphabetical part
			string B = Regex.Replace(id, @"[^\d]", "");//Row index
			int idxCol = System.Array.IndexOf<string>(columnNames, A);
			int idxRow = System.Convert.ToInt32(B) - 1;//Starts at 0
			return new int[2] { idxCol, idxRow };
		}
	}
}

