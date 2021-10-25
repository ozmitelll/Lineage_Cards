﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackedHero : MonoBehaviour, IDropHandler
{
    public enum HeroType
    {
        PLAYER,
        ENEMY
    }

    public HeroType Type;
    public Color NormalCol, TargetCol;

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManagerScr.Instance.IsPlayerTurn)
            return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (card &&
            card.Card.CanAttack &&
            Type == HeroType.ENEMY)
        {
            GameManagerScr.Instance.DamageHero(card, true);
        }
    }
    public void HighlightAsTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ? TargetCol : NormalCol;
    }
}
