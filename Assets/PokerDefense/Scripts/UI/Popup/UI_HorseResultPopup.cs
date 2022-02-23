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
        }

        TextMeshProUGUI resultText;
        Action OnConfirm;

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

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);
        }

        public void InitUI(int rank, Action OnConfirm)
        {
            if (rank == 1)
                RefreshResultText("���!");
            else
                RefreshResultText($"�ƽ��� {rank}��");

            this.OnConfirm = OnConfirm;
        }

        private void RefreshResultText(string text)
            => resultText.text = text;

        private void OnClickConfirmButton(PointerEventData evt)
        {
            OnConfirm?.Invoke();
            ClosePopupUI();
        }
    }
}
