using PokerDefense.Managers;
using PokerDefense.Towers;
using PokerDefense.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PokerDefense.UI.Popup
{
    public class UI_TowerSelectPopup : UI_Popup
    {
        TowerPanel selectedTowerPanel;

        enum GameObjects
        {
            ConfirmButton,
            CancelButton,
            BackButton,
        }

        private void Start()
            => Init();

        private void Init()
        {
            Bind<GameObject>(typeof(GameObjects));
            BindObjects();
        }

        private void BindObjects()
        {
            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);

            GameObject cancelButton = GetObject((int)GameObjects.CancelButton);
            AddUIEvent(cancelButton, OnClickCancelButton, Define.UIEvent.Click);
            AddButtonAnim(cancelButton);

            GameObject backButton = GetObject((int)GameObjects.BackButton);
            AddUIEvent(backButton, OnClickBackButton, Define.UIEvent.Click);
            AddButtonAnim(backButton);
        }

        public void SetTowerPanel(TowerPanel target)
        {
            selectedTowerPanel = target;
            selectedTowerPanel.HighligtPanel();
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            GameManager.Tower.SetSelectedTowerPanel(selectedTowerPanel);
            ClosePopupUI();
        }

        private void OnClickCancelButton(PointerEventData evt)
        {
            selectedTowerPanel.ResetPanel();
            ClosePopupUI();
        }

        private void OnClickBackButton(PointerEventData evt)
        {
            selectedTowerPanel.ResetPanel();
            ClosePopupUI();
        }
    }
}
