using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Enemies;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Towers
{
    public class FlushBuff : MonoBehaviour
    {
        private float flushBuffRange;
        private float flushBuffPercent;
        private CircleCollider2D flushBuffCollider;

        private void Awake()
        {
            flushBuffCollider = this.GetComponent<CircleCollider2D>();
        }

        public void InitFlushBuff(float range, float percent)
        {
            //* 주변 타워에 공속 버프
            this.flushBuffRange = range;
            this.transform.localScale = Vector3.one * flushBuffRange * 2;

            this.flushBuffPercent = percent;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Tower"))
            {
                other.GetComponent<Tower>().towerIndivData.AddSpeedBuff(flushBuffPercent);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Tower"))
            {
                other.GetComponent<Tower>().towerIndivData.DeleteSpeedBuff(flushBuffPercent);
            }
        }

    }
}
