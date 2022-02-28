using UnityEngine;
using PokerDefense.Utils;
using TMPro;
using System;
using UnityEngine.EventSystems;

namespace PokerDefense.UI.Popup
{
    public class UI_HorseResultPopup : UI_Popup
    {
        enum GameObjects
        {
            ConfirmButton,
            ResultText,
            PriceText,
        }

        TextMeshProUGUI resultText, priceText;
        Action OnConfirmAction;

        private void Awake()
            => Init();

        public override void Init()
        {
            base.Init();

            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            resultText = GetObject((int)GameObjects.ResultText).GetComponent<TextMeshProUGUI>();
            priceText = GetObject((int)GameObjects.PriceText).GetComponent<TextMeshProUGUI>();

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);
        }

        public void InitUI(int rank, int price, Action OnConfirmAction)
        {
            this.OnConfirmAction = OnConfirmAction;

            if (rank == 1)
                RefreshResultText("¿ì½Â!");
            else
                RefreshResultText($"¾Æ½±°Ô {rank}µî");

            priceText.text = $"¿ì½Â »ó±Ý : {price}";
        }

        private void RefreshResultText(string text)
            => resultText.text = text;

        private void OnClickConfirmButton(PointerEventData evt)
        {
            OnConfirmAction?.Invoke();
            ClosePopupUI();
        }
    }
}
