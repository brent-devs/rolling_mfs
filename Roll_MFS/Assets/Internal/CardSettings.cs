using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class CardSettings : MonoBehaviour
{
    public static CardSettings Instance { get; private set; }

    [Header("Layer Setup")]
    public LayerMask HideInteractionLayer;
    public LayerMask InteractionLayer;
    public LayerMask IgnoreCollisionLayer;
    public LayerMask HoldingLayer;
    public LayerMask DiceHoldingLayer;
    public LayerMask BoardElementsLayers;
    public LayerMask AllNonUIElements;
    public LayerMask RollingLayer;
    public LayerMask PlaceLayer;
    public LayerMask WhiteOutlineLayer;

    [Header("Hand and Grab")]
    public float HAND_MOVE_SPEED = 0.2f;
    public float DECK_DRAW_SPEED = 0.6f;
    public float HAND_RETURN_SPEED = 0.6f;
    public float PLATFORM_DRAW_SPEED = 0.6f;
    public float CARD_ROTATION_TO_CAM_SPEED = 2.4f;

    [Header("Dice")]
    public TMP_FontAsset diceFontAsset;
    public Material DiceGlitchMat;
    public Material DiceStandardMat;

    public float SixFontSize = 0.72f;
    public float EightFontSize = 0.7f;
    public float TenFontSize = 0.66f;
    public float TwentyFontSize = 0.44f;
    public float WaitBeforeReturningToStandardView = 0.5f;

    [Header("Dice Setup")]
    public Vector2 RollForceRange = new Vector2(0, 0);
    public Vector2 RollSpinRange = new Vector2(0, 0);
    public float DiceFlyTime = 1.0f;
    public float DiceFloatTime = 0.35f;
    public float DiceCamFollowZ = 0.35f;
    public float DiceCamFollowX = 0.35f;
    public float GroundParticleOffset = 0.02f;
    public float DiceCamTransitionTime = 0.5f;
    public float DelayForDiceLaunchOnCheat = 0.125f;
    public float AngularDragOfDiceDuringRoll = 1.25f;
    public float AngularDragOfDiceDuringSpin = 0.5f;
    public float AngularDragOfDiceDuringHold = 0.05f;

    [Header("Box HauntSetup")]
    public float TimeForBoxHauntToLinger = 1.2f;

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