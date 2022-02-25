using PokerDefense.Managers;
using PokerDefense.Towers;
using PokerDefense.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PokerDefense.UI.Popup
{
    public class UI_TowerTouchPopup : UI_Popup
    {
        TowerPanel touchedTowerPanel;
        Tower touchedTower;
        TextMeshProUGUI damageText, speedText, rangeText, criticalText;

        GameObject towerIdleArea;

        enum GameObjects
        {
            DamageUpgradeButton,
            SpeedUpgradeButton,
            RangeUpgradeButton,
            CriticalUpgradeButton,
            DamageText,
            SpeedText,
            RangeText,
            CriticalText,
            DestroyButton,
            BackButton,
        }

        private void Awake()
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

            damageText = GetObject((int)GameObjects.DamageText).GetComponent<TextMeshProUGUI>();
            speedText = GetObject((int)GameObjects.SpeedText).GetComponent<TextMeshProUGUI>();
            rangeText = GetObject((int)GameObjects.RangeText).GetComponent<TextMeshProUGUI>();
            criticalText = GetObject((int)GameObjects.CriticalText).GetComponent<TextMeshProUGUI>();

            GameObject destroyButton = GetObject((int)GameObjects.DestroyButton);
            AddUIEvent(destroyButton, OnClickDestroyButton, Define.UIEvent.Click);
            AddButtonAnim(destroyButton);

            GameObject backButton = GetObject((int)GameObjects.BackButton);
            AddUIEvent(backButton, OnClickBackButton, Define.UIEvent.Click);
            AddButtonAnim(backButton);
        }

        public void InitUI(TowerPanel target)
        {
            touchedTowerPanel = target;
            touchedTower = touchedTowerPanel.GetTower();
            string towerName = touchedTower.towerIndivData.TowerName;

            LoadTowerIdleAnim(towerName);
            SetUIText(touchedTower);

            GameManager.Tower.StartTowerPanelSelect(touchedTowerPanel);
        }

        private void LoadTowerIdleAnim(string towerName)
        {
            towerIdleArea = GameObject.FindGameObjectWithTag("TowerIdleArea");
            GameManager.Resource.Instantiate($"TowerIdle/{towerName}", towerIdleArea.transform);
        }

        private void SetUIText(Tower tower)
        {
            damageText.text = $"데미지 : {tower.towerIndivData.Damage.ToString()}";
            speedText.text = $"공속 : {tower.towerIndivData.Speed.ToString()}";
            rangeText.text = $"사거리 : {tower.towerIndivData.Range.ToString()}";
            criticalText.text = $"크확 : {tower.towerIndivData.Critical.ToString()}";
        }

        private void OnClickDamageUpgradeButton(PointerEventData evt)
        {
            touchedTower.UpgradeDamageLevel();
            SetUIText(touchedTower);
        }

        private void OnClickSpeedUpgradeButton(PointerEventData evt)
        {
            touchedTower.UpgradeSpeedLevel();
            SetUIText(touchedTower);
        }

        private void OnClickRangeUpgradeButton(PointerEventData evt)
        {
            touchedTower.UpgradeRangeLevel();
            SetUIText(touchedTower);
        }

        private void OnClickCriticalUpgradeButton(PointerEventData evt)
        {
            touchedTower.UpgradeCriticalLevel();
            SetUIText(touchedTower);
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
