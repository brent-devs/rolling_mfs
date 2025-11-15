using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MovableObjectAction {
    Rotate,
    Throw,
    Roll,
}

public enum HoverType
{
    PhysicsRaise,
    VisualRaise,
}

[RequireComponent(typeof(Rigidbody))]
public class PhysicsInteractable : MovableInteractable
{
    [Header("Setup")]
    public UnityEvent OnGrabbedEvent;
    public UnityEvent OnReleaseEvent;
    public UnityEvent OnStoppedEvent;
    public UnityEvent OnCollisionEvent;
    public UnityEvent OnPlacedEvent;
    public Rigidbody rb;
    public float HoverTextOffset = 0.08f;


    [Header("Dependencies")]
    [SerializeField] private Transform visual;

    [Header("Traits")]
    public int Owner = -1;


    [Header("Lift Physics Setup")]
    public float liftY = 0.1f;
    public float LiftedDrag = 7.0f;
    public float LiftedAngularDrag = 2.0f;
    public float HalfHeight = 2.0f;


    [Header("IsInAir Setup")]
    public float objectStoppedThreshold = 0.02f;
    public bool ShouldComputeStop = true;
    public bool RotateToCamInAir = false;
    public Vector3 EulersOffsetForRotate = Vector3.zero;


    public Vector3 initialHoverPos;


    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        visual = transform.Find("Visual");
    }

    public override void Interact()
    {
        if (gameObject == null)
        {
            return;
        }

        InteractionHandling.Instance.GrabObject(this);
        // GrabHandler.Instance.GrabObject(this);
    }

    public void OnRelease()
    {
        OnReleased();
        RestoreOriginalLayer();
    }

    public override void OnGrabbed()
    {
        base.OnGrabbed();
        rb.useGravity = false;
        OnGrabbedEvent?.Invoke();
    }

    public override void OnReleased()
    {
        base.OnReleased();
        OnReleaseEvent?.Invoke();
    }

    public override void OnHover()
    {
        if (gameObject == null)
        {
            return;
        }
        initialHoverPos = transform.position;
        visual.position = initialHoverPos + new Vector3(0.0f, liftY, 0.0f);

        CursorLogic.Instance.ShowClickable();

    }

    public override void OnDeHover()
    {
        if (this == null)
        {
            return;
        }
        if (gameObject == null)
        {
            return;
        }
        visual.position = initialHoverPos;
    }

    public override bool CanInteract(int OwnerTryingToInteract)
    {
        if (Owner == -1)
        {
            return true;
        }
        return Owner == OwnerTryingToInteract;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Portal" || collision.collider.tag == "Panel")
        {
            return;
        }
        /*
        if (DevSettings.Instance.ShouldLogEverything)
        {
            Debug.Log(gameObject.name + " collided with something");
            Debug.Log("Collision Pos:" + collision.contacts[0].point.y);
            Debug.Log("Transform Pos:" + transform.position.y);
            Debug.Log("Impulse:" + collision.impulse);
            Debug.Log("Angular Velocity:" + rb.angularVelocity);
        }
        */
        HandleCollision(collision);
    }

    public virtual void HandleCollision(Collision collision)
    {

    }

    void OnCollisionStay(Collision collision)
    {
        /*
        if (DevSettings.Instance.ShouldLogEverything)
        {
            Debug.Log(gameObject.name + " collided with something");
            Debug.Log("Collision Pos:" + collision.contacts[0].point.y);
            Debug.Log("Transform Pos:" + transform.position.y);
            Debug.Log("Impulse:" + collision.impulse);
            Debug.Log("Angular Velocity:" + rb.angularVelocity);
        }
        */
        HandleCollision(collision);
    }

    public void OnStoppedMovingFromAir()
    {
        Debug.Log("In Here");
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        IsInAir = false;
        OnStoppedEvent?.Invoke();
    }

    public void CheckStoppedMovingInAir()
    {
        if ((rb.velocity.magnitude < objectStoppedThreshold) && (rb.angularVelocity.magnitude < objectStoppedThreshold) && ShouldComputeStop)
        {
            OnStoppedMovingFromAir();
        }
    }
}
