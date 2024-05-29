using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace InsaneSystems.RoadNavigator
{
	[CustomEditor(typeof(Map))]
	public class MapCustomInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			var map = target as Map;
			
			var levelMapSettingsProperty = serializedObject.FindProperty("levelMapSettings");
			var mapContainerTransformProperty = serializedObject.FindProperty("mapContainerTransform");
			var mapImageProperty = serializedObject.FindProperty("mapImage");
			
			var levelMapSettings = levelMapSettingsProperty.objectReferenceValue as LevelMapSettings;
			var mapContainerTransform = mapContainerTransformProperty.objectReferenceValue as RectTransform;
			var mapTransform = mapImageProperty.objectReferenceValue != null ? (mapImageProperty.objectReferenceValue as Image).rectTransform : null;
			
			if (!levelMapSettings)
				EditorGUILayout.HelpBox("Add level map settings to start work with asset", MessageType.Warning);
			
			DrawDefaultInspector();

			if (!mapContainerTransform || !levelMapSettings || !mapTransform)
			{
				EditorGUILayout.HelpBox("You have some empty fields in component. To continue work, setup them correctly.", MessageType.Warning);

				return;
			}
			
			if (!levelMapSettings.mapSprite)
			{
				EditorGUILayout.HelpBox("In your Map Settings asset Map Sprite field is empty. Setup it to continue.", MessageType.Warning);

				return;
			}
			
			if (!CheckTransformPixelSize(mapContainerTransform, levelMapSettings.mapSprite) || !CheckTransformPixelSize(mapTransform, levelMapSettings.mapSprite))
				EditorGUILayout.HelpBox("Your Map Sprite size does not correspond to current Map size. Possible, you need to use Initial Setup.", MessageType.Warning);

			if (GUILayout.Button("Initial setup"))
				map.SetupMapImage();
		}

		bool CheckTransformPixelSize(RectTransform targetTransform, Sprite sprite)
		{
			var size = targetTransform.sizeDelta;
			var spriteSize = new Vector2(sprite.texture.width, sprite.texture.height);

			return Mathf.Approximately(size.x, spriteSize.x) && Mathf.Approximately(size.y, spriteSize.y);
		}
	}
}