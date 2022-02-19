using UnityEngine;
using PokerDefense.Utils;
using UnityEngine.EventSystems;
using PokerDefense.Managers;

namespace PokerDefense.UI.Popup
{
    public class UI_InGameMenuPopup : UI_Popup
    {
        enum GameObjects
        {
            SaveButton,
            QuitButton,
            BackButton,
        }

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

            GameObject saveButton = GetObject((int)GameObjects.SaveButton);
            AddUIEvent(saveButton, OnClickSaveButton, Define.UIEvent.Click);
            AddButtonAnim(saveButton);

            GameObject quitButton = GetObject((int)GameObjects.QuitButton);
            AddUIEvent(quitButton, OnClickQuitButton, Define.UIEvent.Click);
            AddButtonAnim(quitButton);

            GameObject backButton = GetObject((int)GameObjects.BackButton);
            AddUIEvent(backButton, OnClickBackButton, Define.UIEvent.Click);
            AddButtonAnim(backButton);
        }

        private void OnClickSaveButton(PointerEventData evt)
        {
            GameManager.Data.SaveSlotData();
        }

        private void OnClickQuitButton(PointerEventData evt)
        {

        }

        private void OnClickBackButton(PointerEventData evt)
        {
            ClosePopupUI();
        }
    }
}
