namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using System.Text.RegularExpressions;
	public class UserInterface {
		/// <summary>
		/// Initialise Editor Window, assign Title and icon
		/// </summary>
		public static void InitWindow(EditorWindow window) {
#if UNITY_5_0 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_0_1 || UNITY_4_0
			MetaSheets.window.titleContent = "MetaSheets";
#else
			window.titleContent = new GUIContent("MetaSheets", MetaSheetsImages.IconDocument);//Unity 5.1.x REQUIRED
#endif
			window.minSize = new Vector2(240, 224);
		}
#region GUI
		public static bool OnGUI_EditorWindow(EditorWindow window, Configuration configuration) {
			OnGUI_Header();
			if (!Help.isEnabled) {
				OnGUI_DropdownSettings(configuration);
				if (OnGUI_DocumentKey(window, configuration))
					return true;
				if (OnGUI_BuildSettings(window, configuration))
					return true;
				OnGUI_Warnings(configuration);
			} else {
				Help.OnGUI();
			}
			return false;
		}
		public static bool OnGUI_ScriptableObject(EditorWindow window, Configuration configuration) {
			OnGUI_Header();
			if (!Help.isEnabled) {
				if (OnGUI_DocumentKey(window, configuration))
					return true;
				if (OnGUI_BuildSettings(window, configuration))
					return true;
				OnGUI_Warnings(configuration);
			} else {
				Help.OnGUI();
			}
			return false;
		}
#endregion
#region GUI Modules
		private static GUIContent[] contentTabs = new GUIContent[]{new GUIContent(" Settings", MetaSheetsImages.IconTabSettings), new GUIContent(" Help", MetaSheetsImages.IconTabHelp) };
		private static void OnGUI_Header() {
			if (MetaSheetsImages.Logo && MetaSheetsImages.White) {
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.MinHeight(MetaSheetsImages.Logo.height));
				Rect rect = GUILayoutUtility.GetLastRect();
				GUI.DrawTexture(rect, MetaSheetsImages.White);
				GUI.DrawTexture(new Rect(rect.x, rect.y, MetaSheetsImages.Logo.width, rect.height), MetaSheetsImages.Logo);
				GUIStyle style          = new GUIStyle(GUI.skin.label);
				style.fontSize			= 10;
				style.normal.textColor	= Color.Lerp(Color.black, Color.white, 0.55f);
				style.alignment			= TextAnchor.UpperRight;
				style.active			= style.normal;
				GUI.Label(rect, "PRO " + MetaSheets.VERSION, style);
			}
			bool isHelpVisible = GUILayout.Toolbar(Help.isEnabled ? 1 : 0, contentTabs) == 1;
			if(isHelpVisible != Help.isEnabled) {
				Help.isEnabled = isHelpVisible;
				if (Help.isEnabled) {
					Help.ShowOverview();
				}
			}
		}
		private static bool OnGUI_DocumentKey(EditorWindow window, Configuration configuration) {
			string key = EditorGUILayout.TextField("Google Document Key", configuration.documentKey);
			EditorGUILayout.LabelField("Document Title", configuration.documentTitle.Length == 0 ? "?" : configuration.documentTitle, EditorStyles.boldLabel);
			EditorGUILayout.Space();
			//Extract document key from browser URL
			if (GetDocumentKey(ref key)) {
				if(window != null) {
					window.ShowNotification(new GUIContent(Copy.copy.notification_keyExtracted));
				}
				configuration.documentKey = key;
				configuration.documentTitle = "";
				return true;
			}
			if(key != configuration.documentKey) {
				configuration.documentKey = key;
				return true;
			}
			if (configuration.documentKey.Length > 0) {
				if (UiButton("Open Sheet", Copy.copy.tooltip_openSheet, MetaSheetsImages.IconOpen)) {
					string url = "https://docs.google.com/spreadsheet/ccc?key=" + configuration.documentKey + "&usp=drive_web#gid=0";
					Application.OpenURL(url);
				}
			} else {
				if (UiButton("Create New Sheet", Copy.copy.tooltip_createSheet, MetaSheetsImages.IconCreate)) {
					string url = "https://docs.google.com/spreadsheet/";
					Application.OpenURL(url);
				}
			}
			return false;
		}
		private static void OnGUI_DropdownSettings(Configuration configuration) {
			GUI.enabled = Settings.All.Length > 0;
			string[] classNames = new string[Settings.All.Length];
			List<GUIContent> labels = new List<GUIContent>();
			for (int i = 0; i < Settings.All.Length; i++) {
				labels.Add(new GUIContent(Settings.All[i].configuration.className + ".cs\t" + Settings.All[i].configuration.documentTitle));
				classNames[i] = Settings.All[i].configuration.className;
			}
			int indexCurrent    = System.Array.IndexOf<string>( classNames, MetaSheets.configuration.className);
			int indexSelected   = EditorGUILayout.Popup(new GUIContent("Settings",Copy.copy.tooltip_settings.Copy(Settings.All.Length) ), indexCurrent, labels.ToArray());
			if (Settings.All.Length > 0 && indexSelected != indexCurrent) {
				MetaSheets.configuration.Apply(Settings.All[indexSelected].configuration);
				Configuration.Cache = Settings.All[indexSelected].configuration;
			}
			GUILayout.Space(8);
			GUI.enabled = true;
		}
		private static bool OnGUI_BuildSettings(EditorWindow window, Configuration configuration) {
			EditorGUILayout.Space();
			GUILayout.BeginHorizontal();
			string className = EditorGUILayout.TextField(new GUIContent("Class Name", MetaSheetsImages.IconClass), configuration.className, GUILayout.ExpandWidth(true));
			if(className != configuration.className) {
				configuration.className = className;
				return true;
			}
			GUILayout.Label(".cs", GUILayout.ExpandWidth(true));
			GUILayout.EndHorizontal();
			//Reload Code Options
			bool doAddReloadCode = EditorGUILayout.Toggle(new GUIContent("Add reload code", Copy.copy.tooltip_addReloadCode.Copy(configuration.className)), configuration.doAddReloadCode);
			if(doAddReloadCode != configuration.doAddReloadCode) {
				configuration.doAddReloadCode = doAddReloadCode;
				return true;
			}
			EditorGUILayout.BeginHorizontal();
			if (doAddReloadCode) {
				string proxyURL = EditorGUILayout.TextField(new GUIContent("Proxy URL", Copy.copy.tooltip_proxyURL), configuration.proxyURL);
				if(proxyURL != configuration.proxyURL) {
					configuration.proxyURL = proxyURL;
					return true;
				}
				GUI.enabled = proxyURL.Length > 0 && configuration.documentKey != "";
				if (GUILayout.Button("Test", GUILayout.ExpandWidth(false))) {
					Application.OpenURL(proxyURL + "" + WWW.EscapeURL(configuration.DocumentURL));
				}
				GUI.enabled = true;
			}
			EditorGUILayout.EndHorizontal();
			//Don't allow to build when Unity is playing in the Editor
			if (Application.isPlaying) {
				GUI.enabled = false;
			}
			//Class name has to be defined
			if(configuration.className.Length == 0) {
				GUI.enabled = false;
			}
			//Document key has to be defined
			if (configuration.documentKey.Length == 0) {
				GUI.enabled = false;
			}
			if (UiButton("Generate \"" + configuration.className + ".cs\"", Copy.copy.tooltip_generateCode.Copy(configuration.className), MetaSheetsImages.IconGenerate)) {
				//Save Settings
				MetaSheets.configuration.Apply(configuration);
				MetaSheets.Save();
				MetaSheets.Build(configuration, window);
			}
			GUI.enabled = true;
			return false;
		}
		private static void OnGUI_Warnings(Configuration configuration) {
			//Check for www sandbox builds / environments
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL) {
				if (configuration.proxyURL == "") {
					EditorGUILayout.HelpBox(Copy.copy.hint_webTargetNoProxyURL, MessageType.Warning);
				}
			}
			//Check if proxy contains "http"
			if (configuration.proxyURL.Length > 0 && configuration.proxyURL.IndexOf("http") == -1 && configuration.doAddReloadCode) {
				EditorGUILayout.HelpBox(Copy.copy.hint_proxyUrlIncorrect, MessageType.Warning);
			}
			//Document key
			if (configuration.documentKey.Length == 0) {
				EditorGUILayout.HelpBox(Copy.copy.hint_specifyDocumentKey, MessageType.Warning);
			}
			string correctedClassName;
			if (configuration.className.Length == 0) {
				//Check for class name
				EditorGUILayout.HelpBox(Copy.copy.hint_specifyClassName, MessageType.Warning);
			}else if (IsBadClassName(configuration.className, out correctedClassName)) {
				//Bad class name
				EditorGUILayout.HelpBox(StringUtilities.Format(Copy.copy.hint_badClassName, correctedClassName), MessageType.Warning);
			}
			GUI.enabled = true;
		}
		private static bool IsBadClassName(string input, out string output) {
			string s = input;
			//Only a-zA-Z
			Regex regex = new Regex(@"[^a-zA-Z\s]", (RegexOptions)0);
			s = regex.Replace(s, "");
			//Pascal Case
			s = StringUtilities.GetPascalCase(s);
			if(s.Length == 0) {
				output = "Data";
			} else {
				output = s;
			}
			return output != input;
		}
		#endregion
		#region Helpers
		private static bool UiButton(string label, string context, Texture icon = null) {
			int margin = 3;
			bool state;
			if (icon) {
				state = GUILayout.Button(new GUIContent(label, context), GUILayout.MinHeight(icon.height + 2 * margin));
			} else {
				state = GUILayout.Button(new GUIContent(label, context));
			}
			if (icon) {
				Rect rect = GUILayoutUtility.GetLastRect();
				rect.y += (rect.height - icon.height) / 2f;
				rect.x += (rect.height - icon.height) / 2f;
				rect.width = icon.width;
				rect.height = icon.height;
				GUI.DrawTexture(rect, icon);
			}
			return state;
		}
		private static bool GetDocumentKey(ref string documentKey) {
			if (documentKey.Contains("https://docs.google.com")) {
				//Old URL scheme in gDrive
				if (documentKey.Contains("?key=")) {
					string key = "?key=";
					int A = documentKey.IndexOf(key);
					int B = A;
					if (documentKey.Contains("&")) {
						B = documentKey.IndexOf("&", A + key.Length);
					} else if (documentKey.Contains("#")) {
						B = documentKey.IndexOf("#", A + key.Length);
					}
					if (B > A) {
						documentKey = documentKey.Substring(A + key.Length, B - A - key.Length);
						return true;
					}
				} else if (documentKey.Contains("spreadsheets/d/")) {
					//New url scheme in gDrive
					string key = "spreadsheets/d/";
					int A = documentKey.IndexOf(key);
					int B = documentKey.IndexOf("/", A + key.Length);
					documentKey = documentKey.Substring(A + key.Length, B - A - key.Length);
					return true;
				}
			}
			return false;
		}
		#endregion
	}
}
