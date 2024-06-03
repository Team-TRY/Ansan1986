using UnityEngine;

namespace InsaneSystems.RoadNavigator
{
	public class ObjectToDrawOnMap : MonoBehaviour
	{
		[Tooltip("This Id can be used to remove this icon from map by code. To do it, use Map.sceneInstance.RemoveObjectFromMap(name).")]
		[SerializeField] string mapId = "IconName";
		[SerializeField] Sprite icon;

		Map minimap;

		bool isShown;

		void Start()
		{
			minimap = Map.sceneInstance;

			ShowOnMap();
		}

		public void HideFromMap()
		{
			if (!isShown || !minimap)
				return;

			minimap.RemoveObjectFromMap(mapId);
			isShown = false;
		}

		public void ShowOnMap()
		{
			if (isShown || !minimap)
				return;

			minimap.AddObjectToMap(icon, transform.position, mapId);
			isShown = true;
		}
	}
}