using com.gdcbd.bossbattle.components;
using com.gdcbd.bossbattle.utility;
using UnityEngine;
using UnityEngine.Pool;

namespace com.gdcbd.bossbattle.components
{
    public abstract class ObjectSpawner<T> : PersistentMonoSingleton<ObjectSpawner<T>> where T : MonoBehaviour
    {
        protected abstract GameObject Prefab { get; }

        public enum PoolType
        {
            Stack,
            LinkedList
        }

        public PoolType ObjectTypePool;
        public bool CollectionChecks = true;
        public int MaxPoolSize = 100;

        private IObjectPool<GameObject> _pool;

        public IObjectPool<GameObject> Pool
        {
            get
            {
                if (_pool == null)
                {
                    if (ObjectTypePool == PoolType.Stack)
                        _pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                            OnDestroyPoolObject, CollectionChecks, 10, MaxPoolSize);
                    else
                        _pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                            OnDestroyPoolObject, CollectionChecks, MaxPoolSize);
                }

                return _pool;
            }
        }

        private GameObject CreatePooledItem()
        {
            var go = Instantiate(Prefab);
            go.SetActive(false);
            var returnToPool = go.AddComponent<PoolObject>();
            returnToPool.pool = Pool;

            return go;
        }

        private void OnReturnedToPool(GameObject obj)
        {
            obj.SetActive(false);
        }

        private void OnTakeFromPool(GameObject obj)
        {
            obj.SetActive(true);
        }

        private void OnDestroyPoolObject(GameObject obj)
        {
            Destroy(obj);
        }
    }
}
