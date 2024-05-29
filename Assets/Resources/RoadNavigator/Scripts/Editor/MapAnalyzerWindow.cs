using System;
using InsaneSystems.RoadNavigator;
using UnityEngine;
using UnityEditor;

namespace InsaneSystems.RoadGPSNavigator
{
    public class MapAnalyzerWindow : EditorWindow
    {
        Map map;
        NavigatorPointsParent pointsParent;
        
        string analyzeResult;

        [MenuItem("Window/Insane Systems/Road GPS Navigator/Map Analyzer")]
        static void Init()
        {
            var window = (MapAnalyzerWindow)EditorWindow.GetWindow(typeof(MapAnalyzerWindow));
            window.minSize = new Vector2(368, 368);
            window.maxSize = new Vector2(1024, 1024);
            window.titleContent = new GUIContent("Map Analyzer");
            window.Show();
        }

        void OnGUI()
        {
            if (!map)
                GUILayout.Label("Find Map object to analyze");
            
            if (GUILayout.Button("Find map"))
            {
                map = FindObjectOfType<Map>();
                pointsParent = FindObjectOfType<NavigatorPointsParent>();
            }

            if (!map || !pointsParent)
            {
                GUILayout.Label("No Map component found or it is broken.");
                GUI.enabled = false;
            }
            else if (map)
            {
                GUILayout.Label("Map object found. Now you can analyze it.");
            }
            
            GUILayout.Space(10);
            if (GUILayout.Button("Analyze"))
                DoAnalyze();

            GUILayout.Label("Analyze result");
            GUILayout.TextArea(analyzeResult);

            GUI.enabled = true;
        }

        void DoAnalyze()
        {
            if (!pointsParent)
                return;

            var parent = pointsParent.transform;
            
            analyzeResult = String.Empty;
            
            var points = parent.GetComponentsInChildren<NavigatorCrossRoadPoint>();
            
            for (var i = 0; i < points.Length; i++)
            {
                var point = points[i];

                for (var w = 0; w < point.connectedPoints.Count; w++)
                {
                    var connectedPoint = point.connectedPoints[w];

                    if (!connectedPoint)
                        analyzeResult += "Point '" + point.name + "' has missing field for one of connected points.\n";
                    else if (connectedPoint == point)
                        analyzeResult += "Point '" + point.name +
                                         "' references to itself in its connections. Remove this connection. \n";
                }

                if (point.connectedPoints.Count == 0)
                    analyzeResult += "Point '" + point.name + "' has no connections. Remove it or add connections.\n";
                else if (point.connectedPoints.Count >= 6)
                    analyzeResult += "Point '" + point.name + "' has more than 5 connections. It isn't correct, 5 is enough for any crossroad.\n";
            }

            if (analyzeResult == String.Empty)
                analyzeResult += "Looks like all settings are correct.";
        }
    }
}