namespace MetaSheets {
	///		Meta Sheets Pro
	///		Copyright © 2017 renderhjs
	///		Version 2.00.0
	///		Website www.metasheets.com
	using UnityEngine;
	using System.Collections;
	using UnityEditor;
	[CustomEditor(typeof(Settings))]
	public class EditorSettings : Editor {
		private Settings Component {
			get {
				return (Settings)target;
			}
		}
		public override void OnInspectorGUI() {
			//Save settings if  one of the values has been changed
			if (UserInterface.OnGUI_ScriptableObject(null, Component.configuration)) {
				EditorUtility.SetDirty(Component);
			}
		}
	}
}

