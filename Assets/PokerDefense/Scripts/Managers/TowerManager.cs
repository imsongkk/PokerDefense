using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Towers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

        private void InitTowerPanels()
        {
            towerPanelsObject = GameObject.FindGameObjectWithTag("TowerPanels");
            if (towerPanelsObject == null)
            {
                Debug.LogError($"towerPanelsObject not found");
                return;
            }

            towerPanelArray = new TowerPanel[TOWER_AREA_HEIGHT, TOWER_AREA_WITDH];

            for(int i=0; i<TOWER_AREA_HEIGHT; i++)
            {
                for(int j=0; j<TOWER_AREA_WITDH; j++)
                {
                    GameObject towerPanelObject = GameManager.Resource.Instantiate($"Tile/TowerPanel", towerPanelsObject.transform);
                    TowerPanel towerPanel = towerPanelObject.AddComponent<TowerPanel>();
                    towerPanel.InitTowerPanel(j, i);
                    towerPanelArray[i, j] = towerPanel;
                }
            }

        }

        public void BuildTower(string towerName)
        {
            if (selectedTowerPanel == null) return;

            // GameObject towerObject = GameManager.Resource.Instantiate(towerName, selectedTowerPanel.transform);
            GameObject towerObject = GameManager.Resource.Instantiate("TestTower", selectedTowerPanel.transform);
            //towerObject.AddComponent<Tower>(); 이미 Tower Component가 붙어 있게 설계 할듯?
            Tower tower = towerObject.GetComponent<Tower>();
            tower.InitTower("TestTower");
            //tower.InitTower(towerName);

            selectedTowerPanel.SetTower(tower);
        }

        public void UpgradeTower(Tower tower)
        {

        }


        public void DestroyTower(Tower tower, FastAction destroyAction)
        {
            // TODO : Destroy 성공시 action
        }

        public void SetSelectedTowerPanel(TowerPanel target)
            => selectedTowerPanel = target;
    }
}