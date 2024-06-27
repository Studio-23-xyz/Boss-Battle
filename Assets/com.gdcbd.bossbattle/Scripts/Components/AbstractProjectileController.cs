using UnityEngine;

namespace com.gdcbd.bossbattle.components
{
    public abstract class AbstractProjectileController : ScriptableObject
    {
        public abstract void LaunchWith(ShootInfo shootInfo);
    }

}