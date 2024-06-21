using UnityEngine;

namespace com.gdcbd.bossbattle.components
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
    public abstract class AbstractGunController : ScriptableObject
    {
        public abstract void Shoot(ShootInfo shootInfo);
        public abstract void Reload();
      
    }
}