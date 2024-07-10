using UnityEngine;

namespace Studio23.SS2
{
    public class CinemachineBrainUpdate : MonoBehaviour
    {
        private bool _updateCinemachineBrain;

        void OnEnable()
        {
            _updateCinemachineBrain = true;
        }

        void LateUpdate()
        {
            if (!_updateCinemachineBrain) return;
            _updateCinemachineBrain = false;
        }
    }
}
