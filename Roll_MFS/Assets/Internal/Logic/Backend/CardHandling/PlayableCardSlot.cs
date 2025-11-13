using UnityEngine;

public enum CardSlotType
{
    Attack,
    Defense
}

public class PlayableCardSlot : MonoBehaviour
{
    [Header("Slot Configuration")]
    public CardSlotType SlotType;
    public int PlayerIndex;
    public int PlayerTargetingIndex = 0;

    public Card CurrentCard { get; private set; }
    public bool IsOccupied => CurrentCard != null;

    public void SetCard(Card card)
    {
        if (card != null)
        {
            CurrentCard = card;
            
            // Add to CardHandling tracking
            if (CardHandling.Instance != null)
            {
                if (SlotType == CardSlotType.Attack)
                {
                    CardHandling.Instance.AddCardToAttackSlot(PlayerIndex, card);
                }
                else
                {
                    CardHandling.Instance.AddCardToDefenseSlot(PlayerIndex, card);
                }
            }
        }
    }

    public Card RemoveCard()
    {
        Card removedCard = CurrentCard;
        
        if (removedCard != null && CardHandling.Instance != null)
        {
            // Remove from CardHandling tracking
            if (SlotType == CardSlotType.Attack)
            {
                CardHandling.Instance.RemoveCardFromAttackSlot(PlayerIndex, removedCard);
            }
            else
            {
                CardHandling.Instance.RemoveCardFromDefenseSlot(PlayerIndex, removedCard);
            }
        }
        
        CurrentCard = null;
        return removedCard;
    }

    public void SetPlayerTargettingIndex(int playerIndex)
    {
        PlayerTargetingIndex = playerIndex;
    }
    
    public void ClearSlot()
    {
        RemoveCard();
    }
}

