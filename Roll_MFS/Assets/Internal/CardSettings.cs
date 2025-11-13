using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class CardSettings : MonoBehaviour
{
    public static CardSettings Instance { get; private set; }

    [Header("Layer Setup")]
    public LayerMask InteractionLayer;
    public LayerMask HeldInteractionLayer;
    public LayerMask PlaceableLayer;

    [Header("Hand and Grab")]
    public float HAND_MOVE_SPEED = 0.2f;
    public float DECK_DRAW_SPEED = 0.6f;
    public float HAND_RETURN_SPEED = 0.6f;
    public float PLATFORM_DRAW_SPEED = 0.6f;
    public float CARD_ROTATION_TO_CAM_SPEED = 2.4f;


    [Header("Card Setup")]
    public Material CardGlitchMat;
    public float CardCamTransitionTime = 0.5f;
    public float CardActivedTime = 1.2f;
    public float CardRaiseTime = 0.2f;
    public int CardRaisePower = 4;
    public float CardSpinTime = 0.4f;
    public float CardFadeTime = 0.2f;
    public float CardShrinkTime = 0.4f;
    public float CardShake = 0.4f;
    public float CardShakeBuffer = 0.2f;
    public float CardHoverHeight = 0.04f;
    public float CardUnusedTime = 0.6f;
    public float CardDestroyedTime = 1.2f;
    public float CardPreActivatedTime = 0.15f;
    public float CopyCatStareAtNewCard = 0.5f;
    public float CardConsumedByChestTime = 0.15f;
    public float AngularDragOfCardDuringHold = 2.0f;
    public float DragOfCardDuringHold = 7.0f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}