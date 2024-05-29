using System.Collections.Generic;
using UnityEngine;

namespace InsaneSystems.RoadNavigator
{
	public class NavigatorCrossRoadPoint : MonoBehaviour
	{
		[Tooltip("Min value is 0, max value is 255. Default value is 127 (middle value). Decrease this value, to add priority to this point, increase it to set lower priority (more in guide).")]
		[SerializeField] [Range(1, 255)] int weight = 127;

		[SerializeField]
		public List<NavigatorCrossRoadPoint> connectedPoints = new List<NavigatorCrossRoadPoint>();

		public Vector2 anchoredPosition { get; protected set; }

		/// <summary>Cached transform.position used for threading. Being updated by Minimap only when Navigator.isUsingThreading == true. </summary>
		public Vector3 realPosition { get; protected set; }

		///<summary>Weight of the point.</summary>
		public int Weight { get { return weight; } }

		void Start()
		{
			var rectTransform = GetComponent<RectTransform>();

			if (!rectTransform)
				rectTransform = gameObject.AddComponent<RectTransform>();

			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.zero;
			anchoredPosition = rectTransform.anchoredPosition;

			PointPositionChanged();
		}

		public void PointPositionChanged()
		{
			realPosition = transform.position;
		}

		void OnDrawGizmosSelected()
		{
			var color = Color.yellow;
			color.a = 0.5f;

			Gizmos.color = color;

			for (int i = 0; i < connectedPoints.Count; i++)
			{
				if (!connectedPoints[i])
				{
					Debug.LogWarning("[Road GPS Navigator] Point '" + name + "' connected points " + i + " is empty! fill it or delete from array.");
					continue;
				}

				var conTransform = connectedPoints[i].transform;

				GizmosDrawArrow(transform.position, conTransform.position);
			}
		}

		void GizmosDrawArrow(Vector3 origin, Vector3 destination)
		{
			var direction = destination - origin;
			DrawArrow.ForGizmo(origin, direction, 2f, 20);
		}
	}

	public static class DrawArrow
	{
		public static void ForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
		{
			Gizmos.DrawRay(pos, direction);

			var right = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
			var left = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
			Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
			Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
		}

		public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
		{
			Gizmos.color = color;
			Gizmos.DrawRay(pos, direction);

			var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
			var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
			Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
			Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
		}
	}
}