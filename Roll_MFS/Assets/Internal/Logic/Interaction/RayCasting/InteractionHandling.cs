using UnityEngine;
using UnityEngine.EventSystems;

public enum InteractionState
{
    None,
    Idle,
    Shopping,
    RollTurn,
}

public class InteractionHandling : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InteractionRaycaster idleRaycaster;
    [SerializeField] private InteractionRaycaster shoppingRaycaster;
    [SerializeField] private InteractionRaycaster rollTurnRaycaster;

    [Header("Traits")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private InteractionState currState;
    [SerializeField] private IInteractable currentHoveredInteractable;

    public Camera MainCamera
    {
        get => mainCamera;
        set => mainCamera = value;
    }

    public InteractionState CurrState
    {
        get => currState;
        set => currState = value;
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
    }

    void Start() {
        mainCamera = GetComponent<Camera>();
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
            case InteractionState.RollTurn:
                rollTurnRaycaster.Raycast();
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
}
