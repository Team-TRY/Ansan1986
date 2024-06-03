using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace InsaneSystems.RoadNavigator
{
	[CustomEditor(typeof(MapScreenCamera))]
	public class MapScreenCameraEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (Application.isPlaying)
			{
				EditorGUILayout.HelpBox("Work with screen cam is not allowed during PlayMode.", MessageType.Info);
				GUI.enabled = false;
			}

			var mapScreenCamera = target as MapScreenCamera;

			if (mapScreenCamera.outMapRect != null && (mapScreenCamera.outMapRect.width > 512 || mapScreenCamera.outMapRect.height > 512))
				EditorGUILayout.HelpBox("Some values of resolution scale is unaviable due big map size.", MessageType.Info);

			if (GUILayout.Button("Make map snapshot"))
				mapScreenCamera.TakeShot();
			
			GUILayout.Label("Map preview:");
			GUILayout.Label(mapScreenCamera.renderTexture, GUILayout.MaxWidth(250), GUILayout.MaxHeight(250)); 
			
			if (GUILayout.Button("Save map screenshot to Assets"))
				mapScreenCamera.SaveScreenshot();
		}
	}
}