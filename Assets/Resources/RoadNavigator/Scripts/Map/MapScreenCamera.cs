using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace InsaneSystems.RoadNavigator
{
	[ExecuteInEditMode]
	public class MapScreenCamera : MonoBehaviour
	{
		[HideInInspector] public new Camera camera;
		[Tooltip("Default map width and height is based on it size in world units, for example, 128 x 128 meters is 128 x 128 pixels. Resolution Scale is a multiplier for this value. 2x is 256 x 256 pixels, etc. Max allowed size is 8192 x 8192 pixels.")]
		[SerializeField] [Range(1, 8)] int resolutionScale = 4;
		public RenderTexture renderTexture { get; protected set; }
		public Rect outMapRect { get; protected set; }

		Map map;

		void Update()
		{
		}

		public float GetMapAspect()
		{
			return map.RealMapScale.x / map.RealMapScale.y;
		}

		public float GetScreenAspect()
		{
			return Screen.width / (float)Screen.height;
		}

		public void SaveScreenshot()
		{
			DumpRenderTexture(camera, renderTexture, Application.dataPath + "/MapScreen.png");

			Debug.Log("[Road GPS Navigator] Screenshot of the map created! You can find it in Assets directory.");
		}

		public static void DumpRenderTexture(Camera cam, RenderTexture rt, string pngOutPath)
		{
			var oldRT = RenderTexture.active;
			
			cam.Render(); 

			var tex = new Texture2D(rt.width, rt.height);
			RenderTexture.active = rt;
			tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
			tex.Apply();

			File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
			RenderTexture.active = oldRT;
			
			#if UNITY_EDITOR
			AssetDatabase.Refresh();
			#endif
		}

		public void TakeShot()
		{
			if (!camera)
				camera = GetComponent<Camera>();

			if (!map)
				map = FindObjectOfType<Map>();

			if (!map)
			{
				Debug.LogWarning("[Map Screen Camera] No Map component found on scene. Add it first.");
				return;
			}

			if (Application.isPlaying)
			{
				camera.enabled = false;
				enabled = false;
			}

			var mapRect = new Rect();
			mapRect.x = map.GetLevelMapSettings.baseOffset.x;
			mapRect.y = map.GetLevelMapSettings.baseOffset.y;
			mapRect.width = map.RealMapScale.x;
			mapRect.height = map.RealMapScale.y;

			outMapRect = mapRect;

			while ((mapRect.width * resolutionScale > 8192 || mapRect.height * resolutionScale > 8192) && resolutionScale > 1)
				resolutionScale--;

			renderTexture = new RenderTexture((int)(mapRect.width * resolutionScale), (int)(mapRect.height * resolutionScale), 0);
			renderTexture.depth = 16;
			camera.targetTexture = renderTexture;

			var x = mapRect.width / 2f + mapRect.x;
			var z = mapRect.height / 2f + mapRect.y;
			transform.position = new Vector3(x, transform.position.y, z);

			camera.orthographicSize = mapRect.height / 2f; // Mathf.Max(mapRect.width / 2f, mapRect.height / 2f);
		}
	}
}