using PokerDefense.Managers;
using PokerDefense.Towers;
using PokerDefense.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PokerDefense.UI.Popup
{
    public class UI_TowerDestroyPopup : UI_Popup
    {
        UI_TowerTouchPopup towerTouchPopup;

        enum GameObjects
        {
            ConfirmButton,
            CancelButton,
        }

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));
            BindObjects();
        }

        public void SetTowerTouchPopup(UI_TowerTouchPopup target)
            => towerTouchPopup = target;

        private void BindObjects()
        {
            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);

            GameObject cancelButton = GetObject((int)GameObjects.CancelButton);
            AddUIEvent(cancelButton, OnClickCancelButton, Define.UIEvent.Click);
            AddButtonAnim(cancelButton);
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            GameManager.Tower.DestroyTower(towerTouchPopup.GetTowerPanel().GetTower(), 
                () => towerTouchPopup.ClosePopupUI(this.ClosePopupUI)
                );
        }

        private void OnClickCancelButton(PointerEventData evt)
            => ClosePopupUI();
    }
}
