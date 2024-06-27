using UnityEngine;

namespace com.gdcbd.bossbattle.components
{
    public class ShootInfo
    {
        public ShootInfo(Transform startTransform, Vector2 directon)
        {
            StartTransform = startTransform;
            Directon = directon;
        }
        public Transform StartTransform;
        public Vector2 Directon;
    }
    public abstract class AbstractGunController : ScriptableObject
    {
        public abstract void Shoot(ShootInfo shootInfo);
        public abstract void Reload();
      
    }
}