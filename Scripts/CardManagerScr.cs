using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public enum AbilityType
    {
        NO_ABILITY,
        INSTANT_ACTIVE,
        DOUBLE_ATTACK,
        SHIELD,
        PROVOCATION,
        REGENERATION_EACH_TURN,
        COUNTER_ATTACK
    }



    public string Name;
    public Sprite Logo;
    public int Attack, Defence, Manacost;
    public bool CanAttack;
    public bool IsPlaced;

    public List<AbilityType> Abilities;
    public bool HasAbility
    {
        get { return Abilities.Count > 0; }
    }
    public bool IsProvocation
    {
        get { return Abilities.Exists(x => x == AbilityType.PROVOCATION);}
    }

    public int TimesTookDamage;
    public int TimesDealedDamage;

    public bool IsAlive
    {
        get
        {
            return Defence > 0;
        }
    }
    public Card(string name, string logoPath, int attack, int defence, int manacost, AbilityType abilityType = 0)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defence = defence;
        Manacost = manacost;
        CanAttack = false;
        IsPlaced = false;

        Abilities = new List<AbilityType>();

        if (abilityType != 0)
            Abilities.Add(abilityType);

        TimesTookDamage = TimesDealedDamage = 0;
    }

   

    public void GetDamage(int dmg)
    {
        Defence -= dmg;
    }
}

public static class CardManager
{
    public static List<Card> AllCards = new List<Card>();
}

public class CardManagerScr : MonoBehaviour
{
    public void Awake()
    {
        CardManager.AllCards.Add(new Card("Рыцарь Шилен", "Sprites/Shillen Knight", 4, 5, 3));
        CardManager.AllCards.Add(new Card("Ремесленник", "Sprites/Crafter", 2, 7, 4));
        CardManager.AllCards.Add(new Card("Повелитель бури", "Sprites/Povelitel Buri", 5, 1, 7));
        CardManager.AllCards.Add(new Card("Берсерк", "Sprites/Berserk", 4, 2, 4));
        CardManager.AllCards.Add(new Card("Хавк", "Sprites/Sniper", 7, 1, 8));


    }
}