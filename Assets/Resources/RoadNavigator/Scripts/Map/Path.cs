using System.Collections.Generic;
using UnityEngine;

namespace InsaneSystems.RoadNavigator
{
	public class Path
	{
		public List<int> pathPointsIds { get; protected set; }
		public float length { get; protected set; }
		public float averageWeight { get; protected set; }

		public Path()
		{
			pathPointsIds = new List<int>();
		}

		public void SetupWithPoints(List<int> pathPointsIds, Vector2 carUIPosition)
		{
			this.pathPointsIds = new List<int>(pathPointsIds);
			CalculateLength(carUIPosition);
			averageWeight = GetAverageWeight();
		}

		public float CalculateLength(Vector2 carUIPosition)
		{
			length = CalculateLength(pathPointsIds, carUIPosition);

			return length;
		}

		public List<NavigatorCrossRoadPoint> GetPathPoints()
		{
			var list = new List<NavigatorCrossRoadPoint>();

			for (int i = 0; i < pathPointsIds.Count; i++)
			{
				int index = pathPointsIds[i];
				list.Add(Navigator.navigatorPoints[index]);
			}

			return list;
		}

		public static float CalculateLength(List<int> pathPointsIds, Vector2 carUIPosition)
		{
			float length = 0;

			if (pathPointsIds.Count > 1)
			{
				for (int i = 1; i < pathPointsIds.Count; i++)
				{
					var foundDistance = Navigator.FindDistanceBetweenPoints(pathPointsIds[i - 1], pathPointsIds[i]);

					if (foundDistance == null)
					{
						length += 1000; // todo road navigator - this is not very correct, but prevents some nulls (which should not exist in any case, so we need to check point distances generator or smth else, for example, wave algo).
						continue;
					}
					
					length += foundDistance.distance;
				}

				// todo check, is it needed for correct calculations, or maybe not?
				// length += (Navigator.navigatorPoints[pathPointsIds[0]].anchoredPosition - carUIPosition).magnitude; // better to change for distance between real icons using for example Vector3.Distance.
			}

			return length;
		}

		float GetAverageWeight()
		{
			float avgWeight = 0f;

			for (int i = 0; i < pathPointsIds.Count; i++)
				avgWeight += Navigator.navigatorPoints[pathPointsIds[i]].Weight;

			avgWeight /= pathPointsIds.Count;

			return avgWeight;
		}
	}
}