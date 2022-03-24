using UnityEngine;
using TMPro;
using PokerDefense.Managers;
using PokerDefense.Utils;
using UnityEngine.EventSystems;
using PokerDefense.UI.Popup;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace PokerDefense.UI.Scene
{
    public class UI_InGameScene : UI_Scene
    {
        enum GameObjects
        {
            MenuButton,
            Bottom,
            RemainUI,
            SystemMessageUI,

            MeteoSkillButton,
            TimeStopSkillButton,
            FireHoleSkillButton,
            EarthQuakeSkillButton,

            ReadyButton,
            ShopButton,

            HeartText,
            GoldText,
            RoundText,
            ChanceText,
            CurrentRoundCountText,
            DiedEnemyCountText,
            ElapsedTimeCountText,

            UserItemSlots,
        }

        TextMeshProUGUI heartText, goldText, roundText, chanceText, currentRoundCountText, diedEnemyCountText, elapsedTimeCountText;
        GameObject bottomUIObject;
        Transform remainUiObject, systemMessageUIObject;
        UI_UserItemSlots userItemSlots;

        List<Image> coolTimeImageList = new List<Image>();

        private void Awake()
            => Init();

        private void Start()
            => InitUI();

        public override void Init()
        {
            base.Init();

            BindObject();
        }

        private void InitUI()
        {
            InitCoolTimeImage();
            InitUserItemSlots();
        }

        private void InitCoolTimeImage()
        {
            for (int i = 0; i < coolTimeImageList.Count; i++)
            {
                int lambdaCapture = i;
                InGameManager.Skill.skillStarted[lambdaCapture].AddListener((remainTime, coolTime) =>
                {
                    ShowRemainTime(remainTime);
                    StartCoroutine(ShowCoolTime(coolTimeImageList[lambdaCapture], coolTime));
                });
            }
        }

        private void InitUserItemSlots()
        {
            userItemSlots.InitItemSlotsUI();
        }

        private void BindObject()
        {
            Bind<GameObject>(typeof(GameObjects));

            heartText = GetObject((int)GameObjects.HeartText).GetComponent<TextMeshProUGUI>();
            goldText = GetObject((int)GameObjects.GoldText).GetComponent<TextMeshProUGUI>();
            roundText = GetObject((int)GameObjects.RoundText).GetComponent<TextMeshProUGUI>();
            chanceText = GetObject((int)GameObjects.ChanceText).GetComponent<TextMeshProUGUI>();
            currentRoundCountText = GetObject((int)GameObjects.CurrentRoundCountText).GetComponent<TextMeshProUGUI>();
            diedEnemyCountText = GetObject((int)GameObjects.DiedEnemyCountText).GetComponent<TextMeshProUGUI>();
            elapsedTimeCountText = GetObject((int)GameObjects.ElapsedTimeCountText).GetComponent<TextMeshProUGUI>();

            bottomUIObject = GetObject((int)GameObjects.Bottom);
            remainUiObject = GetObject((int)GameObjects.RemainUI).transform;
            systemMessageUIObject = GetObject((int)GameObjects.SystemMessageUI).transform;

            GameObject menuButton = GetObject((int)GameObjects.MenuButton);
            AddUIEvent(menuButton, OnClickMenuButton, Define.UIEvent.Click);
            AddButtonAnim(menuButton);

            GameObject readyButton = GetObject((int)GameObjects.ReadyButton);
            AddUIEvent(readyButton, OnClickReadyButton, Define.UIEvent.Click);
            AddButtonAnim(readyButton);

            GameObject shopButton = GetObject((int)GameObjects.ShopButton);
            AddUIEvent(shopButton, OnClickShopButton, Define.UIEvent.Click);
            AddButtonAnim(shopButton);

            // 0번 스킬
            GameObject timeStopButton = GetObject((int)GameObjects.TimeStopSkillButton);
            AddUIEvent(timeStopButton, (evt) => { OnClickSkill(0); }, Define.UIEvent.Click);
            AddButtonAnim(timeStopButton);
            coolTimeImageList.Add(timeStopButton.transform.GetChild(1).GetComponent<Image>());

            // 1번 스킬
            GameObject fireHoleButton = GetObject((int)GameObjects.FireHoleSkillButton);
            AddUIEvent(fireHoleButton, (evt) => { OnClickSkill(1); }, Define.UIEvent.Click);
            AddButtonAnim(fireHoleButton);
            coolTimeImageList.Add(fireHoleButton.transform.GetChild(1).GetComponent<Image>());

            // 2번 스킬
            GameObject earthQuakeButton = GetObject((int)GameObjects.EarthQuakeSkillButton);
            AddUIEvent(earthQuakeButton, (evt) => { OnClickSkill(2); }, Define.UIEvent.Click);
            AddButtonAnim(earthQuakeButton);
            coolTimeImageList.Add(earthQuakeButton.transform.GetChild(1).GetComponent<Image>());

            // 3번 스킬
            GameObject meteoButton = GetObject((int)GameObjects.MeteoSkillButton);
            AddUIEvent(meteoButton,  (evt) => { OnClickSkill(3); }, Define.UIEvent.Click);
            AddButtonAnim(meteoButton);
            coolTimeImageList.Add(meteoButton.transform.GetChild(1).GetComponent<Image>());

            userItemSlots = GetObject((int)GameObjects.UserItemSlots).GetComponent<UI_UserItemSlots>();
        }

        private void ShowRemainTime(float remainTime)
        {
            if (remainTime == 0) return;
            GameObject remainItem = GameManager.Resource.Instantiate("UI/UI_RemainItem");
            remainItem.transform.SetParent(remainUiObject);

            UI_RemainItem uI_RemainItem = remainItem.GetComponent<UI_RemainItem>();
            // TODO : skill 혹은 유저 아이템에 맞는 Image 전달해주기
            uI_RemainItem.InitUI(remainTime, null);
        }

        IEnumerator ShowCoolTime(Image targetImage, float coolTime)
        {
            float time = 0f;
            targetImage.fillAmount = 1f;

            while(time <= coolTime)
            {
                yield return null;
                targetImage.fillAmount = 1 - time / coolTime;
                time += Time.deltaTime;
            }

            targetImage.fillAmount = 0f;
        }

        private void OnClickMenuButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_InGameMenuPopup>();
        }

        private void OnClickReadyButton(PointerEventData evt)
        {
            InGameManager.Round.OnClickReadyButton();
        }

        private void OnClickShopButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_ShopPopup>();
        }

        private void OnClickSkill(int skillIndex)
        {
            InGameManager.Skill.SkillClicked(skillIndex);
        }

        public void ActivateBottomUI()
        {
            bottomUIObject.SetActive(true);
        }

        public Transform GetSystemMessageUIObject()
            => systemMessageUIObject;

        public void SetRoundText(int round)
            => roundText.text = $"Round : {round}";
        public void SetHeartText(int count)
            => heartText.text = count.ToString();
        public void SetGoldText(int count)
            => goldText.text = count.ToString();
        public void SetChanceText(int count)
            => chanceText.text = count.ToString();
        public void SetCurrentRoundCountText(int count, int entireCount)
            => currentRoundCountText.text = $"현재 웨이브 : {count} / {entireCount}";
        public void SetDiedEnemyCountText(int count, int entireCount)
            => diedEnemyCountText.text = $"처치한 몬스터 : {count} / {entireCount}";

        TimeSpan time;
        public void SetElapsedTimeCountText(int sec) // 라운드 시작 기준
        {
            time = TimeSpan.FromSeconds(sec);
            elapsedTimeCountText.text = $"라운드 경과 : {time.Minutes:D2}:{time.Seconds:D2}";
        }
    }
}
