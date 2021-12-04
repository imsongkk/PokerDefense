using UnityEngine;
using PokerDefense.Utils;
using System;

namespace PokerDefense.Towers
{
    public class TowerPanel : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;

        bool isInit = false;
        int xIndex, yIndex;
        float x, y;

        Tower tower;

        Color originColor;

        private void Update()
        {
            if (!isInit) return;
        }

        public void InitTowerPanel(int x, int y)
        {
            xIndex = x;
            yIndex = y;

            spriteRenderer = GetComponent<SpriteRenderer>();
            originColor = spriteRenderer.color;

            // TODO : �ϵ��ڵ� ���� ���� �ػ� ����!
            transform.localPosition = new Vector2((float)(-3.5 + x), (float)(3.5 - y));

            isInit = true;
        }

        public void SetTower(Tower target)
        {
            tower = target;
        }

        public void HighligtPanel()
        {
            spriteRenderer.color = Color.red;
        }

        public void ResetPanel()
        {
            spriteRenderer.color = originColor;
        }

        public void OnEndPoker()
        {
            ResetPanel();
        }
    }
}