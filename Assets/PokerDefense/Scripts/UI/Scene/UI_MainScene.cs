using PokerDefense.Managers;
using PokerDefense.UI.Popup;
using PokerDefense.UI.Scene;
using PokerDefense.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace PokerDefense.UI.Scene
{
    public class UI_MainScene : UI_Scene
    {
        enum GameObjects
        {
            NewGameButton,
            ContinueButton,
            SettingButton,
            HelpButton,
            CreditButton,
            ExitButton,
        }

        GameObject newGameButton, continueButton, settingButton, helpButton, creditButton, exitButton;

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
            newGameButton = GetObject((int)GameObjects.NewGameButton);
            AddUIEvent(newGameButton, OnClickNewGameButton, Define.UIEvent.Click);
            AddButtonAnim(newGameButton);

            continueButton = GetObject((int)GameObjects.ContinueButton);
            AddUIEvent(continueButton, OnClickContinueButton, Define.UIEvent.Click);
            AddButtonAnim(continueButton);

            settingButton = GetObject((int)GameObjects.SettingButton);
            AddUIEvent(settingButton, OnClickSettingButton, Define.UIEvent.Click);
            AddButtonAnim(settingButton);

            helpButton = GetObject((int)GameObjects.HelpButton);
            AddUIEvent(helpButton, OnClickHelpButton, Define.UIEvent.Click);
            AddButtonAnim(helpButton);

            creditButton = GetObject((int)GameObjects.CreditButton);
            AddUIEvent(creditButton, OnClickCreditButton, Define.UIEvent.Click);
            AddButtonAnim(creditButton);

            exitButton = GetObject((int)GameObjects.ExitButton);
            AddUIEvent(exitButton, OnClickExitButton, Define.UIEvent.Click);
            AddButtonAnim(exitButton);
        }

        private void OnClickNewGameButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_SlotSelectPopup>();
            /*
            GameManager.InGameSceneMode = GameManager.inGameSceneMode.NewGame;
            SceneManager.LoadScene("InGameScene");
            */
        }

        private void OnClickContinueButton(PointerEventData evt)
        {
            GameManager.InGameSceneMode = GameManager.inGameSceneMode.LoadGame;
            SceneManager.LoadScene("InGameScene");
        }

        private void OnClickSettingButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_Setting>();
        }

        private void OnClickHelpButton(PointerEventData evt)
        {

        }

        private void OnClickCreditButton(PointerEventData evt)
        {

        }

        private void OnClickExitButton(PointerEventData evt)
        {
            Application.Quit();
        }
    }
}
