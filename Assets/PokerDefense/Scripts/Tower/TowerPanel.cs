using UnityEngine;

namespace PokerDefense.Towers
{
    public class TowerPanel : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        GameObject towerBase;

        Tower tower;

        Color originColor;

        private void Start()
            => Init();

        private void Init()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originColor = spriteRenderer.color;
            towerBase = transform.GetChild(0).gameObject;
            towerBase.SetActive(false);
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