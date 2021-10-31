using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PokerDefense.UI.Popup;
using PokerDefense.Utils;

namespace PokerDefense.UI
{
    public class UI_Button : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        bool isClicked = false;
        public bool HasAnim { get; set; } = true;

        RectTransform rectTransform;

        Vector3 originScale;
        public enum BUTTONTYPE
        {
            Button,
        }

        public BUTTONTYPE buttonType;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            originScale = rectTransform.localScale;
        }

        /*
        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<Text>(typeof(Texts));
            Bind<GameObject>(typeof(GameObject));
            Bind<Image>(typeof(Image));
            GetButton((int)Buttons.PointButton).gameObject.AddUIEvent(OnPointerClick);
            /*
            GameObject go = GetImage((int)Images.Icon).gameObject;
            AddUIEvent(go, (PointerEventData eventData) =>
            {
                go.transform.position = eventData.position;
            },
            Define.UIEvent.Drag);
        }
        */

        private void Update()
        {
            //if(Input.GetMouseButton(0))
            if (HasAnim)
            {
                if (isClicked)
                    rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, originScale * 1.05f, 0.5f);
                else
                    rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, originScale, 0.5f);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // 좌클릭만 처리
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            isClicked = true;
            // TODO : 버튼 사운드 처리
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // 좌클릭만 처리
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            isClicked = false;
        }
    }
}
