using UnityEngine;
using System.Collections;
public class GrabHandling : MonoBehaviour
{
    public static GrabHandling Instance { get; private set; }

    [Header("Traits")]

    public MovableInteractable CurrentHeld;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (CurrentHeld == null)
        {
            return;
        }
        // TODO(oliver): handle hold duration management
        // TODO(oliver): tracking velocity. TrackMouseVelocity();
        CheckShouldRotateObject();
        CheckShouldThrowObject();
        CheckShouldDropObject();
        
    }

    void FixedUpdate()
    {
        if (CurrentHeld == null)
        {
            return;
        }
        HandleObjectPosInHand();
    }


    public void GrabObject(MovableInteractable interactable)
    {
        if (interactable == null)
        {
            Debug.LogWarning("GrabHandling.GrabObject called with a null interactable.");
            return;
        }

        if (CurrentHeld != null && CurrentHeld != interactable)
        {
            ReleaseCurrentHeld();
        }

        CurrentHeld = interactable;
        CurrentHeld.OnGrabbed();
    }

    public void ReleaseCurrentHeld()
    {
        if (CurrentHeld == null)
        {
            return;
        }

        CurrentHeld.OnReleased();
        CurrentHeld.RestoreOriginalLayer();
        CurrentHeld = null;
    }


    private void HandleObjectPosInHand()
    {
        Rigidbody rigidBody = CurrentHeld.gameObject.GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            return;
        }

        Vector3 targPos = HandleHandTracking();

        // Handle Dice
        // Handle Card


        Vector3 direction = (targPos - rigidBody.position).normalized;
        float distance = Vector3.Distance(rigidBody.position, targPos);

        if (distance > InteractionHandling.Instance.FollowCursorThreshold)
        {

            float forceMagnitude = distance * InteractionHandling.Instance.CursorFollowMagnitudeMultiplier;
            Vector3 force = direction * forceMagnitude;
            rigidBody.AddForce(force, ForceMode.VelocityChange);
        }
    }

    private Vector3 HandleHandTracking()
    {
        return CurrentHeld.HandleHeldPosTracking();
    }

    private void CheckShouldDropObject()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (CurrentHeld == null)
            {
                return;
            }
            Rigidbody rigidBody = CurrentHeld.GetComponent<Rigidbody>();
            if (rigidBody == null)
            {
                return;
            }
            DropObject(rigidBody);
        }
    }

    private void CheckShouldThrowObject()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentHeld == null)
            {
                return;
            }
            Rigidbody rigidBody = CurrentHeld.GetComponent<Rigidbody>();
            if (rigidBody == null)
            {
                return;
            }
            ThrowObject(rigidBody);
        }
    }

    private void DropObject(Rigidbody rigidBody)
    {
        rigidBody.transform.parent = null;
        rigidBody.useGravity = true;

        rigidBody.drag = 1.0f;
        rigidBody.angularDrag = 1.0f;
        rigidBody.constraints = RigidbodyConstraints.None;

        // TODO(oliver): move this into dicce and card classes
        /*
        CurrentHeld.IsInAir = true;
        if (CurrentHeld)
        {
            CurrentHeld.gameObject.layer = (int)Mathf.Log(DiceSettings.Instance.RollingLayer.value, 2);
            // ExternalLogger.Instance.DieEventLog("dice_dropped", grabbedDie.MyType.ToString(), grabbedDie);
       
            if (CheatDetector.Instance != null)
            {
                Vector3 mouseVelocity3D = new Vector3(mouseVelocity.x, 0, mouseVelocity.y);
                CheatDetector.Instance.DetectCheatRoll(mouseVelocity3D, Vector3.zero);
            }

        }
        else
        {
            CurrentHeld.gameObject.layer = (int)Mathf.Log(AnimationSettings.Instance.InteractionLayer.value, 2);
        }

        Card grabbedCard = CurrentHeld as Card;
        if (grabbedCard != null)
        {
            UIManager.Instance.CardReleased(grabbedCard);
            ExternalLogger.Instance.CardEventLog("card_dropped", grabbedCard.GetCardName(), grabbedCard);
        }
        */

        if (CurrentHeld.IsOverPlacementPos()) 
        {
            Debug.Log("IsOverPlacementPos");
            if (CurrentHeld.GetPlacementPosIsOver().IsValidPlacement(CurrentHeld)) {
                Debug.Log("IsValidPlacement");
                PlacementPosition pos = CurrentHeld.GetPlacementPosIsOver();
                StartCoroutine(WaitAFrameAndHandlePlaced(CurrentHeld, pos));
            }
        }
        else
        {
            StartCoroutine(WaitAFrameAndHandleReleased(CurrentHeld));
        }
        CurrentHeld = null;
    }
    
    public void CheckShouldRotateObject()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (CurrentHeld == null)
            {
                return;
            }
            CurrentHeld.transform.Rotate(Vector3.up * 90f * Time.deltaTime, Space.Self);
        }
        if (DevSettings.Instance.DevMode)
        {
            if (Input.GetKey(KeyCode.L))
            {
                // TODO(oliver): support dev testing rolls of 1. 
                // grabbedDie.FacesLogic.RotateToFaceWithValue(1);
            }
        }
    }


    private void ThrowObject(Rigidbody rigidBody)
    {
        rigidBody.transform.parent = null;
        rigidBody.useGravity = true;

        rigidBody.drag = 1.0f;
        rigidBody.angularDrag = 1.0f;
        rigidBody.constraints = RigidbodyConstraints.None;

        CurrentHeld.IsInAir = true;

        Vector3 direction = (GetRollTargetPosition() - rigidBody.position).normalized;
        float rollForce = Random.Range(DiceSettings.Instance.RollForceRange.x, DiceSettings.Instance.RollForceRange.y);
        float spinForce = Random.Range(DiceSettings.Instance.RollSpinRange.x, DiceSettings.Instance.RollSpinRange.y);

        rigidBody.AddForce(direction * rollForce, ForceMode.Impulse);
        Vector3 randomTorque = new Vector3(
            Random.Range(-spinForce, spinForce),
            Random.Range(-spinForce, spinForce),
            Random.Range(-spinForce, spinForce)
        );
        rigidBody.AddTorque(randomTorque, ForceMode.Impulse);

        /*
        if (grabbedDie)
        {
            CurrentHeld.gameObject.layer = (int)Mathf.Log(AnimationSettings.Instance.RollingLayer.value, 2);
            ExternalLogger.Instance.DieEventLog("dice_rolled", grabbedDie.MyType.ToString(), grabbedDie);
        }
        */

        StartCoroutine(WaitAFrameAndHandleReleased(CurrentHeld));
        CurrentHeld = null;
    }
    
    private Vector3 GetRollTargetPosition()
    {
        return Vector3.zero;
    }


    private IEnumerator WaitAFrameAndHandleReleased(MovableInteractable movableObj)
    {
        yield return new WaitForSeconds(0.1f);
        movableObj.OnReleased();
    }

    private IEnumerator WaitAFrameAndHandlePlaced(MovableInteractable movableObj, PlacementPosition pos)
    {
        Debug.Log("WaitAFrameAndHandlePlaced");
        yield return new WaitForSeconds(0.1f);
        movableObj.OnPlaced(pos);
    }
}

