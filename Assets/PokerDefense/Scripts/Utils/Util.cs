using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Random;

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
        /* FindChild : go의 자식들 중에서 T 컴포넌트를 가지며 name과 이름이 일치하는 
         *             recursive가 false일 때는 go의 직속 자식들 중에서 T 컴포넌트를 가진 자식을 찾음
         *             recursive가 true일 때는 go의 모든 자식들 중에서 T 컴포넌트를 가진 자식을 찾음
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

        public static Vector2 GetNearFourDirection(Vector2 target)
        {
            float rightAngle = Vector2.SignedAngle(Vector2.right, target);
            float leftAngle= Vector2.SignedAngle(Vector2.left, target);
            float upAngle = Vector2.SignedAngle(Vector2.up, target);
            float downAngle = Vector2.SignedAngle(Vector2.down, target);

            if (rightAngle <= 45 && rightAngle >= -45) return Vector2.right;
            else if (leftAngle <= 45 && leftAngle >= -45) return Vector2.left;
            else if (upAngle <= 45 && upAngle >= -45) return Vector2.up;
            else if (downAngle <= 45 && downAngle >= -45) return Vector2.down;

            throw new Exception();
        }

        public static Vector2 GetNearFourDirection(Vector2 from, Vector2 to)
            => GetNearFourDirection(to - from);

        public static Vector2 GetNearTwoDirection(Vector2 target)
        {
            float rightAngle = Vector2.SignedAngle(Vector2.right, target);
            float leftAngle = Vector2.SignedAngle(Vector2.left, target);

            if (rightAngle <= 90 && rightAngle >= -90) return Vector2.right;
            else if (leftAngle < 90 && leftAngle > -90) return Vector2.left;

            throw new Exception();
        }

        public static Vector2 GetNearTwoDirection(Vector2 from, Vector2 to)
            => GetNearTwoDirection(to - from);

        public static void RunHorse(Action<int, int[]> OnMatchOver)
        {
            const int randomNumberCount = 120;
            const int horseCount = 4;

            int[,] horses = new int[horseCount, randomNumberCount];
            int[] horseSum = new int[horseCount]; // [120, 1200]
            int winnerIndex = 0;

            for (int i = 0; i < horseCount; i++)
            {
                for (int j = 0; j < randomNumberCount; j++)
                {
                    horses[i, j] = Range(1, 11); // [1, 10]
                    horseSum[i] += horses[i, j];
                }
            }

            for (int i = 0; i < horseCount; i++)
            {
                if (horseSum[i] > horseSum[winnerIndex])
                    winnerIndex = i;
            }

            OnMatchOver.Invoke(winnerIndex, horseSum);
        }

        /*
        public static IEnumerator Fade(float fadeTime, bool isOut, Action callback)
        {
            // isOut이 True면 alpha 감소
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
