﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card Card;

    public bool IsPlayerCard;

    public CardInfoScr Info;
    public CardMovementScript Movement;

    GameManagerScr gameManager;

    public void Init(Card card, bool isPlayerCard)
    {
        Card = card;
        gameManager = GameManagerScr.Instance;
        IsPlayerCard = isPlayerCard;

        if(isPlayerCard)
        {
            Info.ShowCardInfo();
            GetComponent<AttackedCard>().enabled = false;
        }
        else
        {
            Info.HideCardInfo();
        }        
    }


    public void OnCast()
    {
        if (IsPlayerCard)
        {
            gameManager.PlayerHandCards.Remove(this);
            gameManager.PlayerFieldCards.Add(this);
            gameManager.ReduceMana(true, Card.Manacost);
            gameManager.CheckCardsForAvalibility();
        }
        else
        {
            gameManager.EnemyHandCards.Remove(this);
            gameManager.EnemyFieldCards.Add(this);
            gameManager.ReduceMana(false, Card.Manacost);
        }
        Card.IsPlaced = true;
    }
    public void OnTakeDamage(CardController attacker = null)
    {
        CheckForAlive();
    }

    public void OnDamageDeal()
    {
        Card.CanAttack = false;
        Info.HighlightCard(false);

    }

    public void CheckForAlive()
    {
        if (Card.IsAlive)
            Info.RefreshData();
        else
            DestroyCard();
    }

    public void DestroyCard()
    {
        Movement.OnEndDrag(null);

        RemoveCardFromList(gameManager.EnemyFieldCards);
        RemoveCardFromList(gameManager.EnemyHandCards);
        RemoveCardFromList(gameManager.PlayerHandCards);
        RemoveCardFromList(gameManager.PlayerFieldCards);

        Destroy(gameObject);
    }
        
    void RemoveCardFromList(List<CardController> list)
    {
        if (list.Exists(x => x == this))
            list.Remove(this);
    }

}
