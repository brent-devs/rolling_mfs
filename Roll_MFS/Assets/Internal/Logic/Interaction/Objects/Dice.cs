using System.Collections;
using UnityEngine;

public enum DieState {
    Resting,
    InHand,
    Rolling,
    Spinning,
}

public enum DieType {
    Six,
    Eight,
    Ten,
    Twenty,
}

public class Dice : PhysicsInteractable
{
    [Header("Setup")]
    public bool IsDisplayDice = false;
    public bool IsDisplayDiceCheckingResult = false;

    [Header("Traits")]
    public int CurrMax = 20;
    public DieState CurrState = DieState.Resting;
    public DieType MyType = DieType.Twenty;
    private bool hanging = false;

    [Header("Dependencies")]
    public DieFacesLogic FacesLogic;
    public MeshRenderer renderer;

    [Header("DiceRollPhysics")]
    public float DragForRoll = 0.5f;

    void Update()
    {
        if (!ShouldComputeStop)
        {
            CheckHangTime();
        }
        
        // Check if dice should transition from rolling to spinning
        if (CurrState == DieState.Rolling)
        {
            CheckRollingToSpinningTransition();
        }
        
        // Check if dice should transition from spinning to resting
        if (CurrState == DieState.Spinning)
        {
            CheckSpinningToRestingTransition();
        }
    }


    private void CheckHangTime()
    {
        if (!hanging)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity = Vector3.zero;
                rb.useGravity = false;
                hanging = true;
                StartCoroutine(HangTime(DiceSettings.Instance.DiceFloatTime));
            }
        }
    }

    private IEnumerator HangTime(float time)
    {
        yield return new WaitForSeconds(time);
        rb.useGravity = true;
        hanging = false;
        ShouldComputeStop = true;
    }

    private void CheckRollingToSpinningTransition()
    {
        if (rb.velocity.magnitude < 0.15f)
        {
            CurrState = DieState.Spinning;
            rb.angularDrag = DiceSettings.Instance.AngularDragOfDiceDuringSpin;
        }
    }

    private void CheckSpinningToRestingTransition()
    {
        if (rb.angularVelocity.magnitude < 0.05f)
        {
            OnStoppedEvent.Invoke();
        }
    }

    /*
    public void SwapToGlitch()
    {
        if (renderer == null)
        {
            return;
        }
        Material[] mats = renderer.materials;
        mats[0] = DiceSettings.Instance.DiceGlitchMat;
        renderer.materials = mats;
        /AfterImageFPS fps = GetComponent<AfterImageFPS>();
        if (fps != null && FeatureFlags.Instance.vfrVFXFlag)
        {
            fps.TurnOff();
        }
    }
    */

    /*
    public IEnumerator MoveToDestination(Vector3 destination, bool shouldBeInteractableAtEnd)
    {
        if (this == null)
        {
            yield return null;
        }
        else
        {
            rb.isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer(CursorSettings.Instance.HideInteractionLayer);

            Vector3 startingPosition = transform.position;

            float elapsedTime = 0f;
            float travelTime = DiceSettings.Instance.DiceFlyTime;

            float arcHeight = 0.15f;
            while (elapsedTime < travelTime)
            {
                float t = elapsedTime / travelTime;

                Vector3 currentPos = Vector3.Lerp(startingPosition, destination, t);

                float heightOffset = Mathf.Sin(t * Mathf.PI) * arcHeight;
                currentPos.y += heightOffset;

                transform.position = currentPos;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = destination;
            rb.isKinematic = false;

            if (shouldBeInteractableAtEnd)
            {
                gameObject.layer = LayerMask.NameToLayer(CursorSettings.Instance.InteractionLayer);
            }
        }
    }
    */

    public void TriggerResultFinder()
    {

    }

    private void SetupEndOfRollPhysics()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        IsInAir = true;
        // TODO(oliver): change to  layer
        gameObject.layer = Mathf.RoundToInt(Mathf.Log(DiceSettings.Instance.InteractionLayer.value, 2));
    }
    
    private void SetupEndOfTurnPhysics()
    {
        gameObject.layer = Mathf.RoundToInt(Mathf.Log(DiceSettings.Instance.InteractionLayer.value, 2));
    }

    private void SetupRollingPhysics()
    {
        IsInAir = true;
        rb.useGravity = true;
        rb.angularDrag = DiceSettings.Instance.AngularDragOfDiceDuringRoll;
        gameObject.layer = Mathf.RoundToInt(Mathf.Log(DiceSettings.Instance.RollingLayer.value, 2));
    }

    public void TriggerRollEvent()
    {
        CurrState = DieState.Rolling;
        SetupRollingPhysics();
        HandleRollLogic();
    }


    public void TriggerRollEndEvent()
    {
        CurrState = DieState.Resting;
        SetupEndOfRollPhysics();
        HandleEndOfRollLogic();
    }

    public void SetupRollFinishedPhysics()
    {
        IsInAir = false;
        CurrState = DieState.Resting;
        rb.angularDrag = DiceSettings.Instance.AngularDragOfDiceDuringRoll;
    }

    public void TriggerLiftEvent()
    {
        CurrState = DieState.InHand;
        SetupLiftPhysics();
        HandleLiftLogic();
    }

    private void SetupLiftPhysics()
    {
        rb.drag = DiceSettings.Instance.DragOfDiceDuringHold;
        rb.angularDrag = DiceSettings.Instance.AngularDragOfDiceDuringHold;
        gameObject.layer = (int)Mathf.Log(DiceSettings.Instance.HoldingLayer.value, 2);
    }

    private void HandleEndOfRollLogic()
    {
        RollHandling.Instance.HandleDiceRollStopped(this);
    }


    private void HandleLiftLogic()
    {
        RollHandling.Instance.HandleBeginDiceHold(this);
    }

    private void HandleRollLogic()
    {
        RollHandling.Instance.HandleDiceRolled(this);
        InteractionHandling.Instance.CurrState = InteractionState.None; 
    }
}
