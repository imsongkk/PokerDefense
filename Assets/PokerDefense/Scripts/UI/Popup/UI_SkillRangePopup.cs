using UnityEngine;
using PokerDefense.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PokerDefense.Data;
using PokerDefense.Managers;
using System;

namespace PokerDefense.UI.Popup
{
    public class UI_SkillRangePopup : UI_Popup
    {
        enum GameObjects
        {
            ConfirmButton,
            CancelButton,

            SkillRangeCircle,
        }

        Image skillRangeCircleImage;
        GameObject skillRangeCircle;

        Action<Vector2> OnConfirmButton;

        Vector2 circleSpacePos;

        private void Awake()
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
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);

            GameObject cancelButton = GetObject((int)GameObjects.CancelButton);
            AddUIEvent(cancelButton, (a) => ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(cancelButton);

            skillRangeCircle = GetObject((int)GameObjects.SkillRangeCircle);
            AddUIEvent(skillRangeCircle, OnDragSkillRangeCircle, Define.UIEvent.Drag);

            skillRangeCircleImage = skillRangeCircle.GetComponent<Image>();

            circleSpacePos = skillRangeCircle.transform.position;
        }

        public void InitSkillRangePopup(int skillIndex, Action<Vector2> OnConfirmButton)
        {
            GameManager.Data.SkillDataDict.TryGetValue(skillIndex, out var skillData);
            float ratio = (Screen.width / (float)Screen.height);
            skillRangeCircle.GetComponent<RectTransform>().sizeDelta *= ratio;
            skillRangeCircleImage.transform.localScale = new Vector3(2 * skillData.skillRange, 2 * skillData.skillRange, 0);
            this.OnConfirmButton = OnConfirmButton;
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            OnConfirmButton?.Invoke(circleSpacePos);
            ClosePopupUI();
        }

        private void OnDragSkillRangeCircle(PointerEventData evt)
        {
            circleSpacePos = new Vector2(
                skillRangeCircle.transform.position.x + evt.delta.x,
                skillRangeCircle.transform.position.y + evt.delta.y);

            // TODO : UI_InGameScene의 높이 정보 가져와서 초기화 지금은 그냥 대충
            circleSpacePos.x = Mathf.Clamp(circleSpacePos.x, 0, Screen.width);
            circleSpacePos.y = Mathf.Clamp(circleSpacePos.y, 600, Screen.height - 150);
            skillRangeCircle.transform.position = circleSpacePos;
        }
    }
}
