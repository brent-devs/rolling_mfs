using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class DiceSettings : MonoBehaviour
{
    public static DiceSettings Instance { get; private set; }

    [Header("Layer Setup")]
    public LayerMask HideInteractionLayer;
    public LayerMask InteractionLayer;
    public LayerMask IgnoreCollisionLayer;
    public LayerMask HoldingLayer;
    public LayerMask BoardElementsLayers;
    public LayerMask AllNonUIElements;
    public LayerMask RollingLayer;
    public LayerMask PlaceableLayer;
    public LayerMask WhiteOutlineLayer;

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

    public float DragOfDiceDuringHold = 7.0f;

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

    [Header("Cursor Setup")]
    public float CursorHideTimeAfterDrop = 0.5f;
    public float CursorHideTimeAfterPanelClick = 0.3f;

    [Header("ReplaceCard Anims")]
    public float DiceReplaceTimer = 0.7f;

    [Header("Card Anims Time")]
    public float CopyCatStareAtNewCard = 0.5f;
    public float CardConsumedByChestTime = 0.15f;

    [Header("Girl Opponent Constant")]
    public float SlamAnimTime_Girl = 0.6f;
    public float WaitAfterOppDies = 1.2f;

    [Header("Game Items Constants")]
    public float ItemUsedCursorDelay = 0.25f;
    public float ItemDeletionTime = 0.25f;
    public float ItemShakeDuration = 0.6f;
    public float PauseForItemSelection = 1.25f;

    [Header("Items Anims")]
    public Material ItemGlitchMat;
    public Vector3 FinishedLighterEulers = Vector3.zero;
    public Vector3 StartLighterEulers = Vector3.zero;

    [Header("Lighter Cover")]
    public Vector3 StartLighterCoverEulers = Vector3.zero;
    public Vector3 HallfwayLighterCoverEulers = Vector3.zero;
    public Vector3 FinishedLighterCoverEulers = Vector3.zero;
    public float CoverOpenStart = 0.2f;
    public float CoverOpenFinish = 0.1f;

    public Vector3 LighterPositionOffset = Vector3.zero;
    public float ComicDropThreshold = 0.2f;

    [Header("Misc")]
    public float LightBulbTransitionDuration = 0.4f;
    public float WaitBeforeOppLoss = 0.35f;
    public float WaitBeforeCleaningBoardAfterOppLoss = 2.5f;
    public float WaitToLookAtDeathRoll = 0.75f;
    public float WaitToLaunchDeathRoll = 0.25f;

    [Header("Tutorial")]
    public float TutorialDisplayTime = 12.0f;

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

    public void SetObjectToInteractLayer(GameObject obj)
    {
        if (obj != null)
        {
            obj.layer = Mathf.RoundToInt(Mathf.Log(InteractionLayer.value, 2));
        }
        else
        {
            Debug.LogWarning("SetObjectToInteractLayer: GameObject is null.");
        }
    }

    public void SetObjectToInteractHiddenLayer(GameObject obj)
    {
        if (obj != null)
        {
            obj.layer = Mathf.RoundToInt(Mathf.Log(HideInteractionLayer.value, 2));
        }
        else
        {
            Debug.LogWarning("SetObjectToHiddenLayer: GameObject is null.");
        }
    }

    public void SetObjectToIgnoreCollisionLayer(GameObject obj)
    {
        if (obj != null)
        {
            obj.layer = Mathf.RoundToInt(Mathf.Log(IgnoreCollisionLayer.value, 2));
        }
        else
        {
            Debug.LogWarning("SetObjectToHiddenLayer: GameObject is null.");
        }
    }
}