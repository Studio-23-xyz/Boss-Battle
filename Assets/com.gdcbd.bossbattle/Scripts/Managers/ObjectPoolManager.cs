using System.Collections.Generic;
using com.gdcbd.bossbattle.utility;
using UnityEngine;

namespace com.gdcbd.bossbattle
{
    public class ObjectPoolManager : PersistentMonoSingleton<ObjectPoolManager>
    {
       

        public List<GameObject> bulletPrefabs;
        public int amountToPool;

        private Dictionary<string, List<GameObject>> pooledObjects;
        protected override void Initialize()
        {
            pooledObjects = new Dictionary<string, List<GameObject>>();

            foreach (GameObject prefab in bulletPrefabs)
            {
                if (!pooledObjects.ContainsKey(prefab.name))
                {
                    pooledObjects[prefab.name] = new List<GameObject>();

                    for (int i = 0; i < amountToPool; i++)
                    {
                        GameObject obj = Instantiate(prefab);
                        obj.SetActive(false);
                        pooledObjects[prefab.name].Add(obj);
                    }
                }
            }
        }
        public GameObject GetPooledObject(string prefabName)
        {
            if (pooledObjects.ContainsKey(prefabName))
            {
                foreach (GameObject obj in pooledObjects[prefabName])
                {
                    if (!obj.activeInHierarchy)
                    {
                        return obj;
                    }
                }
                foreach (GameObject prefab in bulletPrefabs)
                {
                    if (prefab.name == prefabName)
                    {
                        GameObject newObj = Instantiate(prefab);
                        newObj.SetActive(false);
                        pooledObjects[prefabName].Add(newObj);
                        return newObj;
                    }
                }
            }
            return null;
        }

        public GameObject GetPooledObject(GameObject prefab)
        {
            return GetPooledObject(prefab.name);
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
        }
    }
}
