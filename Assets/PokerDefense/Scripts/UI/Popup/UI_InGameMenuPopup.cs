using UnityEngine;
using PokerDefense.Utils;
using UnityEngine.EventSystems;
using PokerDefense.Managers;
using UnityEngine.SceneManagement;

namespace PokerDefense.UI.Popup
{
    public class UI_InGameMenuPopup : UI_Popup
    {
        enum GameObjects
        {
            SaveButton,
            HelpButton,
            QuitButton,
            BackButton,
        }

        private void Start()
            => Init();

        public override void Init()
        {
            isStoppable = true;

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
            AddUIEvent(backButton, (e)=>ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(backButton);

            GameObject helpButton = GetObject((int)GameObjects.HelpButton);
            AddUIEvent(helpButton, (e)=>GameManager.UI.ShowPopupUI<UI_HelpPopup>(), Define.UIEvent.Click);
            AddButtonAnim(helpButton);
        }

        private void OnClickSaveButton(PointerEventData evt)
        {
            GameManager.Data.SaveSlotData();
        }

        private void OnClickQuitButton(PointerEventData evt)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
