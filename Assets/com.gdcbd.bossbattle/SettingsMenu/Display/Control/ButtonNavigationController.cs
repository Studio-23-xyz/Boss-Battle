using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio23.SS2.UI.Misc
{
    public class ButtonNavigationController : BaseNavigationController
    {
        [SerializeField]private bool _verticalNav = true;
        [SerializeField] private bool _horizontalNav = false;
        [SerializeField] private int _perRowCount = 3;
        public override async void SetButtonNavigation()
        {
            _navigationCancelToken?.Cancel();
            _navigationCancelToken = new CancellationTokenSource();
            var isCancelled = await UniTask.Delay(TimeSpan.FromSeconds(.1f), ignoreTimeScale: true, cancellationToken: _navigationCancelToken.Token).SuppressCancellationThrow();
            if (isCancelled) return;

            var navigationList = GetComponentsInChildren<ButtonAvailabilityController>(true).ToList();

            _buttonsToNavigate = new List<Button>();
            _buttonsToNavigate = ButtonPruning(navigationList);

            var isCancelled2 = await UniTask.Delay(TimeSpan.FromSeconds(.1f), ignoreTimeScale: true, cancellationToken: _navigationCancelToken.Token).SuppressCancellationThrow();
            if (isCancelled2) return;

            for (int i = 0; i < _buttonsToNavigate.Count; i++)
            {
                Navigation currentButtonNav = _buttonsToNavigate[i].navigation;
                if (_verticalNav && !_horizontalNav)
                {
                    currentButtonNav.selectOnDown = (i + 1) < _buttonsToNavigate.Count ? _buttonsToNavigate[i + 1] : _buttonsToNavigate[0];
                    currentButtonNav.selectOnUp = (i - 1) >= 0 ? _buttonsToNavigate[i - 1] : _buttonsToNavigate[^1];
                }
                else if (_verticalNav && !_horizontalNav)
                {
                    currentButtonNav.selectOnRight = (i + 1) < _buttonsToNavigate.Count ? _buttonsToNavigate[i + 1] : _buttonsToNavigate[0];
                    currentButtonNav.selectOnLeft = (i - 1) >= 0 ? _buttonsToNavigate[i - 1] : _buttonsToNavigate[^1];
                }
                else if (_verticalNav && _horizontalNav)
                {
                    currentButtonNav.selectOnDown = (i + _perRowCount) < _buttonsToNavigate.Count ? _buttonsToNavigate[i + _perRowCount] : _buttonsToNavigate[i % _perRowCount];
                    currentButtonNav.selectOnUp = (i - _perRowCount) >= 0 ? _buttonsToNavigate[i - _perRowCount] : _buttonsToNavigate[^1];
                    currentButtonNav.selectOnLeft = (i - 1) >= 0 ? _buttonsToNavigate[i - 1] : _buttonsToNavigate[^1];
                    currentButtonNav.selectOnRight = (i + 1) < _buttonsToNavigate.Count ? _buttonsToNavigate[i + 1] : _buttonsToNavigate[0];
                }

                _buttonsToNavigate[i].navigation = currentButtonNav;
            }

            SetFirstSelectedButton(_buttonsToNavigate.Count > 0 ? 0 : -1);
        }
    }
}
