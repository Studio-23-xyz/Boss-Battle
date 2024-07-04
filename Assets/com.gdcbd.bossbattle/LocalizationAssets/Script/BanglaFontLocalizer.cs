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
        public bool CallAtStart;

        void Awake()
        {
            Initialize();
            _textField = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            if(CallAtStart)
                UpdateText(_localizedString.GetLocalizedString());
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
            if(_localizedString!=null && _localizedString!= GetComponent<LocalizeStringEvent>().StringReference) Initialize();

             //var fixedText = BangleTextMeshProIssueFixingController.Instance.FixBangleOrder(value);
           // _textField.text = fixedText;
        }

        void Initialize()
        {
            if (TryGetComponent(out LocalizeStringEvent localizeStringEvent))
            {
                localizeStringEvent.OnUpdateString.AddListener(UpdateText);
                _localizedString = localizeStringEvent.StringReference;
            }
        }


        public void UpdateText()
        {
            UpdateText(_textField.text);
        }
    }
}