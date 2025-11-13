using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum PlacementType {
    Card,
    Item,
}

public class PlacementPosition : MonoBehaviour
{
    [Header("Setup")]
    public PlacementType myType = PlacementType.Card;
    public bool IsAttack = false;

    [Header("Dependencies")]
    public ActionGroupDisplay displayActionableGroup; 

    [Header("Traits")]
    public Card currentCardIn;

    [Header("HookIns")]
    public UnityEvent OnCardPlaced;

    public bool IsValidPlacement(GameObject thingBeingPlaced) {
        if (myType == PlacementType.Card) {
            Card cardToPlace = thingBeingPlaced.GetComponent<Card>();
            if (cardToPlace != null && cardToPlace.IsValidPlacement(this)) {
                if (currentCardIn == null) {
                    return true;
                }
            }
        }
        return false;
    }

    public void PlaceCard(Card card, float offset)
    {
        // CardAudioFXManager.Instance.PlayCardPlaced();
        OnCardPlaced.Invoke();
        // TODO_LONG(oliver): make this compatible with multiplayer first plays
        if (card.owner == 0)
        {
            HandleTutorialLogic();
        }
        currentCardIn = card;
        currentCardIn.MyPlacementPosition = this;
        ComputeCardRotation(card);
        card.rb.velocity = Vector3.zero;
        card.rb.angularVelocity = Vector3.zero;
        card.transform.position = transform.position + new Vector3(0, offset, 0);
        card.rb.useGravity = true;
        card.rb.isKinematic = false;
    }

    public void ShowDisplayActionableGroup(Card card)
    {
        if (currentCardIn == null || currentCardIn == card)
        {
            displayActionableGroup.Show();
        }
    }
    
    public void HandleTutorialLogic()
    {
                    /*
            if (TutorialManager.Instance != null) {
                TutorialManager.Instance.HideCardTutPanel();
                if (IsAttack)
                {
                    if (TutorialManager.Instance.ShouldShowOffensePanel())
                    {
                        TutorialManager.Instance.ShowOffenseTutorialPanel();
                    }
                }
                else
                {
                    if (TutorialManager.Instance.ShouldShowDefensePanel())
                    {
                        TutorialManager.Instance.ShowDefenseTutorialPanel();
                    }
                }
            }
            */
    }

    public void HideDisplayActionableGroup() {
        displayActionableGroup.Hide();
    }

    public void RemoveCard() {
        currentCardIn = null;
    }

    private void ComputeCardRotation(Card card) {
        if (card.owner == 0) {
            card.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
        else {
            card.transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }
}
