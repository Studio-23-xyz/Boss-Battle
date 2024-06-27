using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Studio23.SS2
{
    public class BanglaFontLocalizer : MonoBehaviour
    {

        private LocalizedString _localizedString;
        private TextMeshProUGUI _textField;

        void Awake()
        {
            if (TryGetComponent(out LocalizeStringEvent localizeStringEvent))
            {
                localizeStringEvent.OnUpdateString.AddListener(UpdateText);
                _localizedString = localizeStringEvent.StringReference;
            }
            _textField = GetComponent<TextMeshProUGUI>();
        }

        void OnEnable()
        {
            if(_localizedString!=null && _textField!=null)
                _localizedString.StringChanged += UpdateText;
        }

        void OnDisable()
        {
            if (_localizedString != null && _textField != null)
                _localizedString.StringChanged -= UpdateText;
        }

        private void UpdateText(string value)
        {
            var fixedText = BangleTextMeshProIssueFixingController.Instance.FixBangleOrder(value);
            _textField.text = fixedText;
        }

        public void UpdateText()
        {
            UpdateText(_textField.text);
        }
    }
}
