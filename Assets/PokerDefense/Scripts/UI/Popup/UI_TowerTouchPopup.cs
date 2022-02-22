using PokerDefense.Managers;
using PokerDefense.Towers;
using PokerDefense.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PokerDefense.UI.Popup
{
    public class UI_TowerTouchPopup : UI_Popup
    {
        TowerPanel touchedTowerPanel;

        enum GameObjects
        {
            UpgradeButton,
            DestroyButton,
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
            GameObject upgradeButton = GetObject((int)GameObjects.UpgradeButton);
            AddUIEvent(upgradeButton, OnClickUpgradeButton, Define.UIEvent.Click);
            AddButtonAnim(upgradeButton);

            GameObject destroyButton = GetObject((int)GameObjects.DestroyButton);
            AddUIEvent(destroyButton, OnClickDestroyButton, Define.UIEvent.Click);
            AddButtonAnim(destroyButton);

            GameObject cancelButton = GetObject((int)GameObjects.CancelButton);
            AddUIEvent(cancelButton, OnClickCancelButton, Define.UIEvent.Click);
            AddButtonAnim(cancelButton);

            GameObject backButton = GetObject((int)GameObjects.BackButton);
            AddUIEvent(backButton, OnClickBackButton, Define.UIEvent.Click);
            AddButtonAnim(backButton);
        }

        public void SetTouchedTowerPanel(TowerPanel target)
        {
            touchedTowerPanel = target;

            GameManager.Tower.StartTowerPanelSelect(touchedTowerPanel);
        }

        private void OnClickUpgradeButton(PointerEventData evt)
        {
            Tower tower = touchedTowerPanel.GetTower();
            tower.UpgradeDamageLevel();
        }

        private void OnClickDestroyButton(PointerEventData evt)
        {
            UI_TowerDestroyPopup popup = GameManager.UI.ShowPopupUI<UI_TowerDestroyPopup>();
            popup.SetTowerTouchPopup(this);
        }

        private void OnClickCancelButton(PointerEventData evt)
        {
            touchedTowerPanel.ResetPanel();
            ClosePopupUI();
        }

        private void OnClickBackButton(PointerEventData evt)
        {
            touchedTowerPanel.ResetPanel();
            ClosePopupUI();
        }

        public override void ClosePopupUI()
        {
            GameManager.Tower.EndTowerPanelSelect(touchedTowerPanel);

            base.ClosePopupUI();
        }

        public void ClosePopupUI(Action action)
        {
            action?.Invoke();
            ClosePopupUI();
        }

        public TowerPanel GetTowerPanel()
            => touchedTowerPanel;
    }
}
