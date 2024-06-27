using UnityEngine;

public class ProjectileSpawner : ObjectSpawner<ProjectileSpawner>
{
    [SerializeField] private GameObject _bulletPrefab;

    protected override GameObject Prefab => _bulletPrefab;

    protected override void Initialize()
    {
    }
}
