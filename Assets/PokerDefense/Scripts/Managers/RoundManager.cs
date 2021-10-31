using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    enum State
    {
        NONE,
        STOP,
        PLAY,
        POKER,
    }

    State state;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TextMeshPro heartText, goldText;
    public int Round { get; private set; } = 1;
    private int heart = 5;
    private int gold = 10;
    public int Heart
    {
        get => heart;
        private set
        {
            heart = value;
            heartText.text = heart.ToString();
        }
    }
    public int Gold
    {
        get => gold;
        private set
        {
            gold = value;
            goldText.text = gold.ToString();
        }
    }

    private void Start()
        => Init();

    private void Init()
    {
        heartText.text = heart.ToString();
        goldText.text = gold.ToString();
    }

    private void Update()
    {
        heart--;
    }
}
