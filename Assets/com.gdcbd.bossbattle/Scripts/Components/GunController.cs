using UnityEngine;

namespace com.gdcbd.bossbattle.Scripts.Components
{
    public class ShootInfo
    {
        public ShootInfo(Transform startTransform, Vector2 directon)
        {
            this.startTransform = startTransform;
            this.directon = directon;
        }

        public Transform startTransform;
        public Vector2 directon;
    }
    [CreateAssetMenu(fileName = "NewGunController", menuName = "BossBattle/GunController")]
    public class GunController : AbstractGunController
    {
        public GameObject _gunPrefab;
       
        public ProjectileController projectile;
        public int ammoCount;
        public int magazineSize;
        public float fireRate;
       

        private float nextFireTime;
        
        public override void Shoot(ShootInfo shootInfo)
        {
           
            if ( ammoCount > 0)
            {
                ammoCount--;
                nextFireTime = Time.time + 1f / fireRate;
                projectile.Launch(shootInfo);
            }
            else if (ammoCount <= 0)
            {
                Reload();
            }
        }

        public override void Reload()
        {
            ammoCount = magazineSize;
        }
        
    }
}