using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillManager : MonoBehaviour
{
    public UnityEvent<float> TimeStopStarted, EarthQuakeStarted, FireHoleStarted;
    public UnityEvent TimeStopFinished, EarthQuakeFinished, FireHoleFinished;

    public bool IsCoolTimeTimeStop { get; private set; } = false;
    public bool IsCoolTimeEarthQuake { get; private set; } = false;
    public bool IsCoolTimeFireHole { get; private set; } = false;

    public void InitSkillManager()
    {
        TimeStopStarted.AddListener(StartTimeStop);
        TimeStopFinished.AddListener(FinishTimeStop);

        EarthQuakeStarted.AddListener(StartEarthQuake);
        EarthQuakeFinished.AddListener(FinishEarthQuake);

        FireHoleStarted.AddListener(StartFireHole);
        FireHoleFinished.AddListener(FinishFireHole);
    }

    public void UseTimeStopSkill()
    {
        // TODO : ������ ��������
        float stopTime = 5f; 
        TimeStopStarted.Invoke(stopTime);
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

    private void StartTimeStop(float time)
    {
        StartCoroutine(SetTimer(time, TimeStopFinished));
        IsCoolTimeTimeStop = true;
        // TODO : �ð��� ���߾��ٴ� UI �۾�
        // TODO : UI_InGameScene�� timeText Ȧ��
    }

    private void FinishTimeStop()
    {
        IsCoolTimeTimeStop = false;
    }

    private void StartEarthQuake(float time)
    {
        StartCoroutine(SetTimer(time, EarthQuakeFinished));

        // TODO : ���� ���� ó�� grid ����
        // TODO : ������ ��������
        float earthQuakeSlow = 100f;
    }

    private void FinishEarthQuake()
    {

    }

    private void StartFireHole(float time)
    {
        StartCoroutine(SetTimer(time, FireHoleFinished));

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
