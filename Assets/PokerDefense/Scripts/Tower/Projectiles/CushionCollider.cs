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
        private List<Enemy> enemies = new List<Enemy>();
        private Enemy nextTarget;
        private float nextTargetDistance;

        private Enemy collidedEnemy;

        private void Awake()
        {
            cushionRangeCollider = this.GetComponent<CircleCollider2D>();

        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            TargetClear();
            Debug.Log("target cleared");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collidedEnemy = collision.gameObject.GetComponent<Enemy>();
                if (!(collidedEnemy.isCollided))
                {
                    Debug.Log($"OnTriggerEnter2D Called");
                    Debug.Log($"Target Entered: {collision.gameObject.ToString()}");
                    collidedEnemy.isCollided = true;
                    enemies.Add(collidedEnemy);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collidedEnemy = collision.gameObject.GetComponent<Enemy>();
                collidedEnemy.isCollided = false;
                if (enemies.Remove(collidedEnemy)) Debug.Log($"Target Exit: {collision.gameObject.ToString()}");
            }

        }

        public Enemy NextTarget(Enemy currentTarget)
        {
            nextTarget = null;
            nextTargetDistance = 2123456789f;

            enemies.Remove(currentTarget);
            foreach (var enemy in enemies)
            {
                float targetDistance = Vector3.Distance(enemy.transform.position, this.transform.position);
                if (targetDistance < nextTargetDistance)
                {
                    nextTarget = enemy;
                    nextTargetDistance = targetDistance;
                }
                else continue;
            }

            Debug.Log($"Next Target: {nextTarget}");
            return nextTarget;
        }

        private void TargetClear()
        {
            foreach (var enemy in enemies)
            {
                enemy.isCollided = false;
            }
            enemies.Clear();
        }
    }
}