using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class Card : PhysicsInteractable
{
    [Header("Card Traits")]
    public HandPositioning handPositioning;
    public float placementOffset = 0.015f;
    public int owner = 0;
    public CardPlayLocation PlayLocation;
    public CardType type;
    public UnityEvent OnPlayEvent;
    public PlacementPosition MyPlacementPosition;
    public PlayableCardSlot MyCardSlot;

    [Header("Card Dependencies")]
    public CardFrontDisplay frontDisplay;
    public MeshFilter meshFilter;
    public MeshRenderer cardRenderer;
    public Mesh[] potentialMeshes;
    public Sprite[] textureSprites;
    public Image textureImage;

    [Header("Card Transforms")]
    public Transform transformFront;
    public Transform transformBack;

    void Awake()
    {
        if (meshFilter == null || potentialMeshes == null || textureSprites == null || potentialMeshes.Length == 0)
        {
            Debug.LogWarning("MeshFilter or potentialMeshes not set up correctly.");
            return;
        }
        int index = Random.Range(0, potentialMeshes.Length);
        Mesh randomMesh = potentialMeshes[index];
        Sprite randomSprite = textureSprites[index];
        meshFilter.mesh = randomMesh;
        textureImage.sprite = randomSprite;
        MyCardSlot = null;
    }

    public void Place(PlacementPosition pos)
    {
        Debug.Log("Place");
    }

    public void HandleCardRelease()
    {
        PlacementPosition pos = InteractionHandling.Instance.RaycastCursorPosPlacementPosition();
        if (pos == null)
        {
            if (handPositioning != null)
            {
                // CardAudioFXManager.Instance.PlayCardReturnedToHand();
                //handPositioning.TryMoveCardToHand(this, transform.position, CardFlyType.ReturnToHand);
                handPositioning.HandleCardDrop();
            }
            // CursorSettings.Instance.ShowCursorFromItemUseAfterDelay();
            return;
        }
        if (pos.IsValidPlacement(gameObject))
        {
            // pos.PlaceCard(this, placementOffset);
            //ExternalLogger.Instance.CardEventLog("card_placed", GetCardName(), this, "target_position", pos.IsAttack ? "oppose" : "self");
            handPositioning.HandleCardDrop();
        }
        else
        {
            if (handPositioning != null)
            {
                // CardAudioFXManager.Instance.PlayCardReturnedToHand();
                //handPositioning.TryMoveCardToHand(this, transform.position, CardFlyType.ReturnToHand);
                handPositioning.HandleCardDrop();
            }
        }
        // CursorSettings.Instance.ShowCursorFromItemUseAfterDelay();
    }

    public void HandleCardPickUp()
    {
        if (handPositioning != null)
        {
            // bool _result = handPositioning.TryRemoveCard(this);
            // handPositioning.HandleCardPickUp(this);
        }
        if (MyPlacementPosition != null)
        {
            MyPlacementPosition.RemoveCard();
            MyPlacementPosition = null;
        }
    }


    public IEnumerator PlayCard()
    {
        yield return null;
    }

    public string GetCardName()
    {
        switch (type)
        {
            case CardType.ReplaceSix:
                return StringRepo.Instance.Replace_6_Name;
            case CardType.ReplaceTen:
                return StringRepo.Instance.Replace_10_Name;
            case CardType.Angel:
                return StringRepo.Instance.Angel_Name;
            case CardType.Thief:
                return StringRepo.Instance.Thief_Name;
            case CardType.CopyCat:
                return StringRepo.Instance.Copycat_Name;
            case CardType.Cage:
                return StringRepo.Instance.Cage_Name;
            case CardType.Torch:
                return StringRepo.Instance.Torch_Name;
            case CardType.Reroll:
                return StringRepo.Instance.Reroll_Name;
            case CardType.Treasure:
                return StringRepo.Instance.Treasure_Name;
            case CardType.Bonfire:
                return StringRepo.Instance.Bonfire_Name;
        }
        return "missingName";
    }

    public void HandleCardTypePlayed(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.ReplaceSix:
                //CardEffectRepo.Instance.PlayReplaceSixEffect();
                break;
            case CardType.ReplaceTen:
                //CardEffectRepo.Instance.PlayReplaceTenEffect();
                break;
            case CardType.Angel:
                //CardEffectRepo.Instance.PlayAngelEffect();
                break;
            case CardType.Thief:
                //CardEffectRepo.Instance.PlayThiefEffect();
                break;
            case CardType.CopyCat:
                //CardEffectRepo.Instance.PlayCopyCatEffect(this);
                break;
            case CardType.Cage:
                //CardEffectRepo.Instance.PlayCageEffect();
                break;
            case CardType.Torch:
                // CardEffectRepo.Instance.PlayTorchEffect();
                break;
            case CardType.Reroll:
                // CardEffectRepo.Instance.PlayRerollEffect();
                break;
            case CardType.Treasure:
                //CardEffectRepo.Instance.PlayTreasureEffect(this);
                break;
            case CardType.Bonfire:
                //CardEffectRepo.Instance.PlayBonfireEffect(this);
                break;
        }
    }

    public void SetOwner(int newOwner)
    {
        owner = newOwner;
        /*
        if (owner == Deck.Instance.currViewerIndex)
        {
            AnimationSettings.Instance.SetObjectToInteractLayer(gameObject);
        }
        else
        {
            AnimationSettings.Instance.SetObjectToInteractHiddenLayer(gameObject);
        }
        */
        // TODO(oliver): this should be done on interact level 
    }

    public bool IsValidPlacement(PlacementPosition pos)
    {
        if (pos.IsAttack)
        {
            if (PlayLocation == CardPlayLocation.Defense)
            {
                return false;
            }
            return true;
        }
        else
        {
            if (PlayLocation == CardPlayLocation.Attack)
            {
                return false;
            }
            return true;
        }
    }

    public void SwapType(CardType newType)
    {
        type = newType;
        switch (newType)
        {
            case CardType.Angel:
                ItemName = StringRepo.Instance.Angel_Name;
                PlayLocation = CardPlayLocation.Defense;
                break;
            case CardType.ReplaceSix:
                ItemName = StringRepo.Instance.Replace_6_Name;
                PlayLocation = CardPlayLocation.Both;
                break;
            case CardType.ReplaceTen:
                ItemName = StringRepo.Instance.Replace_10_Name;
                PlayLocation = CardPlayLocation.Both;
                break;
            case CardType.Reroll:
                ItemName = StringRepo.Instance.Reroll_Name;
                PlayLocation = CardPlayLocation.Both;
                break;
            case CardType.Cage:
                ItemName = StringRepo.Instance.Cage_Name;
                PlayLocation = CardPlayLocation.Both;
                break;
            case CardType.Treasure:
                ItemName = StringRepo.Instance.Treasure_Name;
                PlayLocation = CardPlayLocation.Both;
                break;
            case CardType.Bonfire:
                ItemName = StringRepo.Instance.Bonfire_Name;
                PlayLocation = CardPlayLocation.Both;
                break;
            case CardType.Torch:
                ItemName = StringRepo.Instance.Torch_Name;
                PlayLocation = CardPlayLocation.Both;
                break;
            case CardType.Thief:
                ItemName = StringRepo.Instance.Thief_Name;
                PlayLocation = CardPlayLocation.Defense;
                break;
            case CardType.CopyCat:
                ItemName = StringRepo.Instance.Copycat_Name;
                PlayLocation = CardPlayLocation.Both;
                break;
        }
        frontDisplay.SetupCardType(type);
    }

    public void RemovePhysics()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void TurnOnPhysics()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void RemoveCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("IgnoreCollision");
    }

    public void TurnOnCollisions()
    {
        if (owner == 0)
        {
            gameObject.layer = LayerMask.NameToLayer("Interaction");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("InteractionHidden");
        }
        RemovePhysics();
    }

    public string GetCardDescription()
    {
        switch (type)
        {
            case CardType.Angel:
                return StringRepo.Instance.Angel_Description;
            case CardType.ReplaceSix:
                return StringRepo.Instance.Replace_6_Description;
            case CardType.ReplaceTen:
                return StringRepo.Instance.Replace_10_Description;
            case CardType.Reroll:
                return StringRepo.Instance.Reroll_Description;
            case CardType.Cage:
                return StringRepo.Instance.Encage_Description;
            case CardType.Treasure:
                return StringRepo.Instance.Treasure_Description;
            case CardType.Bonfire:
                return StringRepo.Instance.Bonfire_Description;
            case CardType.Torch:
                return StringRepo.Instance.Torch_Description;
            case CardType.Thief:
                return StringRepo.Instance.Rat_Description;
            case CardType.CopyCat:
                return StringRepo.Instance.Copycat_Description;
            default:
                return "? ? ?";
        }
    }

    /*
    public PanelSize GetCardDescriptionSize()
    {
        switch (type)
        {
            case CardType.Angel:
                return StringRepo.Instance.Angel_Description_Size;
            case CardType.ReplaceSix:
                return StringRepo.Instance.Replace_6_Description_Size;
            case CardType.ReplaceTen:
                return StringRepo.Instance.Replace_10_Description_Size;
            case CardType.Reroll:
                return StringRepo.Instance.Reroll_Description_Size;
            case CardType.Cage:
                return StringRepo.Instance.Encage_Description_Size;
            case CardType.Treasure:
                return StringRepo.Instance.Treasure_Description_Size;
            case CardType.Bonfire:
                return StringRepo.Instance.Bonfire_Description_Size;
            case CardType.Torch:
                return StringRepo.Instance.Torch_Description_Size;
            case CardType.Thief:
                return StringRepo.Instance.Rat_Description_Size;
            case CardType.CopyCat:
                return StringRepo.Instance.Copycat_Description_Size;
            default:
                return PanelSize.Small;
        }
    }
    */

    /*
    public IEnumerator BeginDestroy(CardDestroyType destroyType)
    {
        switch (destroyType)
        {
            case CardDestroyType.Unused:
                yield return cardShaker.Shake();
                yield return ShrinkAndDelete();
                break;
            case CardDestroyType.Burned:
                //yield return CardTriggerHandler.Instance.HandleCardFloatWithoutSpin(this);
                yield return new WaitForSeconds(AnimationSettings.Instance.CardPreActivatedTime);
                TransferToBurnAnim();
                yield return ShrinkAndDelete();
                break;
            case CardDestroyType.Standard:
                yield return ShrinkAndDelete();
                break;
        }
    }

    */

    public void TransferToBurnAnim()
    {
        Material glitchMat = CardSettings.Instance.CardGlitchMat;
        Material[] newMats = new Material[cardRenderer.materials.Length];
        for (int i = 0; i < newMats.Length; i++)
        {
            newMats[i] = glitchMat;
        }
        if (transformFront != null)
        {
            transformFront.gameObject.SetActive(false);
        }
        if (transformBack != null)
        {
            transformBack.gameObject.SetActive(false);
        }
        cardRenderer.materials = newMats;
    }

    public IEnumerator ShrinkAndDelete()
    {
        float duration = CardSettings.Instance.CardShrinkTime;
        float elapsed = 0f;

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float easedT = t * t;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, easedT);
            yield return null;
        }

        transform.localScale = Vector3.zero;
        if (handPositioning != null)
        {
            // handPositioning.TryRemoveCard(this);
        }
        Destroy(gameObject);
    }

    public void TriggerLiftEvent()
    {
        SetupLiftPhysics();
    }

    public void TriggerReleaseEvent()
    {
        SetupReleasePhysics();
    }

    public void SetupReleasePhysics()
    {
        gameObject.layer = (int)Mathf.Log(CardSettings.Instance.InteractionLayer.value, 2);
    }

    public void SetupLiftPhysics()
    {
        gameObject.layer = (int)Mathf.Log(CardSettings.Instance.HeldInteractionLayer.value, 2);
        rb.angularDrag = CardSettings.Instance.AngularDragOfCardDuringHold;
        rb.drag = CardSettings.Instance.DragOfCardDuringHold;
    }
}