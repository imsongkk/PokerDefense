using UnityEngine;
using TMPro;
using PokerDefense.Managers;
using PokerDefense.Utils;
using UnityEngine.EventSystems;
using PokerDefense.UI.Popup;
using System.Collections;
using UnityEngine.UI;

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

            GameObject menuButton = GetObject((int)GameObjects.MenuButton);
            AddUIEvent(menuButton, OnClickMenuButton, Define.UIEvent.Click);
            AddButtonAnim(menuButton);

            GameObject meteoButton = GetObject((int)GameObjects.MeteoSkillButton);
            AddUIEvent(meteoButton, OnClickMeteoSkill, Define.UIEvent.Click);
            AddButtonAnim(meteoButton);

            GameObject timeStopButton = GetObject((int)GameObjects.TimeStopSkillButton);
            AddUIEvent(timeStopButton, OnClickTimeStopSkill, Define.UIEvent.Click);
            AddButtonAnim(timeStopButton);

            GameObject fireHoleButton = GetObject((int)GameObjects.FireHoleSkillButton);
            AddUIEvent(fireHoleButton, OnClickFireHoleSkill, Define.UIEvent.Click);
            AddButtonAnim(fireHoleButton);

            GameObject earthQuakeButton = GetObject((int)GameObjects.EarthQuakeSkillButton);
            AddUIEvent(earthQuakeButton, OnClickEarthQuakeSkill, Define.UIEvent.Click);
            AddButtonAnim(earthQuakeButton);

            Image meteoCoolTimeImage = meteoButton.transform.GetChild(1).GetComponent<Image>();
            Image timeStopCoolTimeImage = timeStopButton.transform.GetChild(1).GetComponent<Image>();
            Image fireHoleCoolTimeImage = fireHoleButton.transform.GetChild(1).GetComponent<Image>();
            Image earthQuakeCoolTimeImage = earthQuakeButton.transform.GetChild(1).GetComponent<Image>();

            bottomUIObject = GetObject((int)GameObjects.Bottom);

            GameManager.Skill.TimeStopStarted.AddListener((a) => { ShowCoolTime(timeStopCoolTimeImage); });
            GameManager.Skill.TimeStopStarted.AddListener((remainTime) => { ShowRemainTime(remainTime); });
        }

        private void ShowRemainTime(float targetTime)
        {
            // TODO : �ΰ��� ȭ�鿡 ���ӽð� �󸶳� ���Ҵ��� Bar���·� ���׸İ� ����
        }

        private void ShowCoolTime(Image targetImage)
        {
            // TODO : ��ų ��Ÿ�� ��������
            float coolTime = 3f;
            StartCoroutine(StartCoolTime(targetImage, coolTime));
        }

        IEnumerator StartCoolTime(Image targetImage, float coolTime)
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

        private void OnClickMeteoSkill(PointerEventData evt)
        {

        }

        private void OnClickTimeStopSkill(PointerEventData evt)
        {
            if(CheckTimeStopSkill())
                GameManager.Skill.UseTimeStopSkill();
        }

        private bool CheckTimeStopSkill()
        {
            // TODO : �ڽ�Ʈ ��������
            int timeStopCost = 5;
            if (GameManager.Round.Gold < timeStopCost)
            {
                Debug.Log("�ڽ�Ʈ ����");
                // TODO : �ڽ�Ʈ ���� UI ����
            }
            else if (GameManager.Skill.IsCoolTimeTimeStop)
            {
                Debug.Log("��Ÿ�� �Դϴ�");
                // TODO : ��Ÿ�� UI ����
            }
            else if(GameManager.Round.CurrentState != RoundManager.RoundState.PLAY)
            {
                Debug.Log("������ ��ų�� ����� �� �����ϴ�");
                // TODO : UI ����
            }
            else
                return true;
            return false;
        }

        private void OnClickFireHoleSkill(PointerEventData evt)
        {

        }

        private void OnClickEarthQuakeSkill(PointerEventData evt)
        {

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
