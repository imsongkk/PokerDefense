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

        List<TowerPanel> towerPanelList = new List<TowerPanel>();
        List<Tower> userTowerList = new List<Tower> ();
        TowerPanel selectedTowerPanel = null;

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

            for(int i=0; i<towerPanelsObject.transform.childCount; i++)
            {
                TowerPanel towerPanel = towerPanelsObject.transform.GetChild(i).GetComponent<TowerPanel>();
                towerPanelList.Add(towerPanel);
            }
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

            TowerType towerType;
            if (hand.TopShape == CardShape.Joker) towerType = TowerType.Joker;
            else if (hand.TopShape == CardShape.Null) towerType = TowerType.Normal;
            else if ((hand.TopShape == CardShape.Spade) || (hand.TopShape == CardShape.Clover))
                towerType = TowerType.Black;
            else towerType = TowerType.Red;

            HandRank handRank = hand.Rank;
            string towerName = handRank.ToString();

            Tower tower = GetTowerObject(towerName);
            if (tower == null)
            {
                Debug.LogError("Build Tower Error");
                return;
            }

            selectedTowerPanel.SetTower(tower);
            tower.InitTower(towerName, towerType, hand.TopCard, selectedTowerPanel.Index);

            userTowerList.Add(tower);
        }

        private Tower GetTowerObject(string towerName)
        {
            GameObject towerObject = GameManager.Resource.Instantiate($"TowerPrefabs/{towerName}", selectedTowerPanel.transform);
            return towerObject.GetComponent<Tower>();
        }

        public List<Tower> GetUserTowerList()
        {
            RefreshUserTowerList();
            return userTowerList;
        }

        private void RefreshUserTowerList()
        {
            List<Tower> ret = new List<Tower>();
            foreach (var a in userTowerList)
            {
                if (a == null) continue;
                ret.Add(a);
            }
            userTowerList = ret;
        }

        public void DestroyTower(Tower tower, Action afterDestroyAction)
        {
            UnityEngine.Object.Destroy(tower.gameObject);
            afterDestroyAction?.Invoke();
        }

        public void UpgradeTower(Tower tower)
        {
            // TODO : 타워 업그레이드 짜기
        }

        public void AfterTowerBaseConstructed(TowerPanel target)
        {
            EndTowerPanelSelect(target);

            // TowerBase 건설된 TowerPanel 저장
            selectedTowerPanel = target; 

            // TowerBase 건설 성공시 라운드 시작
            GameManager.Round.BreakState();
        }

        public void StartTowerPanelSelect(TowerPanel target) // 포커 패를 뽑기 전, Tower의 위치 선정
        {
            Tower tower = target.GetTower();
            tower?.HighlightRangeCircle();
            target.HighlightPanel();

            GameManager.Round.BreakTimer(true);
        }

        public void EndTowerPanelSelect(TowerPanel target) // TowerPanel 선택이 다 됐거나 취소했을 경우
        {
            Tower tower = target.GetTower();
            tower?.ResetRangeCircle();
            target.ResetPanel();

            GameManager.Round.BreakTimer(false);
        }

    }
}