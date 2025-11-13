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
        for (int i = 0; i < cardsBeingHeld.Count; i++)
        {
            // StartCoroutine(MoveCardToPosGeneric(cardsBeingHeld[i], futurePlacements[i], 0.1f));
        }
    }
    
    public void MoveCardToHand(Card card)
    {
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
    }

    public bool TryRemoveCard(Card cardToRemoveFromHand)
    {
        bool result = cardsBeingHeld.Remove(cardToRemoveFromHand);
        HandleReduceHand();
        return result;
    }

    public Vector3 GetFinalCardPos()
    {
        return cardsBeingHeld[cardsBeingHeld.Count - 1].transform.position;
    }

    public Vector3 GetInitialCardPos()
    {
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
        Vector3 downFaceEulerAngles = CardPrefabRepo.Instance.faceDown;
        Quaternion rotation = Quaternion.Euler(downFaceEulerAngles);
        float totalCards = cardsBeingHeld.Count;
        for (int i = 0; i < totalCards; i++)
        {
            cardsBeingHeld[i].transform.rotation = rotation;
        }
    }

    public List<Card> GetCardsRefs()
    {
        return new List<Card>(cardsBeingHeld);
    }
}
