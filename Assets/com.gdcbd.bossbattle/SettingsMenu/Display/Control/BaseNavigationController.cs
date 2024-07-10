using System;
using Studio23.SS2.UI.Misc;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using Studio23.SS2.AudioSystem.fmod.Core;
using UnityEngine.EventSystems;

namespace Studio23.SS2
{
    public class BaseNavigationController : MonoBehaviour
    {
        [SerializeField] protected List<Button> _buttonsToNavigate;
        public UnityEvent<GameObject> ChildButtonStatusUpdated;
        public UnityEvent ControllerDestroyed;
        protected PlatformButtonData _buttonPlatform;
        protected CancellationTokenSource _navigationCancelToken;


        void Start()
        {
            _navigationCancelToken = new CancellationTokenSource();
        }

        void OnDestroy()
        {
            ControllerDestroyed?.Invoke();
        }

        private void SetProviderType()
        {
            _buttonPlatform = PlatformButtonData.Default;

            //TODO Have to add for playstation and nintendo later
#if UNITY_GAMECORE
            _buttonPlatform = PlatformButtonData.XBox_Console;
#endif

#if MICROSOFT_GAME_CORE
            _buttonPlatform = PlatformButtonData.XBox_PC;
#endif

#if STEAMWORKS_ENABLED
            _buttonPlatform = PlatformButtonData.Steam;
#endif

        }

        protected virtual void OnDisable()
        {
            ChildButtonStatusUpdated?.RemoveListener((_ => SetButtonNavigation()));
            _navigationCancelToken?.Cancel();
        }

        protected virtual void OnEnable()
        {
            SetButtonNavigation();
            ChildButtonStatusUpdated?.AddListener((_ => SetButtonNavigation()));
        }

        public virtual async void SetButtonNavigation() { }

        protected List<Button> ButtonPruning(List<ButtonAvailabilityController> allButtons)
        {
            List<Button> prunedButtons = new List<Button>();
            foreach (var buttonController in allButtons)
            {
                buttonController.gameObject.SetActive(false);
                if ((buttonController.ButtonEnabledPlatfrom() & _buttonPlatform) != _buttonPlatform) continue;
                if (!buttonController.TryGetComponent(out Button prunedButton))
                {
                    buttonController.gameObject.SetActive(true);
                    continue;
                }
                if (!buttonController.IsButtonInteractable()) continue;
                if (prunedButtons.Contains(prunedButton)) continue;

                buttonController.gameObject.SetActive(true);
                prunedButtons.Add(prunedButton);
            }
            return prunedButtons;
        }

        public void SetFirstSelectedButton(int index)
        {
            EventSystem.current.SetSelectedGameObject(index < 0 ? null: _buttonsToNavigate[index].gameObject);
        }
    }


}
