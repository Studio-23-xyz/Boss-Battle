using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Studio23.SS2.UI.Misc
{
    public class ButtonAvailabilityController : MonoBehaviour
    {
        [SerializeField] private BaseNavigationController _buttonNavigationController;
        [SerializeField] private bool _initializeOnStart;
        [SerializeField] private bool _isInteractable;
        public PlatformButtonData _buttonEnabledOnPlatform;



        private void Start()
        {
            if (_initializeOnStart)
                FindNavigationController();
        }

        void OnDestroy()
        {
            if (_buttonNavigationController != null)
                _buttonNavigationController.ChildButtonStatusUpdated?.Invoke(gameObject);
        }

        private void Initialize()
        {
            _buttonNavigationController = GetComponentInParent<BaseNavigationController>(true);
            _buttonNavigationController.ChildButtonStatusUpdated?.Invoke(gameObject);
            _buttonNavigationController.ControllerDestroyed.AddListener(ResetButtonNavController);
        }

        public void UpdateButtonInteractability(bool isInteractable)
        {
            FindNavigationController();
            _isInteractable = isInteractable;
            _buttonNavigationController.ChildButtonStatusUpdated?.Invoke(gameObject);
        }

        private void FindNavigationController()
        {
            if(_buttonNavigationController == null)
                Initialize();
        }

        private void ResetButtonNavController()
        {
            _buttonNavigationController = null;
        }


        public bool IsButtonInteractable() => _isInteractable;
        public PlatformButtonData ButtonEnabledPlatfrom() => _buttonEnabledOnPlatform;
    }
}
