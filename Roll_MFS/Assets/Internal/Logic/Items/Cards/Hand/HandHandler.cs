using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum CardFlyType {
    ReturnToHand, 
    Platform,
    Chest,
}

public class HandHandler : MonoBehaviour
{
    [Header("Dependencies")]
    public HandPositioning HandPositioning;

    [Header("Traits")]
    public int Owner = 0;

    [Header("Internals")]
    [SerializeField] private List<Card> cardsBeingHeld = new List<Card>();

    public void HandleReduceHand()
    {
        if (HandPositioning == null)
        {
            Debug.LogError("HandPositioning dependency missing on HandHandler.");
            return;
        }

        if (cardsBeingHeld.Count == 0)
        {
            return;
        }

        Vector3[] futurePlacements = HandPositioning.ComputeCardFuturePlacements(cardsBeingHeld.Count);
        int placementCount = Mathf.Min(futurePlacements.Length, cardsBeingHeld.Count);
        for (int i = 0; i < placementCount; i++)
        {
            StartCoroutine(HandPositioning.MoveCardToPosGeneric(cardsBeingHeld[i], futurePlacements[i], CardSettings.Instance.HAND_MOVE_SPEED));
        }
    }

    public void MoveCardToHand(Card card, CardFlyType flyType = CardFlyType.ReturnToHand)
    {
        if (HandPositioning == null)
        {
            Debug.LogError("HandPositioning dependency missing on HandHandler.");
            return;
        }

        if (card == null)
        {
            Debug.LogWarning("Attempted to move a null card to hand.");
            return;
        }

        if (cardsBeingHeld.Contains(card))
        {
            Debug.LogWarning("Card already in hand: " + card.GetCardName());
            return;
        }

        Vector3 pos = card.transform.position;
        bool isRightSideEntry = HandPositioning.ComputeSideEntry(pos);
        Debug.Log("Is it a right side entry? " + isRightSideEntry);
        if (isRightSideEntry)
        {
            cardsBeingHeld.Add(card);
        }
        else
        {
            cardsBeingHeld.Insert(0, card);
        }

        card.handPositioning = HandPositioning;

        StartCoroutine(MoveCardToHandRoutine(card, isRightSideEntry, flyType));
    }

    public bool TryRemoveCard(Card cardToRemoveFromHand)
    {
        bool result = cardsBeingHeld.Remove(cardToRemoveFromHand);
        if (result)
        {
            HandleReduceHand();
        }
        return result;
    }

    public Vector3 GetFinalCardPos()
    {
        if (cardsBeingHeld.Count == 0)
        {
            return HandPositioning != null && HandPositioning.centerPosOfHand != null
                ? HandPositioning.centerPosOfHand.position
                : Vector3.zero;
        }
        return cardsBeingHeld[cardsBeingHeld.Count - 1].transform.position;
    }

    public Vector3 GetInitialCardPos()
    {
        if (cardsBeingHeld.Count == 0)
        {
            return HandPositioning != null && HandPositioning.centerPosOfHand != null
                ? HandPositioning.centerPosOfHand.position
                : Vector3.zero;
        }
        return cardsBeingHeld[0].transform.position;
    }

    public bool IsHandEmpty()
    {
        if (cardsBeingHeld.Count == 0)
        {
            return true;
        }
        return false;
    }


    public void HideHand()
    {
        if (HandPositioning == null)
        {
            Debug.LogError("HandPositioning dependency missing on HandHandler.");
            return;
        }

        HandPositioning.HideHand(cardsBeingHeld);
    }

    public List<Card> GetCardsRefs()
    {
        return new List<Card>(cardsBeingHeld);
    }

    public void ShowHand()
    {
        if (HandPositioning == null)
        {
            Debug.LogError("HandPositioning dependency missing on HandHandler.");
            return;
        }

        HandPositioning.ShowHand(cardsBeingHeld);
    }

    private IEnumerator MoveCardToHandRoutine(Card card, bool placedAtEnd, CardFlyType flyType)
    {
        Vector3[] futurePlacements = HandPositioning.ComputeCardFuturePlacements(cardsBeingHeld.Count);
        if (futurePlacements.Length != cardsBeingHeld.Count)
        {
            Debug.LogWarning("Mismatch between cards held and computed placements.");
            yield break;
        }

        if (placedAtEnd)
        {
            if (cardsBeingHeld.Count > 1)
            {
                for (int i = 0; i < cardsBeingHeld.Count - 1; i++)
                {
                    StartCoroutine(HandPositioning.MoveCardToPosGeneric(cardsBeingHeld[i], futurePlacements[i], CardSettings.Instance.HAND_MOVE_SPEED));
                }
            }
            yield return StartCoroutine(HandPositioning.MoveCardToPos(card, futurePlacements[cardsBeingHeld.Count - 1], flyType));
        }
        else
        {
            if (cardsBeingHeld.Count > 1)
            {
                for (int i = 1; i < cardsBeingHeld.Count; i++)
                {
                    StartCoroutine(HandPositioning.MoveCardToPosGeneric(cardsBeingHeld[i], futurePlacements[i], CardSettings.Instance.HAND_MOVE_SPEED));
                }
            }
            yield return StartCoroutine(HandPositioning.MoveCardToPos(card, futurePlacements[0], flyType));
        }
    }
}
