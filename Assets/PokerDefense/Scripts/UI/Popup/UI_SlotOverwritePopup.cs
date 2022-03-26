using UnityEngine;
using PokerDefense.Utils;
using PokerDefense.Managers;

namespace PokerDefense.UI.Popup
{
    public class UI_SlotOverwritePopup : UI_Popup
    {
        enum GameObjects
        {
            ConfirmButton,
            CancelButton,
        }

        public int SlotIndex { get; set; }

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();

            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, (a) => InGameManager.NewGame(SlotIndex, true), Define.UIEvent.Click);
            AddButtonAnim(confirmButton);

            GameObject cancelButton = GetObject((int) GameObjects.CancelButton);
            AddUIEvent(cancelButton, (a) => ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(cancelButton);
        }
    }
}
