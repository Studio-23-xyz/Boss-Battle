using UnityEngine;

namespace com.gdcbd.bossbattle.components
{
    [CreateAssetMenu(fileName = "NewGunController", menuName = "BossBattle/GunController")]
    public class GunController : AbstractGunController
    {
        public GameObject GunPrefab;
        [SerializeField] private ProjectileController _projectile;
        [SerializeField] private int _ammoCount;
        [SerializeField] private int _magazineSize = 7;
        [SerializeField] private float _fireRate = 0.2f;

        private float _nextFireTime;

        public void Reset()
        {
            _nextFireTime = 0;
        }

        private float _time => TimeManager.Instance.TimeCount();

        public override void Shoot(ShootInfo shootInfo)
        {
            if (_time > _nextFireTime && _ammoCount > 0)
            {
                _ammoCount--;
                _nextFireTime = _time + 1f / _fireRate;
                _projectile.LaunchWith(shootInfo);
            }
            else if (_ammoCount <= 0)
            {
                Reload();
            }
        }

        public override void Reload()
        {
            _ammoCount = _magazineSize;
        }
    }
}