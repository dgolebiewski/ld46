using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	public static ObjectPooler instance;

	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	void Awake()
	{
		if(instance != null)
		{
			Destroy(this.gameObject);
			return;
		}

		instance = this;

		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach(Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();
		
			for(int i = 0; i < pool.size; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDictionary.Add(pool.tag, objectPool);
		}
	}

	public GameObject Spawn(string tag, Vector3 position, Quaternion rotation)
	{
		if(!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
			return null;
		}

		GameObject obj = poolDictionary[tag].Dequeue();

		obj.transform.position = position;
		obj.transform.rotation = rotation;
		obj.SetActive(true);

		IPooledObject[] pooledObject = obj.GetComponentsInChildren<IPooledObject>();
		foreach(IPooledObject p in pooledObject)
			p.OnObjectSpawned();

		poolDictionary[tag].Enqueue(obj);

		return obj;
	}

	public void Deactivate(GameObject go, float delay)
	{
		StartCoroutine(DelayedDeactivation(go, delay));
	}

	IEnumerator DelayedDeactivation(GameObject go, float delay)
	{
		yield return new WaitForSeconds(delay);
		go.SetActive(false);
	}
}
