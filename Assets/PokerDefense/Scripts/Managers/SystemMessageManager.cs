using PokerDefense.UI.Scene;
using PokerDefense.Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PokerDefense.Managers
{
    public class SystemMessageManager : MonoBehaviour
    {
        Transform messageUI;

        bool isInit = false;

        private void Update()
        {
            if (!isInit) return;
            // TODO : Mono로 할지 안할지 결정
            // TODO : 메세지 별 보여주는 시간 설정 할건지?

        }

        public void InitSystemTextManager(UI_InGameScene uI_InGameScene)
        {
            Transform systemMessageUIObject = uI_InGameScene.GetSystemMessageUIObject();
            isInit = true;
            messageUI = systemMessageUIObject;
        }

        public void SetSystemMessage(Define.SystemMessage messageType)
        {
            GameManager.Data.SystemMessageDict.TryGetValue(messageType.ToString(), out var messageText);

            Transform message = GameManager.Resource.Instantiate("UI/UI_SystemMessage").transform;
            TextMeshProUGUI text = message.GetComponentInChildren<TextMeshProUGUI>();
            Image backgroundImage = message.GetComponent<Image>();

            text.text = messageText;

            message.SetParent(messageUI);
            message.SetAsLastSibling();

            StartCoroutine(ShowText(backgroundImage, text, 2f));
        }

        IEnumerator ShowText(Image backgroundImage, TextMeshProUGUI text, float remaintime)
        {
            Color backgroundColor = backgroundImage.color;
            Color textColor = text.color;

            float originBackgroundAlpha = backgroundColor.a;
            float originTextAlpha = textColor.a;

            float time = 0f;

            while (time < remaintime)
            {
                backgroundImage.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, originBackgroundAlpha * (1 - (time / remaintime)));
                text.color = new Color(textColor.r, textColor.g, textColor.b, originTextAlpha * (1 - (time / remaintime)));
                time += Time.deltaTime;
                yield return null;
            }

            DestroyMessage(backgroundImage.gameObject);
        }

        private void DestroyMessage(GameObject target)
        {
            // TODO : 만약 메세지마다 띄워지는 시간이 다르다면 Destroy할 때 위에 있는 메세지 위치 이동시키기
            Destroy(target);
        }
    }
}
