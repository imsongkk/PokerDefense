using UnityEngine;

namespace PokerDefense.Towers
{
    public class TowerPanel : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        GameObject towerBase;

        int xIndex, yIndex;

        Tower tower;

        Color originColor;

        public void InitTowerPanel(int x, int y)
        {
            xIndex = x;
            yIndex = y;

            spriteRenderer = GetComponent<SpriteRenderer>();
            originColor = spriteRenderer.color;
            towerBase = transform.GetChild(0).gameObject;
            towerBase.SetActive(false);

            // TODO : �ϵ��ڵ� ���� ���� �ػ� ����!
            transform.localPosition = new Vector2((float)(-3.5 + x), (float)(3.5 - y));
        }

        public void SetTower(Tower target)
        {
            tower = target;
            SetTowerBaseStatus(false);
        }

        public Tower GetTower()
            => tower;

        public void OnEndPoker()
            => ResetPanel();

        public void SetTowerBaseStatus(bool setBase)
            => towerBase.SetActive(setBase);

        public void HighligtPanel()
            => spriteRenderer.color = Color.red;

        public void ResetPanel()
            => spriteRenderer.color = originColor;

        public bool HasTower()
            => tower != null;
    }
}