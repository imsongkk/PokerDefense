using PokerDefense.Data;
using PokerDefense.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public UnityEvent<float, float> TimeStopStarted, EarthQuakeStarted, FireHoleStarted; // ��ų ���ӽð�, ��ų ��Ÿ��
    public UnityEvent TimeStopFinished, EarthQuakeFinished, FireHoleFinished;

    public bool IsCoolTimeTimeStop 
    { 
        get => timeStopSkillData.isInCoolTime;  
        private set => timeStopSkillData.isInCoolTime = value; 
    }
    public bool IsCoolTimeEarthQuake
    {
        get => earthQuakeSkillData.isInCoolTime;
        private set => earthQuakeSkillData.isInCoolTime = value;
    }
    public bool IsCoolTimeFireHole
    {
        get => fireHoleSkillData.isInCoolTime;
        private set => fireHoleSkillData.isInCoolTime = value;
    }

    SkillData timeStopSkillData, fireHoleSkillData, earthQuakeSkillData, meteoSkillData;

    public void InitSkillManager()
    {
        InitSkillData();

        TimeStopStarted.AddListener(StartTimeStop);
        TimeStopFinished.AddListener(FinishTimeStop);

        EarthQuakeStarted.AddListener(StartEarthQuake);
        EarthQuakeFinished.AddListener(FinishEarthQuake);

        FireHoleStarted.AddListener(StartFireHole);
        FireHoleFinished.AddListener(FinishFireHole);
    }

    private void InitSkillData()
    {
        GameManager.Data.SkillDataDict.TryGetValue("TimeStop", out timeStopSkillData);
        GameManager.Data.SkillDataDict.TryGetValue("FireHole", out fireHoleSkillData);
        GameManager.Data.SkillDataDict.TryGetValue("EarthQuake", out earthQuakeSkillData);
        GameManager.Data.SkillDataDict.TryGetValue("Meteo", out meteoSkillData);
    }

    public void UseTimeStopSkill()
    {
        // TODO : ������ ��������
        float skillTime = timeStopSkillData.skillTime;
        float coolTime = timeStopSkillData.coolTime;
        TimeStopStarted.Invoke(skillTime, coolTime);
    }

    public void UseMeteoSkill()
    {

    }

    public void UseEarthQuakeSkill()
    {

    }

    public void UseFireHoleSkill()
    {

    }

    IEnumerator SetTimer(float time, UnityEvent FinishEvent)
    {
        yield return new WaitForSeconds(time);
        FinishEvent?.Invoke();
    }

    IEnumerator SetTimer(float time, Action FinishAction)
    {
        yield return new WaitForSeconds(time);
        FinishAction?.Invoke();
    }

    private void StartTimeStop(float skillTime, float coolTime)
    {
        StartCoroutine(SetTimer(skillTime, TimeStopFinished));
        StartCoroutine(SetTimer(coolTime, () => { IsCoolTimeTimeStop = false; }));

        IsCoolTimeTimeStop = true;
        // TODO : �ð��� ���߾��ٴ� UI �۾�
        // TODO : UI_InGameScene�� timeText Ȧ��
    }

    private void FinishTimeStop()
    {

    }

    private void StartEarthQuake(float skillTime, float coolTime)
    {
        StartCoroutine(SetTimer(skillTime, EarthQuakeFinished));

        // TODO : ���� ���� ó�� grid ����
        // TODO : ������ ��������
        float earthQuakeSlow = 100f;
    }

    private void FinishEarthQuake()
    {

    }

    private void StartFireHole(float skillTime, float coolTime)
    {
        StartCoroutine(SetTimer(skillTime, FireHoleFinished));

        // TODO : ���� ���� ó�� grid ����
        // TODO : ������ ��������
        /*
        float earthQuakeSlow = 100f;
        foreach (var enemy in enemies)
        {
            enemy.OnSlow(time, earthQuakeSlow);
        }
        */
    }

    private void FinishFireHole()
    {

    }
}
