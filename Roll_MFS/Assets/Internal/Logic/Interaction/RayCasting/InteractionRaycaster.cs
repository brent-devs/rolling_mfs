using UnityEngine;

public class InteractionRaycaster : MonoBehaviour
{
    [Header("Dependencies")]
    public LayerMask InteractionLayerForRaycaster;


    [Header("Traits")]
    private Vector3 lastValidCursorPosForHover;

    public void Raycast()
    {
        Ray ray = InteractionHandling.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask InteractionLayer = InteractionLayerForRaycaster;

        if (InteractionHandling.Instance.IsUIBlockingRaycast())
        {
            ResetBackToNeutral();
            return;
        }

        if (CursorLogic.Instance.State == CursorState.Visible || CursorLogic.Instance.State == CursorState.CanClickItem)
        {
            if (Physics.Raycast(ray, out hit, InteractionSettings.Instance.StandardCursorRange, InteractionLayer))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                    {
                        if (interactObj.CanInteract(InteractionHandling.Instance.Owner))
                        {
                            if (InteractionHandling.Instance.CurrentHoveredInteractable != interactObj)
                            {
                                InteractionHandling.Instance.CurrentHoveredInteractable = interactObj;
                                InteractionHandling.Instance.CurrentHoveredInteractable.OnHover();
                            }

                            if (Input.GetMouseButtonDown(0))
                            {
                                InteractionHandling.Instance.CurrentHoveredInteractable.Interact();
                                InteractionHandling.Instance.CurrentHoveredInteractable = null;
                            }
                            return;
                        }
                    }
                }
            }
        }
        ResetBackToNeutral();
    }

    private void ResetBackToNeutral()
    {
        if (CursorLogic.Instance.State != CursorState.Hidden)
        {
            CursorLogic.Instance.ShowCursor();
            InteractionHandling.Instance.CurrentHoveredInteractable = null;
        }
    }
}
