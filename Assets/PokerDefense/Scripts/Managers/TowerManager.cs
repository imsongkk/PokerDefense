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

        private void SelectTowerPosition() // 포커 패를 뽑기 전, Tower의 위치 선정
        {
            if (selectedTowerPanel == null) return;

            GameObject towerObject = GameManager.Resource.Instantiate("TestTower", selectedTowerPanel.transform);
            Tower tower = towerObject.GetComponent<Tower>();
            tower.InitTower("TestTower");

            selectedTowerPanel.SetTower(tower);

            // 타워 건설 성공시 라운드 시작
            GameManager.Round.BreakState();
        }

        public void ConfirmTower(Hand hand)
        {
            // TODO : PokerManager에서 포커 패에 맞는 Tower 정보 가져오기
            // TODO : selectedTower에서 타워 종류 결정
        }


        public void DestroyTower(Tower tower, FastAction destroyAction)
        {

        }

        public void SetSelectedTowerPanel(TowerPanel target)
        {
            selectedTowerPanel = target;
            SelectTowerPosition();
        }
    }
}