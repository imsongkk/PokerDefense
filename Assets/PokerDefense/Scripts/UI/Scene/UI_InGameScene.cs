using UnityEngine;
using TMPro;
using PokerDefense.Managers;
using PokerDefense.Utils;
using UnityEngine.EventSystems;
using PokerDefense.UI.Popup;
using System.Collections;
using UnityEngine.UI;
using PokerDefense.Data;
using System.Collections.Generic;

namespace PokerDefense.UI.Scene
{
    public class UI_InGameScene : UI_Scene
    {
        enum GameObjects
        {
            MenuButton,
            Bottom,

            MeteoSkillButton,
            TimeStopSkillButton,
            FireHoleSkillButton,
            EarthQuakeSkillButton,

            HeartText,
            GoldText,
            RoundText,
            ChanceText,
        }

        TextMeshProUGUI heartText, goldText, roundText, chanceText;
        GameObject bottomUIObject;

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));
            BindObject();

            GameManager.Round.SetUIIngameScene(this);
        }

        private void BindObject()
        {
            heartText = GetObject((int)GameObjects.HeartText).GetComponent<TextMeshProUGUI>();
            goldText = GetObject((int)GameObjects.GoldText).GetComponent<TextMeshProUGUI>();
            roundText = GetObject((int)GameObjects.RoundText).GetComponent<TextMeshProUGUI>();
            chanceText = GetObject((int)GameObjects.ChanceText).GetComponent<TextMeshProUGUI>();

            bottomUIObject = GetObject((int)GameObjects.Bottom);

            List<Image> coolTimeImageList = new List<Image>();

            GameObject menuButton = GetObject((int)GameObjects.MenuButton);
            AddUIEvent(menuButton, OnClickMenuButton, Define.UIEvent.Click);
            AddButtonAnim(menuButton);

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

            for(int i=0; i<coolTimeImageList.Count; i++)
            {
                int lambdaCapture = i;
                GameManager.Skill.skillStarted[lambdaCapture].AddListener((skillTime, coolTime) =>
                {
                    ShowRemainTime(skillTime);
                    StartCoroutine(ShowCoolTime(coolTimeImageList[lambdaCapture], coolTime));
                });
            }
        }

        private void ShowRemainTime(float skillTime)
        {
            if (skillTime == 0) return;
            // TODO : 인게임 화면에 지속시간 얼마나 남았는지 Bar형태로 조그맣게 띄우기
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

        private void OnClickSkill(int skillIndex)
        {
            GameManager.Skill.SkillClicked(skillIndex);
        }

        public void ActivateBottomUI()
        {
            bottomUIObject.SetActive(true);
        }

        public void InitText(int round, int heart, int gold, int chance)
        {
            SetRoundText(round);
            SetHeartText(heart);
            SetGoldText(gold);
            SetChanceText(chance);
        }

        public void SetHorseIndex(int index)
        {
            GameManager.Horse.RunHorse(index);
            GameManager.Round.BreakState();
        }

        public void SetRoundText(int round)
            => roundText.text = $"Round : {round}";
        public void SetHeartText(int count)
            => heartText.text = count.ToString();
        public void SetGoldText(int count)
            => goldText.text = count.ToString();
        public void SetChanceText(int count)
            => chanceText.text = count.ToString();
    }
}
