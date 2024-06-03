using UnityEditor;

namespace InsaneSystems.RoadNavigator
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(NavigatorCrossRoadPoint))]
	public class NavigatorCrossRoadPointEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var targetPoint = target as NavigatorCrossRoadPoint;

			var weightProperty = serializedObject.FindProperty("weight");
			var connectedPointsProperty = serializedObject.FindProperty("connectedPoints");

			EditorGUILayout.PropertyField(weightProperty, true);

			EditorGUILayout.HelpBox("All added to this list points automatically will add this point to its own list, so you'll be able not to connect them again.", MessageType.Info);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(connectedPointsProperty, true);

			serializedObject.ApplyModifiedProperties();
			serializedObject.Update();

			if (EditorGUI.EndChangeCheck())
			{
				for (int i = 0; i < targetPoint.connectedPoints.Count; i++)
				{
					var connectedPoint = targetPoint.connectedPoints[i];

					if (!connectedPoint.connectedPoints.Contains(targetPoint))
						connectedPoint.connectedPoints.Add(targetPoint);
				}
			}
		}
	}
}