﻿using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    SELF_HAND,
    SELF_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}


public class Drop_p : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FieldType Type;
    public void OnDrop(PointerEventData eventData)
    {
        if (Type != FieldType.SELF_FIELD)
            return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (card && GameManagerScr.Instance.IsPlayerTurn
            && GameManagerScr.Instance.PlayerMana >= card.Card.Manacost &&
            !card.Card.IsPlaced)
        {
            
            card.Movement.DefaultParent = transform;
            card.OnCast();
           
        }
        else
        {
            GameManagerScr.Instance.source.clip = GameManagerScr.Instance.clips[1];
            GameManagerScr.Instance.source.Play();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || Type == FieldType.ENEMY_FIELD || Type == FieldType.ENEMY_HAND || Type == FieldType.SELF_HAND)
            return;


        CardMovementScript card = eventData.pointerDrag.GetComponent<CardMovementScript>();

        if (card)
            card.DefaultTempCardParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardMovementScript card = eventData.pointerDrag.GetComponent<CardMovementScript>();

        if (card && card.DefaultTempCardParent == transform)
            card.DefaultTempCardParent = card.DefaultParent;
    }
}

