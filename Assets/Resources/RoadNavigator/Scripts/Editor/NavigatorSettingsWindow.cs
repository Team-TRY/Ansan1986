using InsaneSystems.RoadNavigator;
using UnityEngine;
using UnityEditor;

namespace InsaneSystems.RoadGPSNavigator
{
    public class NavigatorSettingsWindow : EditorWindow
    {
        Storage storage;
        Editor storageEditor;
        
        [MenuItem("Window/Insane Systems/Road GPS Navigator/Settings", false, 100)]
        static void Init()
        {
            var window = (NavigatorSettingsWindow)EditorWindow.GetWindow(typeof(NavigatorSettingsWindow));
            window.minSize = new Vector2(368, 368);
            window.maxSize = new Vector2(1024, 1024);
            window.titleContent = new GUIContent("Navigator Settings");
            window.Show();
        }

        void OnGUI()
        {
            if (!storage)
            {
                var foundAssets = AssetDatabase.FindAssets("t:Storage");

                // there can be other Storages types, so we need to check it correctly
                foreach (var guid in foundAssets)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);

                    var loadedStorageAsset = AssetDatabase.LoadAssetAtPath<Storage>(path);

                    if (ReferenceEquals(loadedStorageAsset, null))
                        continue;

                    storage = loadedStorageAsset;
                    break;
                }

                EditorGUILayout.HelpBox("No Storage file with settings found. Try again or create this file.",
                    MessageType.Warning);
                
                return;
            }

            if (!storageEditor && storage)
            {
                storageEditor = Editor.CreateEditor(storage);
                return;
            }

            if (storageEditor)
                storageEditor.OnInspectorGUI();
            else
                EditorGUILayout.HelpBox("Problem with loading editor. Please, try again or edit Storage file manually.",
                    MessageType.Warning);

        }
    }
}