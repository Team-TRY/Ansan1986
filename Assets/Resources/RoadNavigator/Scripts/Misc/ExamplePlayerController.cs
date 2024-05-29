using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InsaneSystems.RoadNavigator.Misc
{
	public class ExamplePlayerController : MonoBehaviour
	{
		NavMeshAgent navAgent;

		void Start()
		{
			navAgent = GetComponent<NavMeshAgent>();
		}

		void Update()
		{
			float speed = 2f;

			float x = Input.GetAxis("Horizontal") * speed;
			float y = Input.GetAxis("Vertical") * speed;

			Vector3 forwardDest = Vector3.zero;
			Vector3 rightDest = Vector3.zero;

			if (y != 0)
				forwardDest = transform.forward * y;
			if (x != 0)
				rightDest = transform.right * x;

			navAgent.destination = transform.position + forwardDest + rightDest;
		}
	}
}