using UnityEngine;
using System.Collections.Generic;

public class PlayerCardState
{
    public List<Card> AttackSlotCards = new List<Card>();
    public List<Card> DefenseSlotCards = new List<Card>();
    public List<Card> HandAttackCards = new List<Card>();
    public List<Card> HandDefenseCards = new List<Card>();
}

public class CardHandling : MonoBehaviour
{
    [Header("Dependencies")]
    [Header("Setup")]
    [Header("Traits")]
    public Queue<Card> CardsToPlay = new Queue<Card>();
    public static CardHandling Instance { get; private set; }
    public OrderHandling OrderHandling { get; private set; }

    public List<PlayerCardState> PlayerCards = new List<PlayerCardState>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        OrderHandling = GetComponent<OrderHandling>();
    }

    public void InitializePlayers(int playerCount)
    {
        PlayerCards.Clear();
        for (int i = 0; i < playerCount; i++)
        {
            PlayerCards.Add(new PlayerCardState());
        }
    }

#region "Helperss" 
    public void AddCardToAttackSlot(int playerIndex, Card card)
    {
        if (IsValidPlayerIndex(playerIndex))
        {
            PlayerCards[playerIndex].AttackSlotCards.Add(card);
        }
    }

    public void AddCardToDefenseSlot(int playerIndex, Card card)
    {
        if (IsValidPlayerIndex(playerIndex))
        {
            PlayerCards[playerIndex].DefenseSlotCards.Add(card);
        }
    }

    public void AddCardToHandAttack(int playerIndex, Card card)
    {
        if (IsValidPlayerIndex(playerIndex))
        {
            PlayerCards[playerIndex].HandAttackCards.Add(card);
        }
    }

    public void AddCardToHandDefense(int playerIndex, Card card)
    {
        if (IsValidPlayerIndex(playerIndex))
        {
            PlayerCards[playerIndex].HandDefenseCards.Add(card);
        }
    }

    public void RemoveCardFromAttackSlot(int playerIndex, Card card)
    {
        if (IsValidPlayerIndex(playerIndex))
        {
            PlayerCards[playerIndex].AttackSlotCards.Remove(card);
        }
    }

    public void RemoveCardFromDefenseSlot(int playerIndex, Card card)
    {
        if (IsValidPlayerIndex(playerIndex))
        {
            PlayerCards[playerIndex].DefenseSlotCards.Remove(card);
        }
    }

    public void RemoveCardFromHandAttack(int playerIndex, Card card)
    {
        if (IsValidPlayerIndex(playerIndex))
        {
            PlayerCards[playerIndex].HandAttackCards.Remove(card);
        }
    }

    public void RemoveCardFromHandDefense(int playerIndex, Card card)
    {
        if (IsValidPlayerIndex(playerIndex))
        {
            PlayerCards[playerIndex].HandDefenseCards.Remove(card);
        }
    }

    private bool IsValidPlayerIndex(int playerIndex)
    {
        return playerIndex >= 0 && playerIndex < PlayerCards.Count;
    }

    public PlayerCardState GetPlayerCardState(int playerIndex)
    {
        if (IsValidPlayerIndex(playerIndex))
        {
            return PlayerCards[playerIndex];
        }
        return null;
    }

    #endregion 
}
