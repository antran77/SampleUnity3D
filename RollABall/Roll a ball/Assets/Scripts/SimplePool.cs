using UnityEngine;
using System.Collections.Generic;

public static class SimplePool {
	
	const int DEFAULT_POOL_SIZE = 20;
	
	class Pool {
		
		int nextId=1;

		
		Stack<GameObject> inactive;

		// The prefab that we are pooling
		GameObject prefab;

		// Constructor
		public Pool(GameObject prefab, int initialQty) {
			this.prefab = prefab;

			// If Stack uses a linked list internally, then this
			// whole initialQty thing is a placebo that we could
			// strip out for more minimal code. But it can't *hurt*.
			inactive = new Stack<GameObject>(initialQty);
		}

		// Spawn an object from our pool
		public GameObject Spawn(Vector3 pos, Quaternion rot) {
			GameObject obj;
			if(inactive.Count==0) {
				// We don't have an object in our pool, so we
				// instantiate a whole new object.
				obj = (GameObject)GameObject.Instantiate(prefab, pos, rot);
				obj.name = prefab.name + " ("+(nextId++)+")";

				// Add a PoolMember component so we know what pool
				// we belong to.
				obj.AddComponent<PoolMember>().myPool = this;
				Debug.Log("instantiate");
			}
			else {
				Debug.Log("grab object");
				// Grab the last object in the inactive array
				obj = inactive.Pop();

				if(obj == null) {
					
					return Spawn(pos, rot);
				}
			}

			obj.transform.position = pos;
			obj.transform.rotation = rot;
			obj.SetActive(true);
			return obj;

		}

		// Return an object to the inactive pool.
		public void Despawn(GameObject obj) {
			obj.SetActive(false);

			inactive.Push(obj);
		}

	}

	class PoolMember : MonoBehaviour {
		public Pool myPool;
	}

	// All of our pools
	static Dictionary< GameObject, Pool > pools;

	/// <summary>
	/// Initialize our dictionary.
	/// </summary>
	static void Init (GameObject prefab=null, int qty = DEFAULT_POOL_SIZE) {
		if(pools == null) {
			pools = new Dictionary<GameObject, Pool>();
		}
		if(prefab!=null && pools.ContainsKey(prefab) == false) {
			pools[prefab] = new Pool(prefab, qty);
		}
	}

	static public void Preload(GameObject prefab, int qty = 1) {
		Init(prefab, qty);

		// Make an array to grab the objects we're about to pre-spawn.
		GameObject[] obs = new GameObject[qty];
		for (int i = 0; i < qty; i++) {
			obs[i] = Spawn (prefab, Vector3.zero, Quaternion.identity);
		}

		// Now despawn them all.
		for (int i = 0; i < qty; i++) {
			Despawn( obs[i] );
		}
	}

	static public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot) {
		Init(prefab);

		return pools[prefab].Spawn(pos, rot);
	}

	static public void Despawn(GameObject obj) {
		PoolMember pm = obj.GetComponent<PoolMember>();
		if(pm == null) {
			Debug.Log ("Object '"+obj.name+"' wasn't spawned from a pool. Destroying it instead.");
			GameObject.Destroy(obj);
		}
		else {
			pm.myPool.Despawn(obj);
		}
	}
	
}