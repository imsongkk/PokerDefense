using PokerDefense.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PokerDefense.Utils;

namespace PokerDefense.UI.Scene
{
    public class UI_LoadingScene : UI_Scene
    {
        private readonly string nextScene = "MainScene";
        AsyncOperation op;

        enum GameObjects
        {
            LoadingBar,
            StartButton,
        }

        Image loadingBar;
        GameObject startButton;

        private void Start()
            => Init();

        public override void Init()
        {
            Bind<GameObject>(typeof(GameObjects));
            StartCoroutine(Loading());
            BindObjects();
        }


        private void BindObjects()
        {
            loadingBar = GetObject((int)GameObjects.LoadingBar).GetComponent<Image>();
            startButton = GetObject((int)GameObjects.StartButton);
            AddUIEvent(startButton, OnClickStartbutton, Define.UIEvent.Click);
            AddButtonAnim(startButton);
        }

        IEnumerator Loading()
        {
            yield return null;

            op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;
            float timer = 0.0f;

            while (!op.isDone)
            {
                timer += Time.deltaTime;
                yield return new WaitForSeconds(0.1f);
                if(op.progress < 0.9f)
                {
                    loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, op.progress, timer);
                    if (loadingBar.fillAmount >= op.progress) timer = 0f;
                }
                else
                {
                    loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timer);
                    //if(Mathf.Approximately(loadingBar.fillAmount, 1f))
                    if(loadingBar.fillAmount == 1.0f)
                    {
                        LoadComplete();
                        yield break;
                    }
                }
            }
        }

        private void LoadComplete()
        {
            loadingBar.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
        }

        private void OnClickStartbutton(PointerEventData evt)
        {
            // 이 시점은 이미 Data Load가 다 끝난 상태임!
            op.allowSceneActivation = true;
        }
    }
}
