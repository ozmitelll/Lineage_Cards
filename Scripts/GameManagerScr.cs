using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Game
{
    public List<Card> EnemyDeck, PlayerDeck,
                      EnemyHand, PlayerHand,
                      EnemyField, PlayerField;

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();

        EnemyHand = new List<Card>();
        PlayerHand = new List<Card>();

        EnemyField = new List<Card>();
        PlayerField = new List<Card>();
    }

    List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < 10; i++)
            list.Add(CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)]);
        return list;
    }
}


public class GameManagerScr : MonoBehaviour
{
    public static GameManagerScr Instance;


    [Header("Settings")]
    public Game CurrentGame;
    public Transform EnemyHand, PlayerHand,
                     EnemyField, PlayerField;
    public GameObject CardPref;
    int Turn, TurnTime = 30;
    public TextMeshProUGUI TurnTimeTxt;
    public Button EndTurnBtn;

    public int PlayerMana, EnemyMana;
    public TextMeshProUGUI PlayerManaTxt, EnemyManaTxt;

    public int PlayerHP, EnemyHP;
    public TextMeshProUGUI PlayerHPtxt, EnemyHPtxt;

    public AudioSource source;
    public AudioClip[] clips;

    public GameObject ResultGO;
    public TextMeshProUGUI ResultTxt;

    public AttackedHero EnemyHero, PlayerHero;

    public List<CardController> PlayerHandCards = new List<CardController>(),
                            PlayerFieldCards = new List<CardController>(),
                            EnemyHandCards = new List<CardController>(),
                            EnemyFieldCards = new List<CardController>();

    public bool IsPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        StartGame();
    }

    void GiveHandCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while (i++ < 4)
            GiveCardToHand(deck, hand);
    }
    void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0)
            return;

        CreateCardPref(deck[0], hand);
        deck.RemoveAt(0);
    }

    void CreateCardPref(Card card, Transform hand)
    {
        GameObject cardGo = Instantiate(CardPref, hand, false);
        CardController cardController = cardGo.GetComponent<CardController>();

        cardController.Init(card, hand == PlayerHand);

        if (cardController.IsPlayerCard)
            PlayerHandCards.Add(cardController);
        else
            EnemyHandCards.Add(cardController);
    }

    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        TurnTimeTxt.text = TurnTime.ToString();

        foreach (var card in PlayerFieldCards)
            card.Info.HighlightCard(false);

        CheckCardsForAvalibility();

        if (IsPlayerTurn)
        {
            foreach (var card in PlayerFieldCards)
            {
                card.Card.CanAttack = true;
                card.Info.HighlightCard(true);
            }
            while (TurnTime-- > 0)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                yield return new WaitForSeconds(1);
            }
            ChangeTurn();

        }
        else
        {
            foreach (var card in EnemyFieldCards) 
                card.Card.CanAttack = true;
                
           
                StartCoroutine(EnemyTurn(EnemyHandCards));
            
        }
    }
    IEnumerator EnemyTurn(List<CardController> cards)
    {
        yield return new WaitForSeconds(1);

        int count = cards.Count == 1 ? 1 : Random.Range(0, cards.Count);

        for (int i = 0; i < count; i++)
        {
            if (EnemyFieldCards.Count > 5 ||
                EnemyMana == 0 || EnemyHandCards.Count == 0)
                break;

            List<CardController> cardsList = cards.FindAll(x => EnemyMana >= x.Card.Manacost);

            if (cardsList.Count == 0)
                break;

            cardsList[0].GetComponent<CardMovementScript>().MoveToField(EnemyField);



            yield return new WaitForSeconds(.51f);

            cardsList[0].Info.ShowCardInfo();
            cardsList[0].transform.SetParent(EnemyField);

            cardsList[0].OnCast();
        }

        yield return new WaitForSeconds(1);

        foreach (var activeCard in EnemyFieldCards.FindAll(x => x.Card.CanAttack))
        {
            if (Random.Range(0, 2) == 0 &&
                PlayerFieldCards.Count > 0)
            {

                var enemy = PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)];

                Debug.Log(activeCard.Card.Name + " (" + activeCard.Card.Attack + ";" + activeCard.Card.Defence + ")" + "---> " +
                    enemy.Card.Name + " (" + enemy.Card.Attack + ";" + enemy.Card.Defence + " )");

                activeCard.Card.CanAttack = false;

                activeCard.GetComponent<CardMovementScript>().MoveToTarget(enemy.transform);
                yield return new WaitForSeconds(.75f);
                CardsFight(enemy, activeCard);
            }
            else
            {
                activeCard.Card.CanAttack = false;

                activeCard.GetComponent<CardMovementScript>().MoveToTarget(PlayerHero.transform);
                yield return new WaitForSeconds(.75f);
                DamageHero(activeCard, false);
            }
            yield return new WaitForSeconds(.2f);
        }


        yield return new WaitForSeconds(1); 
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        StopAllCoroutines();
        Turn++;

        EndTurnBtn.interactable = IsPlayerTurn;

        if (IsPlayerTurn)
        {
            GiveNewCards();

            PlayerMana = EnemyMana = 10;
            ShowMana();
        }
        StartCoroutine(TurnFunc());
    }
    void GiveNewCards()
    {
        GiveCardToHand(CurrentGame.EnemyDeck, EnemyHand);
        GiveCardToHand(CurrentGame.PlayerDeck, PlayerHand);
    }

    public void CardsFight(CardController attacker, CardController defender)
    {
        source.clip = clips[0];
        source.Play();
        defender.Card.GetDamage(attacker.Card.Attack);
        attacker.OnDamageDeal();
        defender.OnTakeDamage(attacker);

        attacker.Card.GetDamage(defender.Card.Attack);
        attacker.OnTakeDamage();

        attacker.CheckForAlive();
        defender.CheckForAlive();
        
    }

  


    void ShowMana()
    {
        PlayerManaTxt.text = PlayerMana.ToString();
        EnemyManaTxt.text = EnemyMana.ToString();
    }
    void ShowHP()
    {
        PlayerHPtxt.text = PlayerHP.ToString();
        EnemyHPtxt.text = EnemyHP.ToString();
    }
    public void ReduceMana(bool playerMana, int manacost)
    {
        if (playerMana)
            PlayerMana = Mathf.Clamp(PlayerMana - manacost, 0, int.MaxValue);
        else
            EnemyMana = Mathf.Clamp(EnemyMana - manacost, 0, int.MaxValue);

        ShowMana();
    }

    public void DamageHero(CardController card, bool isEnemyAttacked)
    {
        if (isEnemyAttacked)
            EnemyHP = Mathf.Clamp(EnemyHP - card.Card.Attack, 0, int.MaxValue);
        else
            PlayerHP = Mathf.Clamp(PlayerHP - card.Card.Attack, 0, int.MaxValue);

        ShowHP();
        card.OnDamageDeal();
        CheckForResult();
    }


    void CheckForResult()
    {
        if (EnemyHP == 0 || PlayerHP == 0)
        {
            ResultGO.SetActive(true);
            StopAllCoroutines();
            if (EnemyHP == 0)
            {
                GameObject.Find("Camera").GetComponent<AudioSource>().Stop();
                ResultTxt.text = "Победа, ГЦ!";
                source.clip = clips[2];
                source.Play();
            }
            else
            {
                GameObject.Find("Camera").GetComponent<AudioSource>().Stop();
                ResultTxt.text = "Ничего , в следующий раз обязательно победишь!";
            }
        }

    }

    public void CheckCardsForAvalibility()
    {
        foreach (var card in PlayerHandCards)
            card.Info.HighLightManaAvaliability(PlayerMana);
    }

    public void HighLightTargets(bool highlight)
    {
        foreach (var card in EnemyFieldCards)
            card.Info.HighlightAsTarget(highlight);

        EnemyHero.HighlightAsTarget(highlight);
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        foreach (var card in PlayerHandCards)
            Destroy(card.gameObject);
        foreach (var card in PlayerFieldCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyFieldCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyHandCards)
            Destroy(card.gameObject);

        GameObject.Find("Camera").GetComponent<AudioSource>().Stop();
        GameObject.Find("Camera").GetComponent<AudioSource>().time = 0f;
        PlayerHandCards.Clear();
        PlayerFieldCards.Clear();
        EnemyHandCards.Clear();
        EnemyFieldCards.Clear();

        StartGame();
    }
    public void StartGame()
    {
        EndTurnBtn.interactable = true;
        PlayerMana = EnemyMana = 10;
        PlayerHP = EnemyHP = 30;
        Turn = 0;
        StartCoroutine(TurnFunc());

        CurrentGame = new Game();
        GiveHandCards(CurrentGame.EnemyDeck, EnemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, PlayerHand);
        ShowMana();
        ShowHP();
        ResultGO.SetActive(false);
        source.Play();
        GameObject.Find("Camera").GetComponent<AudioSource>().Play();
    }

}
