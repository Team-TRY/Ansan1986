using System.Collections.Generic;
using UnityEngine;

namespace InsaneSystems.RoadNavigator.Misc
{
	public class GraphicsPool
	{
		readonly List<PooledObject> pooledObjects = new List<PooledObject>();

		public void Reset()
		{
			pooledObjects.Clear();
		}
		
		public GameObject Get(GameObject prefab, Transform parent = null)
		{
			var hashCode = prefab.GetHashCode();

			for (var i = 0; i < pooledObjects.Count; i++)
			{
				if (!pooledObjects[i].isUsed && pooledObjects[i].prefabHash == hashCode)
				{
					pooledObjects[i].StartUse();
					return pooledObjects[i].spawnedObject;
				}
			}

			var spawnedObject = GameObject.Instantiate(prefab, parent);

			var pooledNew = new PooledObject(hashCode, spawnedObject);
			pooledNew.StartUse();

			pooledObjects.Add(pooledNew);

			return pooledNew.spawnedObject;
		}

		public void Return(GameObject spawnedObject)
		{
			for (var i = 0; i < pooledObjects.Count; i++)
			{
				if (pooledObjects[i].spawnedObject == spawnedObject)
				{
					pooledObjects[i].EndUse();
					break;
				}
			}
		}
		
		class PooledObject
		{
			public int prefabHash { get; private set; }
			public GameObject spawnedObject { get; private set; }
			public bool isUsed { get; protected set; }

			public PooledObject(int hash, GameObject selfObject)
			{
				prefabHash = hash;
				spawnedObject = selfObject;
			}

			public void StartUse()
			{
				spawnedObject.SetActive(true);
				isUsed = true;
			}

			public void EndUse()
			{
				spawnedObject.SetActive(false);
				isUsed = false;
			}
		}
	}
}