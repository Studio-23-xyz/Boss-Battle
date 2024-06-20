using UnityEngine;

namespace com.gdcbd.bossbattle.Scripts.Components
{
    public abstract class AbstractProjectileController : ScriptableObject
    {
        public abstract void Launch(ShootInfo shootInfo);
    }

}