using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Utils;
using PokerDefense.Managers;
using PokerDefense.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Towers
{
    public class CushionCollider : MonoBehaviour
    {
        private CircleCollider2D cushionRangeCollider;
        private Queue<Enemy> enemies = new Queue<Enemy>();
        private Enemy nextTarget;
        private float nextTargetDistance;

        private void Awake()
        {
            cushionRangeCollider = this.GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Debug.Log($"Target Entered: {collision.gameObject.ToString()}");
                enemies.Enqueue(collision.gameObject.GetComponent<Enemy>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Debug.Log($"Target Exit: {collision.gameObject.ToString()}");
                enemies.Dequeue();
            }
        }

        public Enemy NextTarget(Enemy currentTarget)
        {
            nextTarget = null;
            nextTargetDistance = 2123456789f;

            foreach (var enemy in enemies)
            {
                if (enemy != currentTarget)
                {
                    float targetDistance = Vector3.Distance(enemy.transform.position, this.transform.position);
                    if (targetDistance < nextTargetDistance)
                    {
                        nextTarget = enemy;
                        nextTargetDistance = targetDistance;
                    }
                }
                else continue;
            }
            Debug.Log($"Next Target: {nextTarget}");
            return nextTarget;
        }
    }
}