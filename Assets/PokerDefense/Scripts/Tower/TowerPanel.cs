using UnityEngine;
using PokerDefense.Utils;

namespace PokerDefense.Towers
{
    public class TowerPanel : GameObjectEventHandler
    {
        SpriteRenderer spriteRenderer;

        bool isInit = false;
        int xIndex, yIndex;
        float x, y;

        Tower tower;

        private void Update()
        {
            if (!isInit) return;
        }

        public void InitTowerPanel(int x, int y)
        {
            xIndex = x;
            yIndex = y;
            AddObjectEvent(gameObject, (a) => Debug.Log("Touched"), Define.MouseEvent.Click);

            isInit = true;
        }

        public void SetTower(Tower target)
            => tower = target;
    }
}