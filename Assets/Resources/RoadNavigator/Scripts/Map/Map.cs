using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace InsaneSystems.RoadNavigator
{
	public class Map : MonoBehaviour, IDragHandler
	{
		public static Map sceneInstance { get; protected set; }
		const float maxZoomValue = 3f;

		public Storage MapStorage { get { return storage; } }
		public LevelMapSettings GetLevelMapSettings { get { return levelMapSettings; } }
		public Vector2 RealMapScale { get { return levelMapSettings.realMapScale; } }
		public Mask mapMask { get { return maskTransform.GetComponent<Mask>(); } }
		public Vector2 carUIPosition { get; protected set; }
		public Vector3 carUIRealPosition { get; protected set; }
		public Vector2 minimapOffset { get { return mapTransform.anchoredPosition; } }
		public Vector3 targetUIRealPosition { get { return targetIconOnMap.position; } }
		public bool workAsNavigator { get; protected set; }
		
		[SerializeField] LevelMapSettings levelMapSettings;
		[SerializeField] Storage storage;

		[Header("Scene objects")]
		[SerializeField] RectTransform playerIconOnMap;
		[SerializeField] RectTransform targetIconOnMap;
		[SerializeField] GameObject selfObject;
		[SerializeField] RectTransform maskTransform;
		[SerializeField] RectTransform mapTransform;
		[SerializeField] RectTransform mapContainerTransform;
		[SerializeField] Image mapImage;
		[SerializeField] RectTransform navigatorPlaceholder;

		[Header("Additional parameters")]
		[Tooltip("If you have an Image in Map Canvas, which should be background of navigator (and active only in Navigator Mode), put it to this field.")]
		[SerializeField] Image customNavigatorBG;
		[Tooltip("If you have an Image in Map Canvas, which should be foreground (for example, outline) of navigator (and active only in Navigator Mode), put it to this field.")]
		[SerializeField] Image customNavigatorFG;

		Transform navigatorPlayerTransform;

		Navigator navigator;

		float zoomValue = 1f;

		Vector2 mapImageSize;

		float lastPlayerZoom = 1f;

		readonly List<GameObject> iconsAddedToMap = new List<GameObject>();

		void Awake()
		{
			workAsNavigator = false;
			sceneInstance = this;

			navigator = GetComponent<Navigator>();

			mapMask.enabled = true;

			playerIconOnMap.GetComponent<Image>().sprite = storage.playerIcon;
			targetIconOnMap.GetComponent<Image>().sprite = storage.targetIcon;
		}

		void Start()
		{
			SetupMapImage();
			
			if (navigatorPlaceholder)
				navigatorPlaceholder.gameObject.SetActive(false);
			else 
				Debug.LogWarning("[Road GPS Navigator] Please, setup Navigator Placeholder in Map component parameters. Without it you're using legacy method of Navigator UI position algorithm calculation, which will be removed in future.");
			
			selfObject.SetActive(false);
		}

		public void SetupMapImage()
		{
			mapImage.sprite = levelMapSettings.mapSprite;
			mapImage.rectTransform.sizeDelta = new Vector2(mapImage.sprite.texture.width, mapImage.sprite.texture.height);
			mapImageSize = mapImage.GetComponent<RectTransform>().sizeDelta;
			mapContainerTransform.sizeDelta = mapImageSize;
		}

		void Update()
		{
			HandleInput();

			if (!selfObject.activeSelf)
				return;

			if (!navigatorPlayerTransform)
			{
				var foundPlayer = FindObjectOfType<NavigatorPlayer>();
	
				if (foundPlayer)
					navigatorPlayerTransform = foundPlayer.transform;
				else
					return;
			}

			carUIPosition = WorldToMinimapPosition(navigatorPlayerTransform.position);

			playerIconOnMap.anchoredPosition = carUIPosition;
			playerIconOnMap.localEulerAngles = new Vector3(0, 0, -navigatorPlayerTransform.localEulerAngles.y + storage.playerIconAngleOffset);

			targetIconOnMap.anchoredPosition = navigator.targetUIPoint;

			if (workAsNavigator)
			{
				var mapPosition = Vector2.one;
				mapPosition.x *= mapImageSize.x / 2f;
				mapPosition.y *= mapImageSize.y / 2f;

				var pivot = Vector2.one / 2f;
				pivot.x = carUIPosition.x / mapImageSize.x;
				pivot.y = carUIPosition.y / mapImageSize.y;

				mapTransform.anchoredPosition = mapPosition - carUIPosition;
				mapTransform.pivot = pivot;

				mapTransform.localEulerAngles = new Vector3(0, 0, (storage.playerIconAngleOffset * 2) - playerIconOnMap.localEulerAngles.z - storage.playerIconAngleOffset);

				playerIconOnMap.anchoredPosition = mapPosition;
				playerIconOnMap.localEulerAngles = new Vector3(0, 0, storage.playerIconAngleOffset);

				OnPointsPositionChanged();
			}

			carUIRealPosition = playerIconOnMap.position;
		}

		public void OnDrag(PointerEventData pointerEventData)
		{
			if (!workAsNavigator && storage.allowDragMapByMouse)
				MapPositionChange(pointerEventData.delta);
		}

		void MapPositionChange(Vector2 mousePositionDelta)
		{
			var newPosition = mapContainerTransform.anchoredPosition + mousePositionDelta;
			Vector2 halfMapSize = mapImageSize / 2f * mapContainerTransform.localScale.x;

			newPosition.x = Mathf.Clamp(newPosition.x, -halfMapSize.x, halfMapSize.x);
			newPosition.y = Mathf.Clamp(newPosition.y, -halfMapSize.y, halfMapSize.y);

			mapContainerTransform.anchoredPosition = newPosition;

			OnPointsPositionChanged();
		}

		void HandleInput()
		{
			if (Input.GetKeyDown(storage.showMapKey))
				ShowAsMap();
			if (Input.GetKeyDown(storage.showNavigatorKey))
				ShowAsNavigator();

			if (!workAsNavigator && selfObject.activeSelf)
				Zoom();
		}

		void Zoom()
		{
			float zoomAxisValue = Input.GetAxis("Mouse ScrollWheel");

			zoomValue = Mathf.Clamp(zoomValue + zoomAxisValue * Time.deltaTime * 60f * storage.mapZoomSpeed, 1f, maxZoomValue);
			lastPlayerZoom = zoomValue;

			ApplyZoom();

			MapPositionChange(Vector2.zero);
		}

		public Vector2 WorldToMinimapPosition(Vector3 worldPosition)
		{
			if (mapImageSize == Vector2.zero)
				SetupMapImage();

			var uiPosition = new Vector2(worldPosition.x, worldPosition.z);
			uiPosition -= levelMapSettings.baseOffset;

			uiPosition.x = uiPosition.x / levelMapSettings.realMapScale.x * mapImageSize.x; // mapUISize
			uiPosition.y = uiPosition.y / levelMapSettings.realMapScale.y * mapImageSize.y; // mapUISize

			return uiPosition;
		}

		public void ShowAsMap()
		{
			selfObject.SetActive(!selfObject.activeSelf);

			mapContainerTransform.anchoredPosition = Vector2.zero;

			mapTransform.anchoredPosition = Vector2.zero;
			maskTransform.anchoredPosition = Vector2.zero;
			maskTransform.sizeDelta = new Vector2(0, 0);
			maskTransform.pivot = Vector2.one * 0.5f;

			maskTransform.anchorMin = new Vector2(0f, 0f);
			maskTransform.anchorMax = new Vector2(1f, 1f);
			mapTransform.pivot = Vector2.one * 0.5f;

			mapTransform.localEulerAngles = Vector3.zero;

			workAsNavigator = false;

			zoomValue = lastPlayerZoom;
			ApplyZoom();

			if (customNavigatorBG)
				customNavigatorBG.enabled = false;

			if (customNavigatorFG)
				customNavigatorFG.enabled = false;
		}

		public void ShowAsNavigator()
		{
			selfObject.SetActive(!selfObject.activeSelf);

			mapContainerTransform.anchoredPosition = Vector2.zero;

			if (navigatorPlaceholder)
			{
				maskTransform.pivot = navigatorPlaceholder.pivot;
				maskTransform.anchorMin = navigatorPlaceholder.anchorMin;
				maskTransform.anchorMax = navigatorPlaceholder.anchorMax;

				maskTransform.anchoredPosition = navigatorPlaceholder.anchoredPosition; 
				maskTransform.sizeDelta = navigatorPlaceholder.sizeDelta;
			}
			else // todo legacy, will be removed in future versions
			{
				maskTransform.anchoredPosition = new Vector2(Screen.width / 2 - 10, -Screen.height / 2 + 10);
				maskTransform.sizeDelta = new Vector2(300, 200);
				maskTransform.pivot = new Vector2(1f, 0f);
					
				maskTransform.anchorMin = new Vector2(0.5f, 0.5f);
				maskTransform.anchorMax = new Vector2(0.5f, 0.5f);
			}

			workAsNavigator = true;

			zoomValue = storage.navigatorZoom;
			ApplyZoom();

			if (customNavigatorBG)
				customNavigatorBG.enabled = true;

			if (customNavigatorFG)
				customNavigatorFG.enabled = true;
		}

		void ApplyZoom()
		{
			mapContainerTransform.localScale = Vector3.one * zoomValue;

			OnPointsPositionChanged();
		}

		void OnPointsPositionChanged()
		{
			if (!Navigator.isUsingThreading)
				return;

			for (int i = 0; i < Navigator.navigatorPoints.Length; i++)
				Navigator.navigatorPoints[i].PointPositionChanged();
		}

		/// <summary>Allows add custom icon to the map, for example if your game have Gas Station, some mission on map, or something else. </summary>
		/// <param name="icon">Icon image. Use sprite image here.</param>
		/// <param name="worldPosition">Position of target object in real world (not in map coords). You can take it by transform.position of your object.</param>
		/// <param name="iconName">Name of your icon. It needed only if you want to remove your icon from map in future, for example on mission end etc.</param>
		public void AddObjectToMap(Sprite icon, Vector3 worldPosition, string iconName = "Icon")
		{
			var uiPosition = WorldToMinimapPosition(worldPosition);

			var spawnedObject = Instantiate(storage.iconTemplate, mapTransform);
			spawnedObject.name = iconName;
			spawnedObject.GetComponent<RectTransform>().anchoredPosition = uiPosition;
			spawnedObject.GetComponent<Image>().sprite = icon;
			iconsAddedToMap.Add(spawnedObject);
		}

		/// <summary> Allows to remove previously added to map icon by its name. </summary>
		public void RemoveObjectFromMap(string iconName)
		{
			for (int i = 0; i < iconsAddedToMap.Count; i++)
			{
				if (iconsAddedToMap[i] && iconsAddedToMap[i].name == iconName)
				{
					Destroy(iconsAddedToMap[i]);
					iconsAddedToMap.RemoveAt(i);
					break;
				}
			}
		}

		void OnDrawGizmos()
		{
			if (!levelMapSettings)
				return;

			Gizmos.color = Color.red;

			var center2 = Vector2.Lerp(levelMapSettings.baseOffset, levelMapSettings.realMapScale + levelMapSettings.baseOffset, 0.5f);
			var realCenter2 = new Vector3(center2.x, 0, center2.y);
			var size2 = new Vector3(levelMapSettings.realMapScale.x, 0f, levelMapSettings.realMapScale.y);
			Gizmos.DrawWireCube(realCenter2, size2);
		}
	}
}