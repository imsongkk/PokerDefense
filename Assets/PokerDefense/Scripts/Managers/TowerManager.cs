using PokerDefense.Towers;
using static PokerDefense.Utils.Define;
using System;
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
            Normal,
            Red,
            Black,
            Joker
        }

        readonly private int TOWER_AREA_WITDH = 8;
        readonly private int TOWER_AREA_HEIGHT = 8;

        TowerPanel[,] towerPanelArray;
        TowerPanel selectedTowerPanel = null;
        Tower currentTower;

        public void InitTowerManager()
        {
            InitTowerPanels();
        }
        /* 테스트 */

        private void InitTowerPanels()
        {
            GameObject towerPanelsObject;

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

            //속성이 없는 기본 타워 기반
            selectedTowerPanel.SetTowerBase(true);

            // 타워 건설 성공시 라운드 시작
            GameManager.Round.BreakState();
        }

        public void ConfirmTower(Hand hand)
        {
            /*
            * Tower Prefab은 족보별로 그 족보의 이름을 딴 Prefab이 존재한다
            * 공격방식 및 생김새, 기초 데이터는 해당 족보에서 가져오고,
            * 탑카드 버프 및 타입 결정은 SetTowerSettings으로 따로 결정
            */
            // TODO : PokerManager에서 포커 패에 맞는 Tower 정보 가져오기
            // TODO : selectedTower에서 타워 종류 결정
            HandRank handRank = hand.Rank;
            BuildTower(handRank.ToString());
            TowerType towerType;
            if (hand.TopShape == CardShape.Joker) towerType = TowerType.Joker;
            else if (hand.TopShape == CardShape.Null) towerType = TowerType.Normal;
            else if ((hand.TopShape == CardShape.Spade) || (hand.TopShape == CardShape.Clover))
            {
                towerType = TowerType.Black;
            }
            else towerType = TowerType.Red;

            currentTower.SetTowerSettings(towerType, hand.TopCard);

            currentTower = null;
        }

        private void BuildTower(string towerName)
        {
            if (currentTower != null) UnityEngine.Object.Destroy(currentTower.gameObject);
            GameObject towerObject = GameManager.Resource.Instantiate($"TowerPrefabs/{towerName}", selectedTowerPanel.transform);
            currentTower = towerObject.GetComponent<Tower>();
            Debug.Log(currentTower);
            currentTower.InitTower(towerName);

            selectedTowerPanel.SetTowerBase(false);
            selectedTowerPanel.SetTower(currentTower);
        }

        public void DestroyTower(Tower tower, Action destroyAction)
        {

        }

        public void SetSelectedTowerPanel(TowerPanel target)
        {
            selectedTowerPanel = target;
            SelectTowerPosition();
        }
    }
}