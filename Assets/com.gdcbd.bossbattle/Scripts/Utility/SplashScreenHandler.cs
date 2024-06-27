using UnityEngine;

namespace com.gdcbd.bossbattle.utility
{
    public class SplashScreenHandler : MonoBehaviour
    {
        public void EulaAction(bool accepted)
        {
            if(!accepted)
                Application.Quit();
        }
    }
}
