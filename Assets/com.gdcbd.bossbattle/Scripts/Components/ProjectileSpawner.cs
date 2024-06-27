using UnityEngine;

namespace com.gdcbd.bossbattle.components
{
    public class ProjectileSpawner : ObjectSpawner<ProjectileSpawner>
    {
        [SerializeField] private GameObject _bulletPrefab;

        protected override GameObject Prefab => _bulletPrefab;

        protected override void Initialize()
        {
        }
    }
}
