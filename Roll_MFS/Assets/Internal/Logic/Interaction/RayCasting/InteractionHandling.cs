using UnityEngine;
using UnityEngine.EventSystems;

public enum InteractionState
{
    None,
    Idle,
    Shopping,
    MyTurn,
    Grabbing,
}

public class InteractionHandling : MonoBehaviour
{
    [Header("Layers")]
    public string HeldLayerName = "HeldInteractable";
    public LayerMask BoardElementsMask;
    public LayerMask AllNonUIElements;

    [Header("Setup")]
    public float FollowCursorThreshold = 0.025f;
    public float CursorFollowMagnitudeMultiplier = 1.25f;
    public float DiceOffsetFromBoardElements = 0.025f;
    public float DiceGrabbingMinX = 0.1f;
    public float DiceGrabbingMaxX = 0.9f;
    public float DiceGrabbingMinY = 0.1f;
    public float DiceGrabbingMaxY = 0.9f;
    public float GrabbingCursorRange = 0.9f;
    public float PlacementPositionRange = 1.4f;


    [Header("Dependencies")]
    [SerializeField] private InteractionRaycaster idleRaycaster;
    [SerializeField] private InteractionRaycaster shoppingRaycaster;
    [SerializeField] private InteractionRaycaster myTurnRaycaster;
    [SerializeField] private InteractionRaycaster grabbingRaycaster;

    [Header("Traits")]
    public int Owner = 0;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private InteractionState currState;
    [SerializeField] private IInteractable currentHoveredInteractable;
    [SerializeField] private Vector3 lastValidCursorPosForHover;

    public Camera MainCamera
    {
        get => mainCamera;
        set => mainCamera = value;
    }

    public InteractionState CurrState
    {
        get => currState;
        set
        {
            UpdateCursorForState(value);
            currState = value;
        }
    }

    public IInteractable CurrentHoveredInteractable
    {
        get { return currentHoveredInteractable; }
        set
        {
            if (currentHoveredInteractable == null)
            {
                currentHoveredInteractable = value;
            }
            else
            {
                currentHoveredInteractable.OnDeHover();
                currentHoveredInteractable = value;
            }
        }
    }

    public static InteractionHandling Instance { get; private set; }

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

        if (idleRaycaster == null)
        {
            Debug.LogError("idleRaycaster Missing");
        }
        if (myTurnRaycaster == null)
        {
            Debug.LogError("rollTurnRaycaster Missing");

        }
        if (shoppingRaycaster == null)
        {
            Debug.LogError("shoppingRaycaster Missing");
        }
    }

    void Update()
    {
        switch (currState)
        {
            case InteractionState.None:
                break;
            case InteractionState.Idle:
                idleRaycaster.Raycast();
                break;
            case InteractionState.Shopping:
                shoppingRaycaster.Raycast();
                break;
            case InteractionState.MyTurn:
                myTurnRaycaster.Raycast();
                break;
            case InteractionState.Grabbing:
                grabbingRaycaster.Raycast();
                break;
        }
    }

    public bool IsUIBlockingRaycast()
    {
        if (EventSystem.current != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
        }
        return false;
    }

    public void GrabObject(MovableInteractable interactable)
    {
        if (interactable == null)
        {
            Debug.LogWarning("Tried to grab a null interactable.");
            return;
        }

        if (GrabHandling.Instance == null)
        {
            Debug.LogError("GrabHandling singleton is missing from the scene.");
            return;
        }

        var heldLayer = LayerMask.NameToLayer(HeldLayerName);
        if (heldLayer == -1)
        {
            Debug.LogWarning($"Held layer '{HeldLayerName}' was not found. The interactable layer was not changed.");
        }
        else
        {
            interactable.ApplyLayer(heldLayer);
        }

        GrabHandling.Instance.GrabObject(interactable);
        CurrState = InteractionState.Grabbing;
    }

    public void UpdateCursorForState(InteractionState newState)
    {
        switch (newState)
        {
            case InteractionState.Grabbing:
                CursorLogic.Instance.ShowHoldingCursor();
                break;
            case InteractionState.Idle:
                CursorLogic.Instance.ShowCursor();
                break;
            case InteractionState.MyTurn:
                CursorLogic.Instance.ShowCursor();
                break;
            case InteractionState.Shopping:
                CursorLogic.Instance.ShowCursor();
                break;
            case InteractionState.None:
                CursorLogic.Instance.HideCursor();
                break;
        }
    }

    public Vector3 RaycastCursorPosOnBoard(float offset)
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, GrabbingCursorRange, BoardElementsMask))
        {
            Debug.Log("hit name" + hit.collider.name);
            if (hit.collider != null)
            {
                Vector3 boardGameTarget = new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);
                return boardGameTarget;
            }
        }
        return Vector3.zero;
    }

    public Vector3 RaycastCursorPos()
    {
        Vector2 mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // Viewport bounds
        float minX = DiceGrabbingMinX;
        float maxX = DiceGrabbingMaxX;
        float minY = DiceGrabbingMinY;
        float maxY = DiceGrabbingMaxY;

        // Clamp the viewport position
        mouseViewportPos.x = Mathf.Clamp(mouseViewportPos.x, minX, maxX);
        mouseViewportPos.y = Mathf.Clamp(mouseViewportPos.y, minY, maxY);

        // Convert clamped viewport pos back to screen space
        Vector3 clampedScreenPos = Camera.main.ViewportToScreenPoint(mouseViewportPos);

        // Create ray from clamped screen pos
        Ray ray = mainCamera.ScreenPointToRay(clampedScreenPos);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, GrabbingCursorRange, AllNonUIElements))
        {
            if (hit.collider != null)
            {
                lastValidCursorPosForHover = new Vector3(hit.point.x, hit.point.y + DiceOffsetFromBoardElements, hit.point.z);
                return lastValidCursorPosForHover;
            }
        }
        lastValidCursorPosForHover = ray.GetPoint(GrabbingCursorRange);
        return lastValidCursorPosForHover;
    }

    public PlacementPosition RaycastCursorPosPlacementPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, PlacementPositionRange, CardSettings.Instance.PlaceableLayer))
        {
            if (hit.collider != null)
            {
                return hit.collider.gameObject.GetComponent<PlacementPosition>();
            }
        }

        return null;
    }
}
