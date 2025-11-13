using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class OrderHandling : MonoBehaviour
{
    public static OrderHandling Instance { get; private set; }
    public Queue<Card> CardsToPlay = new Queue<Card>();

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
    }

    public void SetupCardOrder(int currentPlayerTurn)
    {
        ClearCardOrder();
        
        if (CardHandling.Instance == null)
        {
            Debug.LogWarning("CardHandling.Instance is null, cannot determine card order");
            return;
        }

        List<Card> allCardsInPlay = new List<Card>();
        
        foreach (PlayerCardState playerState in CardHandling.Instance.PlayerCards)
        {
            allCardsInPlay.AddRange(playerState.AttackSlotCards);
            allCardsInPlay.AddRange(playerState.DefenseSlotCards);
        }

        List<Card> defenseCards = new List<Card>();
        List<Card> attackCards = new List<Card>();

        foreach (Card card in allCardsInPlay)
        {
            if (card != null && card.MyCardSlot != null)
            {
                if (card.MyCardSlot.SlotType == CardSlotType.Defense)
                {
                    defenseCards.Add(card);
                }
                else if (card.MyCardSlot.SlotType == CardSlotType.Attack)
                {
                    attackCards.Add(card);
                }
            }
        }

        // Sort defense cards by player index
        defenseCards = defenseCards.OrderBy(card => card.MyCardSlot.PlayerIndex).ToList();

        // Filter and sort attack cards: only those targeting current player, ordered by player index
        attackCards = attackCards
            .Where(card => card.MyCardSlot.PlayerTargetingIndex == currentPlayerTurn)
            .OrderBy(card => card.MyCardSlot.PlayerIndex)
            .ToList();

        // Enqueue defense cards first
        foreach (Card card in defenseCards)
        {
            CardsToPlay.Enqueue(card);
            Debug.Log($"Queued Defense Card from Player {card.MyCardSlot.PlayerIndex}: {card.type}");
        }

        // Then enqueue attack cards
        foreach (Card card in attackCards)
        {
            CardsToPlay.Enqueue(card);
            Debug.Log($"Queued Attack Card from Player {card.MyCardSlot.PlayerIndex} targeting Player {card.MyCardSlot.PlayerTargetingIndex}: {card.type}");
        }

        Debug.Log($"Total cards queued for play: {CardsToPlay.Count}");
    }

    public void ClearCardOrder()
    {
        CardsToPlay.Clear();
    }

    public Card GetNextCardToTrigger()
    {
        if (CardsToPlay.Count == 0)
        {
            return null;
        }
        return CardsToPlay.Dequeue();
    }
}
