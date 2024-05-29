using UnityEngine;
using UnityEngine.Serialization;

namespace InsaneSystems.RoadNavigator
{
	[CreateAssetMenu(fileName = "Storage", menuName = "Insane Systems/Road GPS Navigator/Storage")]
	public class Storage : ScriptableObject
	{
		[Header("Map Settings")]
		[Range(0.25f, 4f)] public float mapIconsScaling = 1f;
		[Range(0.25f, 4f)] public float navigatorIconsScaling = 1f;
		public Color linesColor = new Color(1f, 0.75f, 0f, 1f);
		[Range(1, 48)] public int lineWidthInPx = 16;
		[FormerlySerializedAs("busIconAngleOffset")] [Tooltip("If your icon sprite rotated wrong, you can edit this value. Try to set 90 or 180 to fix problem.")]
		public float playerIconAngleOffset = 0;
		public bool allowDragMapByMouse = true;
		[Range(0.25f, 4f)] public float mapZoomSpeed = 1f;
		public Sprite playerIcon;
		public Sprite targetIcon;

		[Header("Navigator settings")]
		[Tooltip("This value means how will be navigator scaled regarding to map scale.")]
		[Range(0.5f, 6f)] public float navigatorZoom = 1.5f;
		[Range(0.25f, 3f)] public float rebuildLineEverySeconds = 0.5f;
		public bool useThreadingForOptimization = true;
		[Tooltip("PREVIEW VERSION! Use it on your own risk. Enables wave search algorithm. It can work faster, but in preview version it can work not fully correct.")]
		public bool useNewAlgorithm;
		
		[Header("Input settings")]
		public KeyCode showMapKey = KeyCode.M;
		public KeyCode showNavigatorKey = KeyCode.N;

		[Header("UI Templates")]
		public GameObject iconTemplate;
		public GameObject lineTemplate;
		public GameObject connectorCircleTemplate;
		
	}
}