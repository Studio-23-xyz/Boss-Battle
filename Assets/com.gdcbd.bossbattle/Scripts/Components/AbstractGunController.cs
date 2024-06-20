using UnityEngine;

namespace com.gdcbd.bossbattle.Scripts.Components
{
    public abstract class AbstractGunController : ScriptableObject
    {
        public abstract void Shoot(ShootInfo shootInfo);
        public abstract void Reload();
      
    }
}