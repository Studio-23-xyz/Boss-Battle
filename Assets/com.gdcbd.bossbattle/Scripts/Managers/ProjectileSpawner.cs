using com.gdcbd.bossbattle.utility;
using UnityEngine;
using UnityEngine.Pool;

namespace com.gdcbd.bossbattle.components
{
    public class ProjectileSpawner : PersistentMonoSingleton<ProjectileSpawner>
    {
        protected override void Initialize()
        {
        }

        public enum PoolType
        {
            Stack,
            LinkedList
        }

        public PoolType ProjectilePoolType;
        public bool CollectionChecks = true;
        public int MaxPoolSize = 100;
        [SerializeField] private GameObject _bulletPrefab;

        private IObjectPool<GameObject> _pool;

        public IObjectPool<GameObject> Pool
        {
            get
            {
                if (_pool == null)
                {
                    if (ProjectilePoolType == PoolType.Stack)
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
            var go = Instantiate(_bulletPrefab);
            go.SetActive(false);
            var returnToPool = go.AddComponent<ProjectilePool>(); // return bullets to the pool when they are disabled.
            returnToPool.pool = Pool;

            return go;
        }

        //  Release
        private void OnReturnedToPool(GameObject obj)
        {
            obj.SetActive(false);
        }

        //  Get
        private void OnTakeFromPool(GameObject obj)
        {
            obj.SetActive(true);
        }

        //pool capacity is reached, items returned will be destroyed.
        private void OnDestroyPoolObject(GameObject obj)
        {
            Destroy(obj);
        }
    }
}