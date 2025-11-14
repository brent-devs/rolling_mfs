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


    public Vector3[] ComputeCardFuturePlacements(int totalCardsInHand)
    {
        if (totalCardsInHand == 0 || centerPosOfHand == null) return new Vector3[0];

        float totalCards = totalCardsInHand;
        float spacing = Mathf.Lerp(maxDistApart, minDistApart, totalCards / 10f);
        float startPos = -((totalCards - 1) * spacing) / 2f;

        Vector3[] futurePositions = new Vector3[totalCardsInHand];

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

    public void ShowHand(List<Card> cardsInHand)
    {
        Vector3 upFaceEulerAngles = CardPrefabRepo.Instance.faceUp;
        Quaternion rotation = Quaternion.Euler(upFaceEulerAngles);
        if (OpposingTurnPos.currentCardIn != null)
        {
            OpposingTurnPos.currentCardIn.transform.rotation = rotation;
        }
        if (SelfTurnPos.currentCardIn != null)
        {
            SelfTurnPos.currentCardIn.transform.rotation = rotation;
        }
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            cardsInHand[i].transform.rotation = rotation;
        }
    }

    public void HideHand(List<Card> cardsInHand)
    {
        Vector3 downFaceEulerAngles = CardPrefabRepo.Instance.faceDown;
        Quaternion rotation = Quaternion.Euler(downFaceEulerAngles);
        if (OpposingTurnPos.currentCardIn != null)
        {
            OpposingTurnPos.currentCardIn.transform.rotation = rotation;
        }
        if (SelfTurnPos.currentCardIn != null)
        {
            SelfTurnPos.currentCardIn.transform.rotation = rotation;
        }
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            cardsInHand[i].transform.rotation = rotation;
        }
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
