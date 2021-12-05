using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Towers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PokerDefense.Managers.RoundManager;

namespace PokerDefense.Managers
{
    public class TowerManager
    {
        public enum TowerType
        {
            Spade,
            Clover,
            Diamond,
            Heart,
        }

        readonly private int TOWER_AREA_WITDH = 8;
        readonly private int TOWER_AREA_HEIGHT = 8;

        TowerPanel[,] towerPanelArray;
        TowerPanel selectedTowerPanel = null;

        GameObject towerPanelsObject;

        public void InitTowerManager()
        {
            InitTowerPanels();
        }
        /* 테스트 */

        private void InitTowerPanels()
        {
            towerPanelsObject = GameObject.FindGameObjectWithTag("TowerPanels");
            if (towerPanelsObject == null)
            {
                Debug.LogError($"towerPanelsObject not found");
                return;
            }

            towerPanelArray = new TowerPanel[TOWER_AREA_HEIGHT, TOWER_AREA_WITDH];

            for (int i = 0; i < TOWER_AREA_HEIGHT; i++)
            {
                for (int j = 0; j < TOWER_AREA_WITDH; j++)
                {
                    GameObject towerPanelObject = GameManager.Resource.Instantiate($"Tile/TowerPanel", towerPanelsObject.transform);
                    TowerPanel towerPanel = towerPanelObject.AddComponent<TowerPanel>();
                    towerPanel.InitTowerPanel(j, i);
                    towerPanelArray[i, j] = towerPanel;
                }
            }

        }

        private void BuildTower() // �̹� ��Ŀ �и� ���� �����̱� ������, �̿� �´� TowerData�� �����Ǿ�����(PokerManager���� ������)
        {
            // TODO : PokerManager���� ��Ŀ �п� �´� Tower ���� ��������
            if (selectedTowerPanel == null) return;

            GameObject towerObject = GameManager.Resource.Instantiate("TestTower", selectedTowerPanel.transform);
            Tower tower = towerObject.GetComponent<Tower>();
            tower.InitTower("TestTower");

            selectedTowerPanel.SetTower(tower);

            // Ÿ�� �Ǽ� ������ ���� ����
            GameManager.Round.BuildTower();
        }

        public void UpgradeTower(Tower tower)
        {

        }


        public void DestroyTower(Tower tower, FastAction destroyAction)
        {
            // TODO : Destroy ������ action
        }

        public void SetSelectedTowerPanel(TowerPanel target)
        {
            selectedTowerPanel = target;
            BuildTower();
        }
    }
}