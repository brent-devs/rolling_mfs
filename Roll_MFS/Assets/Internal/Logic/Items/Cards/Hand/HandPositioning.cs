using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class HandPositioning : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private float maxDistApart = 0.1f;
    [SerializeField] private float minDistApart = 0.01f;
    public Transform centerPosOfHand = null;
    public float boardOffset = 0.005f;
    public bool IsXToRightPositive = false;
    public PlacementPosition OpposingTurnPos = null;
    public PlacementPosition SelfTurnPos = null;
    public Transform TransformOpposingPos;
    public Transform TransformSelfPos;
    public Transform TransformItemPlay;

    [Header("Traits")]
    public int owner = 0;

    [Header("Internals")]
    [SerializeField] private List<Card> cardsBeingHeld = new List<Card>();

    public void HandleReduceHand()
    {
        Vector3[] futurePlacements = ComputeCardFuturePlacements();
        for (int i = 0; i < cardsBeingHeld.Count; i++)
        {
            StartCoroutine(MoveCardToPosGeneric(cardsBeingHeld[i], futurePlacements[i], 0.1f));
        }
    }

    public void TryMoveCardToHand(Card card, Vector3 pos, CardFlyType flyType)
    {
        StartCoroutine(MoveCardToHand(card, pos, flyType));
    }
    
    public IEnumerator MoveCardToHand(Card card, Vector3 pos, CardFlyType flyType)
    {
        bool isRightSideEntry = ComputeSideEntry(pos);
        if (isRightSideEntry)
        {
            cardsBeingHeld.Add(card);
        }
        else
        {
            cardsBeingHeld.Insert(0, card);
        }
        Vector3[] futurePlacements = ComputeCardFuturePlacements();
        if (isRightSideEntry)
        {
            if (cardsBeingHeld.Count > 1)
            {
                for (int i = 0; i < cardsBeingHeld.Count - 1; i++)
                {
                    StartCoroutine(MoveCardToPosGeneric(cardsBeingHeld[i], futurePlacements[i], CardSettings.Instance.HAND_MOVE_SPEED));
                }
            }
            yield return StartCoroutine(MoveCardToPos(card, futurePlacements[cardsBeingHeld.Count - 1], flyType));
        }
        else
        {
            if (cardsBeingHeld.Count > 1)
            {
                for (int i = 1; i < cardsBeingHeld.Count; i++)
                {
                    StartCoroutine(MoveCardToPosGeneric(cardsBeingHeld[i], futurePlacements[i], CardSettings.Instance.HAND_MOVE_SPEED));
                }
            }
            yield return StartCoroutine(MoveCardToPos(card, futurePlacements[0], flyType));
        }
        yield return null;
    }

    public IEnumerator MoveCardToPosGeneric(Card card, Vector3 targetPos, float timeToMove)
    {
        card.RemovePhysics();
        card.RemoveCollisions();
        Vector3 startPos = card.transform.position;
        Quaternion startRot = card.transform.rotation;

        Vector3 faceUpEulerAngles = CardPrefabRepo.Instance.faceUp;
        Quaternion targetRotation = Quaternion.Euler(faceUpEulerAngles);
        if (card.owner != 0)
        {
            Vector3 faceDownEulerAngles = CardPrefabRepo.Instance.faceDown;
            targetRotation = Quaternion.Euler(faceDownEulerAngles);
        }

        float elapsed = 0f;

        while (elapsed < timeToMove)
        {
            float t = elapsed / timeToMove;
            card.transform.position = Vector3.Lerp(startPos, targetPos, t);
            card.transform.rotation = Quaternion.Slerp(startRot, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        card.transform.position = targetPos;
        card.transform.rotation = targetRotation;
        card.initialHoverPos = targetPos;
        card.TurnOnCollisions();
    }

    public IEnumerator MoveCardToPosArc(Card card, Vector3 targetPos, float timeToMove)
    {
        card.RemovePhysics();
        card.RemoveCollisions();
        Vector3 startPos = card.transform.position;
        Quaternion startRot = card.transform.rotation;

        Vector3 faceUpEulerAngles = CardPrefabRepo.Instance.faceUp;
        Quaternion targetRotation = Quaternion.Euler(faceUpEulerAngles);
        if (card.owner != 0)
        {
            Vector3 faceDownEulerAngles = CardPrefabRepo.Instance.faceDown;
            targetRotation = Quaternion.Euler(faceDownEulerAngles);
        }

        float elapsed = 0f;
        float arcHeight = 0.15f;

        while (elapsed < timeToMove)
        {
            float t = elapsed / timeToMove;
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
            float heightOffset = Mathf.Sin(t * Mathf.PI) * arcHeight;
            currentPos.y += heightOffset;

            card.transform.position = currentPos;
            card.transform.rotation = Quaternion.Slerp(startRot, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        card.transform.position = targetPos;
        card.transform.rotation = targetRotation;

        card.TurnOnCollisions();
    }

    public IEnumerator MoveCardToPos(Card card, Vector3 targetPos, CardFlyType flyType)
    {
        switch (flyType)
        {
            case CardFlyType.ReturnToHand:
                yield return MoveCardToPosGeneric(card, targetPos, CardSettings.Instance.HAND_RETURN_SPEED);
                break;
            case CardFlyType.Platform:
                yield return MoveCardToPosGeneric(card, targetPos, CardSettings.Instance.PLATFORM_DRAW_SPEED);
                break;
            case CardFlyType.Chest:
                yield return MoveCardToPosArc(card, targetPos, CardSettings.Instance.DECK_DRAW_SPEED);
                break;
        }
    }


    public Vector3[] ComputeCardFuturePlacements()
    {
        if (cardsBeingHeld.Count == 0 || centerPosOfHand == null) return new Vector3[0];

        float totalCards = cardsBeingHeld.Count;
        float spacing = Mathf.Lerp(maxDistApart, minDistApart, totalCards / 10f);
        float startPos = -((totalCards - 1) * spacing) / 2f;

        Vector3[] futurePositions = new Vector3[cardsBeingHeld.Count];

        for (int i = 0; i < totalCards; i++)
        {
            Vector3 newPosition = centerPosOfHand.position + new Vector3(startPos + (i * spacing), boardOffset + i * 0.001f, 0);
            futurePositions[i] = newPosition;
        }

        return futurePositions;
    }

    public bool ComputeSideEntry(Vector3 pos)
    {
        if (IsXToRightPositive)
        {
            return pos.x >= centerPosOfHand.position.x;
        }
        else
        {
            return pos.x <= centerPosOfHand.position.x;
        }
    }

    public bool TryRemoveCard(Card cardToRemoveFromHand)
    {
        bool result = cardsBeingHeld.Remove(cardToRemoveFromHand);
        HandleReduceHand();
        return result;
    }

    public void ShowHand()
    {
        Vector3 upFaceEulerAngles = CardPrefabRepo.Instance.faceUp;
        Quaternion rotation = Quaternion.Euler(upFaceEulerAngles);
        float totalCards = cardsBeingHeld.Count;
        if (OpposingTurnPos.currentCardIn != null)
        {
            OpposingTurnPos.currentCardIn.transform.rotation = rotation;
        }
        if (SelfTurnPos.currentCardIn != null)
        {
            SelfTurnPos.currentCardIn.transform.rotation = rotation;
        }
        for (int i = 0; i < totalCards; i++)
        {
            cardsBeingHeld[i].transform.rotation = rotation;
        }
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

    public int GetCardsInHand()
    {
        int cardsInHand = cardsBeingHeld.Count;
        if (OpposingTurnPos.currentCardIn != null)
        {
            cardsInHand++;
        }
        if (SelfTurnPos.currentCardIn != null)
        {
            cardsInHand++;
        }
        return cardsInHand++;
    }

    public bool AreSlotsEmpty()
    {
        if (OpposingTurnPos.currentCardIn != null || SelfTurnPos.currentCardIn != null)
        {
            return false;
        }
        return true;
    }

    public bool IsOpposingSlotEmpty()
    {
        if (OpposingTurnPos.currentCardIn != null)
        {
            return false;
        }
        return true;
    }

    public bool IsSelfSlotEmpty()
    {
        if (SelfTurnPos.currentCardIn != null)
        {
            return false;
        }
        return true;
    }

    public void HideHand()
    {
        Vector3 downFaceEulerAngles = CardPrefabRepo.Instance.faceDown;
        Quaternion rotation = Quaternion.Euler(downFaceEulerAngles);
        float totalCards = cardsBeingHeld.Count;
        if (OpposingTurnPos.currentCardIn != null)
        {
            OpposingTurnPos.currentCardIn.transform.rotation = rotation;
        }
        if (SelfTurnPos.currentCardIn != null)
        {
            SelfTurnPos.currentCardIn.transform.rotation = rotation;
        }
        for (int i = 0; i < totalCards; i++)
        {
            cardsBeingHeld[i].transform.rotation = rotation;
        }
    }

    public List<Card> GetCardsRefs()
    {
        return new List<Card>(cardsBeingHeld);
    }


    public void MoveCardToOpponentSlot(Card card)
    {
        TryRemoveCard(card);
        if (OpposingTurnPos.currentCardIn != null)
        {
            // Slot occupied; return card to hand gracefully
            Vector3 returnPos = centerPosOfHand != null ? centerPosOfHand.position : card.transform.position;
            TryMoveCardToHand(card, returnPos, CardFlyType.ReturnToHand);
            return;
        }
        OpposingTurnPos.PlaceCard(card, card.placementOffset);
        return;
    }

    public void MoveCardToSelfSlot(Card card)
    {
        TryRemoveCard(card);
        if (SelfTurnPos.currentCardIn != null)
        {
            // Slot occupied; return card to hand gracefully
            Vector3 returnPos = centerPosOfHand != null ? centerPosOfHand.position : card.transform.position;
            TryMoveCardToHand(card, returnPos, CardFlyType.ReturnToHand);
            return;
        }
        SelfTurnPos.PlaceCard(card, card.placementOffset);
        return;
    }

    public void HandleCardPickUp(Card card)
    {
        switch (card.PlayLocation)
        {
            case CardPlayLocation.Attack:
                OpposingTurnPos.ShowDisplayActionableGroup(card);
                break;
            case CardPlayLocation.Defense:
                SelfTurnPos.ShowDisplayActionableGroup(card);
                break;
            case CardPlayLocation.Both:
                OpposingTurnPos.ShowDisplayActionableGroup(card);
                SelfTurnPos.ShowDisplayActionableGroup(card);
                break;
        }
    }

    public void HandleCardDrop()
    {
        OpposingTurnPos.HideDisplayActionableGroup();
        SelfTurnPos.HideDisplayActionableGroup();
    }
}
