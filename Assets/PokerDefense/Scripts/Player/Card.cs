using PokerDefense.Managers;
using PokerDefense.Utils;
using PokerDefense.UI.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int number;
    [SerializeField] private CardShape cardShape;
    [SerializeField] private Sprite cardSprite;

    private void InitSprite()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cardSprite;
    }

    public void InitCard(int num, CardShape shape, Sprite sprite)
    {
        this.number = num;
        this.cardShape = shape;
        this.cardSprite = sprite;
        InitSprite();
    }
}