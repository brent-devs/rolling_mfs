using System;
using UnityEngine;

public abstract class MovableInteractable : MonoBehaviour, IInteractable
{
    [Header("Interactable Traits")]
    public bool IsInAir = false;
    private int originalLayer = -1;
    private bool hasCachedLayer;
    public int OriginalLayer => originalLayer;
    public String ItemName;
    
    public void CacheOriginalLayer()
    {
        if (hasCachedLayer)
        {
            return;
        }

        originalLayer = gameObject.layer;
        hasCachedLayer = true;
    }

    public void ApplyLayer(int layer)
    {
        CacheOriginalLayer();
        gameObject.layer = layer;
    }

    public void RestoreOriginalLayer()
    {
        if (!hasCachedLayer)
        {
            return;
        }

        gameObject.layer = originalLayer;
    }

    public abstract void Interact();
    public abstract void OnHover();
    public abstract void OnDeHover();
    public abstract bool CanInteract(int ownerTryingToInteract);

    public virtual void OnGrabbed()
    {
    }

    public virtual void OnReleased()
    {
    }

    public virtual void OnPlaced(PlacementPosition posToPlaceInto)
    {
    }


    public virtual Vector3 HandleHeldPosTracking()
    {
        return InteractionHandling.Instance.RaycastCursorPosOnBoard();
    }

    public virtual bool IsOverPlacementPos()
    {
        return false;
    }

    public virtual PlacementPosition GetPlacementPosIsOver()
    {
        return null;
    }
}

