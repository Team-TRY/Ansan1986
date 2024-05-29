using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Linq;

namespace InsaneSystems.RoadNavigator
{
	public class Navigator : MonoBehaviour
	{
		const int checkedPointsLimit = 120;
		const int checkedPathsLimit = 75;

		public static NavigatorCrossRoadPoint[] navigatorPoints;
		public static bool isUsingThreading { get; protected set; }

		static readonly Misc.GraphicsPool graphicsPool = new Misc.GraphicsPool();
		
		[SerializeField] RectTransform linesPanel;

		[Header("Testing")]
		[SerializeField] Transform debugTargetPoint;

		public Vector2 targetUIPoint { get; protected set; }

		Map map;
		
		// Points used to check: if path for these points already built, do not rebuild it.
		NavigatorCrossRoadPoint lastNearestPointToCar, lastNearestPointToTarget;

		float timerToNextCheck = 0.5f;
		Path currentPath;
		bool drawLineFromCarToSecondPoint;

		// todo maybe move it to the path graph?
		List<Path> foundPathes = new List<Path>();
		List<int> checkingPathPointsIds = new List<int>();
		int nearestToTargetPointId, pathsChecked;
	
		NavigatorCrossRoadPoint nearestToTargetPoint, nearestToCarPoint;

		readonly List<GameObject> drawnLines = new List<GameObject>();

		bool isThreadFinished;
		Thread thread;

		void Awake()
		{
			map = GetComponent<Map>();

			navigatorPoints = FindObjectsOfType<NavigatorCrossRoadPoint>();

			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				if (map.MapStorage.useThreadingForOptimization)
					Debug.LogWarning("[Road GPS Navigator] Navigator threading optimization disabled due WebGL platform doesn't support it.");

				map.MapStorage.useThreadingForOptimization = false;
			}

			isUsingThreading = map.MapStorage.useThreadingForOptimization;

			graphicsPool.Reset();
		}

		void Start()
		{
			for (int i = 0; i < navigatorPoints.Length; i++)
			{
				var currentPoint = navigatorPoints[i];

				for (int j = 0; j < currentPoint.connectedPoints.Count; j++)
				{
					if (!currentPoint.connectedPoints[j])
						continue;

					var pointsDistance = new PointsDistance();
					pointsDistance.pointAId = GetIndex(navigatorPoints, currentPoint);
					pointsDistance.pointBId = GetIndex(navigatorPoints, currentPoint.connectedPoints[j]);

					if (FindDistanceBetweenPoints(pointsDistance.pointAId, pointsDistance.pointBId) == null)
					{
						pointsDistance.distance = Vector3.Distance(currentPoint.connectedPoints[j].transform.position, currentPoint.transform.position); //(currentPoint.connectedPoints[j].anchoredPosition - currentPoint.anchoredPosition).sqrMagnitude;

						PathsGraph.distancesBetweenPoints.Add(pointsDistance);
					}
				}
			}
		}

		void Update()
		{
			if (isThreadFinished)
				OnCheckCallback();

			// possible will be available in next updates, currently not finished
			//if (currentPath != null)
			//{
				//var pathPoints = currentPath.GetPathPoints();

				// todo remove line previously drawn

				//var point = (drawLineFromCarToSecondPoint && pathPoints.Count > 1) ? pathPoints[1] : pathPoints[0];
				//DrawLine(map.busUIPosition, point.anchoredPosition);
			//}

			if (timerToNextCheck > 0)
			{
				timerToNextCheck -= Time.deltaTime;
				return;
			}

			timerToNextCheck = map.MapStorage.rebuildLineEverySeconds;

			pathsChecked = 0;

			if (debugTargetPoint && targetUIPoint == Vector2.zero)
			{
				SetTargetPoint(debugTargetPoint.position);
				debugTargetPoint = null;
			}

			if (targetUIPoint == Vector2.zero)
			{
				ClearLines();
				return;
			}

			nearestToCarPoint = GetNearestCrossRoadPointToUIPosition(map.carUIRealPosition, map.carUIPosition, navigatorPoints, true);
			nearestToTargetPoint = GetNearestCrossRoadPointToUIPosition(map.targetUIRealPosition, targetUIPoint, navigatorPoints);

			if (isUsingThreading)
			{
				if (lastNearestPointToCar != nearestToCarPoint || lastNearestPointToTarget != nearestToTargetPoint)
				{
					thread = new Thread(DoCheckPath);
					thread.Start();
				}
				else
				{
					DoDrawWork();
				}
			}
			else
			{
				DoCheckPath();
				DoDrawWork();
			}
		}

		void DoCheckPath()
		{
			if (map.MapStorage.useNewAlgorithm)
			{
				DoCheckPathNew();
				return;
			}
			
			int nearestToCarPointId = -1;
			nearestToTargetPointId = -1;

			if (nearestToCarPoint)
				nearestToCarPointId = GetIndex(navigatorPoints, nearestToCarPoint);

			if (nearestToTargetPoint)
				nearestToTargetPointId = GetIndex(navigatorPoints, nearestToTargetPoint);

			checkingPathPointsIds = new List<int>();
			foundPathes = new List<Path>();

			if (nearestToCarPointId != -1 && nearestToTargetPointId != -1 && (lastNearestPointToCar != nearestToCarPoint || lastNearestPointToTarget != nearestToTargetPoint))
				CheckForPath(nearestToCarPointId);
			else if (nearestToCarPointId == -1 || nearestToTargetPointId == -1)
				currentPath = null;

			if (foundPathes.Count > 0)
			{
				if (foundPathes.Count == 1)
				{
					currentPath = foundPathes[0];
				}
				else
				{
					//var nearestPath = foundPathes[0];
					//float pathLength = nearestPath.length;

					foundPathes = foundPathes.OrderBy(path => path.length).ToList();
					var shortestPathes = new List<Path>();
					
					for (int i = 0; i < foundPathes.Count; i++)
					{
						if (i > 2)
							break;

						shortestPathes.Add(foundPathes[i]);
					}

					shortestPathes = shortestPathes.OrderBy(path => path.averageWeight).ToList();
					
					/* Old check: 
					for (int i = 1; i < foundPathes.Count; i++)
					{
						if (foundPathes[i].length < nearestPath.length)
						{
							nearestPath = foundPathes[i];
							pathLength = foundPathes[i].length;
						}
					}
					*/

					currentPath = shortestPathes[0];
				}
			}

			lastNearestPointToCar = nearestToCarPoint;
			lastNearestPointToTarget = nearestToTargetPoint;

			if (isUsingThreading)
				isThreadFinished = true;
		}

		void DoCheckPathNew()
		{
			int nearestToCarPointId = -1;
			nearestToTargetPointId = -1;

			if (nearestToCarPoint)
				nearestToCarPointId = GetIndex(navigatorPoints, nearestToCarPoint);

			if (nearestToTargetPoint)
				nearestToTargetPointId = GetIndex(navigatorPoints, nearestToTargetPoint);

			if (nearestToCarPointId == -1 || nearestToTargetPointId == -1)
			{
				currentPath = null;
				return;
			}

			if (lastNearestPointToCar == nearestToCarPoint && lastNearestPointToTarget == nearestToTargetPoint)
				return;
			
			// add path select by distance
			
			var outerPoints = new List<int>();
			outerPoints.Add(nearestToCarPointId);

			var cameFrom = new Dictionary<int, int>();
			cameFrom.Add(nearestToCarPointId, -1);

			while (outerPoints.Count > 0)
			{
				var currentPointId = outerPoints[0];
				var currentPoint = navigatorPoints[currentPointId];
				
				outerPoints.RemoveAt(0);

				if (currentPointId == nearestToTargetPointId)
				{
					var pointsList = new List<int>();
					pointsList.Add(currentPointId);
					
					while (currentPointId != nearestToCarPointId)
					{
						currentPointId = cameFrom[currentPointId];
						pointsList.Add(currentPointId);
					}

					pointsList.Add(nearestToCarPointId);
					pointsList.Reverse();
					
					currentPath = new Path();
					currentPath.SetupWithPoints(pointsList, map.carUIPosition);
					
					break;
				}

				for (var i = 0; i < currentPoint.connectedPoints.Count; i++)
				{
					var nextPoint = currentPoint.connectedPoints[i];
					var nextPointId = GetIndex(navigatorPoints, nextPoint);

					if (!cameFrom.ContainsKey(nextPointId))
					{
						cameFrom.Add(nextPointId, currentPointId);
						outerPoints.Add(nextPointId);
					}
				}
			}
			
			lastNearestPointToCar = nearestToCarPoint;
			lastNearestPointToTarget = nearestToTargetPoint;

			if (isUsingThreading)
				isThreadFinished = true;
		}
		
		
		void DoDrawWork()
		{
			if (currentPath != null)
			{
				// maybe we need to move it somewhere out of here
				var targetDistanceToCar = Vector2.Distance(map.carUIPosition, targetUIPoint);
				var targetDistanceToLastPoint = Vector2.Distance(nearestToTargetPoint.anchoredPosition, targetUIPoint);

				if (targetDistanceToCar < targetDistanceToLastPoint)
				{
					ClearLines();

					return;
				}

				var pathPoints = currentPath.GetPathPoints();
				if (pathPoints.Count > 0)
				{
					var nextPoint = pathPoints.Count > 1 ? currentPath.GetPathPoints()[1] : nearestToTargetPoint;

					var nextPointRealPosition = isUsingThreading ? nextPoint.realPosition : nextPoint.transform.position;
					float realDistance = Vector3.Distance(nextPointRealPosition, map.carUIRealPosition);

					if (!IsObstaclesBetweenRealUIPositions(map.carUIRealPosition, nextPointRealPosition, realDistance))
					{
						float distanceFromCarToNextPoint = Vector2.Distance(map.carUIPosition, nextPoint.anchoredPosition); // anchored positions can give wrong results
						float distanceFromCurrentToNextPoint = Vector2.Distance(nearestToCarPoint.anchoredPosition, nextPoint.anchoredPosition);

						if (distanceFromCarToNextPoint < distanceFromCurrentToNextPoint)
							drawLineFromCarToSecondPoint = true;
						else
							drawLineFromCarToSecondPoint = false;
					}
					else
					{
						drawLineFromCarToSecondPoint = false;
					}
				}

				RedrawLines(currentPath);
			}
		}

		void OnCheckCallback()
		{
			isThreadFinished = false;
			DoDrawWork();
		}

		void RedrawLines(Path path)
		{
			ClearLines();

			var pathPoints = path.GetPathPoints();

			if (pathPoints.Count == 0)
				return;

			for (int i = 0; i < pathPoints.Count - 1; i++)
			{
				if (drawLineFromCarToSecondPoint && pathPoints.Count > 1 && i == 0)
					continue;

				DrawLine(pathPoints[i], pathPoints[i + 1]);
				DrawCircle(pathPoints[i]);
			}

			DrawCircle(pathPoints[pathPoints.Count - 1]);
			DrawLine(pathPoints[pathPoints.Count - 1].anchoredPosition, targetUIPoint);
			DrawCircle(targetUIPoint);

			// todo remove next code, if will be added redraw every frame for this line
			var point = (drawLineFromCarToSecondPoint && pathPoints.Count > 1) ? pathPoints[1] : pathPoints[0];
			DrawLine(map.carUIPosition, point.anchoredPosition);
		}

		void DrawCircle(NavigatorCrossRoadPoint point) { DrawCircle(point.anchoredPosition); }

		void DrawCircle(Vector2 position)
		{
			var drawnCircle = graphicsPool.Get(map.MapStorage.connectorCircleTemplate, linesPanel);
			//var drawnCircle = Instantiate(map.MapStorage.connectorCircleTemplate, linesPanel);
			drawnCircle.GetComponent<RectTransform>().anchoredPosition = position;
			drawnCircle.transform.SetAsLastSibling();
			drawnCircle.GetComponent<Image>().color = map.MapStorage.linesColor;

			var rectTransform = drawnCircle.GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(map.MapStorage.lineWidthInPx, map.MapStorage.lineWidthInPx);

			drawnLines.Add(drawnCircle);
		}

		void DrawLine(NavigatorCrossRoadPoint pointA, NavigatorCrossRoadPoint pointB) { DrawLine(pointA.anchoredPosition, pointB.anchoredPosition); }

		void DrawLine(Vector2 pointAPos, Vector2 pointBPos)
		{
			var drawnLine = DrawLine(map.MapStorage.lineTemplate, pointAPos, pointBPos, linesPanel);
			drawnLine.transform.SetAsLastSibling();
			drawnLine.GetComponent<Image>().color = map.MapStorage.linesColor;

			var rectTransform = drawnLine.GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, map.MapStorage.lineWidthInPx);

			drawnLines.Add(drawnLine);
		}

		void ClearLines()
		{
			for (int i = 0; i < drawnLines.Count; i++)
				graphicsPool.Return(drawnLines[i]); // Destroy

			drawnLines.Clear();
		}

		/// <summary> Recursive method, searching for shortest path. </summary>
		bool CheckForPath(int currentPointId, int checkedPoints = 0)
		{
			int currentCheckedPoints = checkedPoints + 1;

			if (pathsChecked >= checkedPathsLimit || currentCheckedPoints > checkedPointsLimit) // will break the recursion, when path will be found or limit of points is reached.
				return false;

			checkingPathPointsIds.Add(currentPointId);

			var currentPathLength = Path.CalculateLength(checkingPathPointsIds, map.carUIPosition);

			if (currentPointId == nearestToTargetPointId)
				pathsChecked++;
			
			for (int i = 0; i < foundPathes.Count; i++)
			{
				if (foundPathes[i].length < currentPathLength)
				{
					checkingPathPointsIds.Remove(currentPointId); // path is longer then others, so do not use it.
					return false;
				}
			}

			if (currentPointId == nearestToTargetPointId)
			{
				var path = new Path();
				path.SetupWithPoints(checkingPathPointsIds, map.carUIPosition);
	
				foundPathes.Add(path);

				checkingPathPointsIds.Remove(currentPointId);
				return true;
			}

			// searching for nearest point to car
			var currentPoint = navigatorPoints[currentPointId];
			var pointsWithDistances = new List<KeyValuePair<float, NavigatorCrossRoadPoint>>();
			for (int i = 0; i < currentPoint.connectedPoints.Count; i++)
			{
				var connectedPoint = currentPoint.connectedPoints[i];
				var connectedPointId = GetIndex(navigatorPoints, connectedPoint);
				
				if (checkingPathPointsIds.Count > 0 && (checkingPathPointsIds.Contains(connectedPointId) && connectedPointId != nearestToTargetPointId)) // real navigator never builds path through points, which already in path, so we skipping this variations here.
				{
					//Debug.DrawLine(currentPoint.transform.position, connectedPoint.transform.position, Color.blue, 0.5f);
					continue;
				}

				var distance = (nearestToTargetPoint.anchoredPosition - connectedPoint.anchoredPosition).sqrMagnitude; // возможны неточности в поиске (наверное), тогда можно попробовать считать ещё дистанцию от currentPoint до connectedPoint и складывать две дистанции - это даст гарантированный результат, что путь кратчайший.
				var pair = new KeyValuePair<float, NavigatorCrossRoadPoint>(distance, connectedPoint);
				pointsWithDistances.Add(pair);
			}

			pointsWithDistances.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

			for (int i = 0; i < pointsWithDistances.Count; i++)
			{
				var connectedPoint = pointsWithDistances[i].Value;
				var connectedPointId = GetIndex(navigatorPoints, connectedPoint);

				CheckForPath(connectedPointId, currentCheckedPoints);

				//Debug.DrawLine(currentPoint.transform.position, connectedPoint.transform.position, Color.Lerp(Color.red, Color.green, currentCheckedPoints / 20f), 0.5f);
				//Debug.Break();
			}

			checkingPathPointsIds.Remove(currentPointId);
			return false;
		}

		static float GetAngleBetweenPoints(Vector2 pointA, Vector2 pointB)
		{
			var vectorBetween = pointB - pointA;
			var angleInRads = Mathf.Atan2(vectorBetween.y, vectorBetween.x);

			return angleInRads * Mathf.Rad2Deg;
		}

		public static GameObject DrawLine(GameObject lineTemplate, Vector2 startPoint, Vector2 endPoint, RectTransform parentPanel)
		{
			var positionBetweenPoints = Vector2.Lerp(startPoint, endPoint, 0.5f);
			var distanceBetweenPoints = Vector2.Distance(startPoint, endPoint);
			var angleBetweenPoints = GetAngleBetweenPoints(startPoint, endPoint);

			var mapLineObject = graphicsPool.Get(lineTemplate, parentPanel); //Instantiate(lineTemplate, parentPanel);
			var mapLineTransform = mapLineObject.GetComponent<RectTransform>();

			mapLineTransform.sizeDelta = new Vector2(distanceBetweenPoints, 4);
			mapLineTransform.anchoredPosition = positionBetweenPoints;
			mapLineTransform.localEulerAngles = new Vector3(0, 0, angleBetweenPoints);
			//mapLineTransform.Rotate(0, 0, angleBetweenPoints);
			mapLineTransform.SetAsFirstSibling(); 

			return mapLineObject;
		}

		public static NavigatorCrossRoadPoint GetNearestCrossRoadPointToUIPosition(Vector3 pointRealPosition, Vector2 uiPosition, NavigatorCrossRoadPoint[] points, bool isNeedObstaclesCheck = false)
		{
			if (points.Length == 0)
				return null;

			NavigatorCrossRoadPoint nearestPoint = null;
			float nearestDistance = float.MaxValue - 1;

			for (int i = 0; i < points.Length; i++) 
			{
				float currentDistance = (uiPosition - points[i].anchoredPosition).sqrMagnitude;

				if (isNeedObstaclesCheck)
				{
					var currentPointRealPosition = isUsingThreading ? points[i].realPosition : points[i].transform.position;
					var realDistance = Vector3.Distance(currentPointRealPosition, pointRealPosition); 

					if (IsObstaclesBetweenRealUIPositions(pointRealPosition, currentPointRealPosition, realDistance))
						continue;

					//Debug.DrawLine(pointRealPosition, pointsList[i].transform.position, Color.green, 0.25f);
				}

				if (currentDistance < nearestDistance)
				{
					nearestDistance = currentDistance;
					nearestPoint = points[i];
				}
			}

			return nearestPoint;
		}

		static bool IsObstaclesBetweenRealUIPositions(Vector3 posA, Vector3 posB, float rayLength)
		{
			var raycast = Physics2D.Raycast(posA, posB - posA, rayLength);

			return raycast.collider != null && raycast.collider.GetComponent<Obstacle>();
		}

		/// <summary> Allows to change navigation target point from code. Pass here target position in real world coordinates. </summary>
		public void SetTargetPoint(Vector3 worldPoint)
		{
			if (!map)
				map = GetComponent<Map>();

			targetUIPoint = map.WorldToMinimapPosition(worldPoint);
		}

		/// <summary> Use it if you want to disable navigation, and use map just as map. Enable navigation again is possible using SetTargetPoint method with needed target. </summary>
		public void StopNavigation()
		{
			targetUIPoint = Vector2.zero;
			ClearLines();
		}

		public static PointsDistance FindDistanceBetweenPoints(NavigatorCrossRoadPoint pointA, NavigatorCrossRoadPoint pointB)
		{
			int pointAId = GetIndex(navigatorPoints, pointA);
			int pointBId = GetIndex(navigatorPoints, pointB);

			return FindDistanceBetweenPoints(pointAId, pointBId);
		}

		public static PointsDistance FindDistanceBetweenPoints(int pointAId, int pointBId)
		{
			return PathsGraph.distancesBetweenPoints.Find(searchPointsDistance => searchPointsDistance.IsSimilar(pointAId, pointBId));
		}

		// Faster than IndexOf
		static int GetIndex(IList<NavigatorCrossRoadPoint> list, NavigatorCrossRoadPoint value)
		{
			for (int index = 0; index < list.Count; index++)
			{
				if (list[index] == value)
					return index;
			}

			return -1;
		}

		static int GetIndex(NavigatorCrossRoadPoint[] array, NavigatorCrossRoadPoint value)
		{
			for (int index = 0; index < array.Length; index++)
			{
				if (array[index] == value)
					return index;
			}

			return -1;
		}
	}
}