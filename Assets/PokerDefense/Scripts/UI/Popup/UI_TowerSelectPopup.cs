using PokerDefense.Managers;
using PokerDefense.Towers;
using PokerDefense.Utils;
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

        public override void Init()
        {
            base.Init();

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

            InGameManager.Tower.StartTowerPanelSelect(selectedTowerPanel);
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            //속성이 없는 기본 타워 기반
            selectedTowerPanel.SetTowerBaseStatus(true);
            InGameManager.Tower.AfterTowerBaseConstructed(selectedTowerPanel);

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

        public override void ClosePopupUI()
        {
            InGameManager.Tower.EndTowerPanelSelect(selectedTowerPanel);

            base.ClosePopupUI();
        }

        public TowerPanel GetTowerPanel()
            => selectedTowerPanel;
    }
}
