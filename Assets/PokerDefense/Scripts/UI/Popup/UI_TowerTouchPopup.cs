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
            DamageUpgradeButton,
            SpeedUpgradeButton,
            RangeUpgradeButton,
            CriticalUpgradeButton,
            DestroyButton,
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
            GameObject damageUpgradeButton = GetObject((int)GameObjects.DamageUpgradeButton);
            AddUIEvent(damageUpgradeButton, OnClickDamageUpgradeButton, Define.UIEvent.Click);
            AddButtonAnim(damageUpgradeButton);

            GameObject speedUpgradeButton = GetObject((int)GameObjects.SpeedUpgradeButton);
            AddUIEvent(speedUpgradeButton, OnClickSpeedUpgradeButton, Define.UIEvent.Click);
            AddButtonAnim(speedUpgradeButton);

            GameObject rangeUpgradeButton = GetObject((int)GameObjects.RangeUpgradeButton);
            AddUIEvent(rangeUpgradeButton, OnClickRangeUpgradeButton, Define.UIEvent.Click);
            AddButtonAnim(rangeUpgradeButton);

            GameObject criticalUpgradeButton = GetObject((int)GameObjects.CriticalUpgradeButton);
            AddUIEvent(criticalUpgradeButton, OnClickCriticalUpgradeButton, Define.UIEvent.Click);
            AddButtonAnim(criticalUpgradeButton);

            GameObject destroyButton = GetObject((int)GameObjects.DestroyButton);
            AddUIEvent(destroyButton, OnClickDestroyButton, Define.UIEvent.Click);
            AddButtonAnim(destroyButton);

            GameObject backButton = GetObject((int)GameObjects.BackButton);
            AddUIEvent(backButton, OnClickBackButton, Define.UIEvent.Click);
            AddButtonAnim(backButton);
        }

        public void SetTouchedTowerPanel(TowerPanel target)
        {
            touchedTowerPanel = target;

            GameManager.Tower.StartTowerPanelSelect(touchedTowerPanel);
        }

        private void OnClickDamageUpgradeButton(PointerEventData evt)
        {
            Tower tower = touchedTowerPanel.GetTower();
            tower.UpgradeDamageLevel();
        }

        private void OnClickSpeedUpgradeButton(PointerEventData evt)
        {
            Tower tower = touchedTowerPanel.GetTower();
            tower.UpgradeSpeedLevel();
        }

        private void OnClickRangeUpgradeButton(PointerEventData evt)
        {
            Tower tower = touchedTowerPanel.GetTower();
            tower.UpgradeRangeLevel();
        }

        private void OnClickCriticalUpgradeButton(PointerEventData evt)
        {
            Tower tower = touchedTowerPanel.GetTower();
            tower.UpgradeCriticalLevel();
        }

        private void OnClickDestroyButton(PointerEventData evt)
        {
            UI_TowerDestroyPopup popup = GameManager.UI.ShowPopupUI<UI_TowerDestroyPopup>();
            popup.SetTowerTouchPopup(this);
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
