using UnityEngine;

namespace InsaneSystems.RoadNavigator
{
	public class DemoHelper : MonoBehaviour
	{
		[SerializeField] Transform[] targets;
		int enabledTarget = 0;

		Navigator navigator;
		Map map;

		bool isInitialized = false;

		void Start()
		{
			navigator = FindObjectOfType<Navigator>();
			map = FindObjectOfType<Map>();
		}

		void Update()
		{
			if (!isInitialized)
			{
				map.ShowAsNavigator();

				isInitialized = true;
			}
		}

		public void SetNewDestination(int number)
		{
			if (targets.Length > enabledTarget && targets[enabledTarget])
				targets[enabledTarget].GetComponent<Renderer>().material.color = Color.gray;

			enabledTarget = number;

			if (targets.Length > enabledTarget && targets[enabledTarget])
			{
				targets[enabledTarget].GetComponent<Renderer>().material.color = Color.red;

				navigator.SetTargetPoint(targets[enabledTarget].position);
			}
			else
			{
				Debug.LogWarning("No target setted up in DemoHelper script. Check it for null fields.");
			}
		}

		public void DisableNavigation()
		{
			if (targets.Length > enabledTarget && targets[enabledTarget])
				targets[enabledTarget].GetComponent<Renderer>().material.color = Color.gray;

			navigator.StopNavigation();
		}
	}
}