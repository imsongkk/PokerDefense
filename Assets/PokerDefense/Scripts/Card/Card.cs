using PokerDefense.Managers;
using PokerDefense.Utils;
using PokerDefense.UI.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PokerDefense.Utils.Define;

/* Deprecated */
public class Card : MonoBehaviour
{
    private Image cardImage;
    [SerializeField] private int number;
    [SerializeField] private CardShape cardShape;
    [SerializeField] private Sprite cardSprite;

    private void InitSprite()
    {
        cardImage = gameObject.GetComponent<Image>();
        cardImage.sprite = cardSprite;
    }

    public void InitCard(int num, CardShape shape, Sprite sprite)
    {
        this.number = num;
        this.cardShape = shape;
        this.cardSprite = sprite;
        InitSprite();
    }
}