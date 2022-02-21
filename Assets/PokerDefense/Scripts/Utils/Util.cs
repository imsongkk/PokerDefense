using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokerDefense.Utils
{
    public class Util
    {
        public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
        {
            T component = go.GetComponent<T>() ?? go.AddComponent<T>();
            return component;
        }

        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            Transform transform = FindChild<Transform>(go, name, recursive);
            if (transform == null)
                return null;

            return transform.gameObject;
        }
        /* FindChild : go�� �ڽĵ� �߿��� T ������Ʈ�� ������ name�� �̸��� ��ġ�ϴ� 
         *             recursive�� false�� ���� go�� ���� �ڽĵ� �߿��� T ������Ʈ�� ���� �ڽ��� ã��
         *             recursive�� true�� ���� go�� ��� �ڽĵ� �߿��� T ������Ʈ�� ���� �ڽ��� ã��
         * 
         * 
         */
        public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
        {
            if (go == null) { return null; }

            if (recursive == false)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    Transform transform = go.transform.GetChild(i);
                    if (string.IsNullOrEmpty(name) || transform.name == name)
                    {
                        T component = transform.GetComponent<T>();
                        if (component != null)
                            return component;
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>(true))
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        return component;
                }
            }
            return null;
        }

        public static void Stop()
        {
            Time.timeScale = 0f;
        }

        public static void Resume()
        {
            Time.timeScale = 1f;
        }

        /*
        public static IEnumerator Fade(float fadeTime, bool isOut, Action callback)
        {
            // isOut�� True�� alpha ����
            Image fadeImage = GameManager.UI.ShowFadeUI().GetComponent<Image>();

            float curTime = Time.time;
            float x = isOut ? 1 : 0;
            float y = isOut ? 0 : 1;

            while (curTime + fadeTime > Time.time)
            {
                float alpha = Mathf.Lerp(x, y, (Time.time - curTime) / fadeTime);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            callback?.Invoke();
        }
        */
    }
}
