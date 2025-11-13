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

    // Utilies
    /*
    public Vector3 RaycastCursorPosOnBoard(float offset)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, CursorSettings.Instance.GrabbingCursorRange, AnimationSettings.Instance.BoardElementsLayers))
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
    */

    /*
    public Vector3 RaycastCursorPos()
    {
        Debug.Log("Camera" + Camera.main);
        Debug.Log("Input" + Input.mousePosition);
        Vector2 mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Debug.Log("mouseViewportPos:" + mouseViewportPos);

        // Viewport bounds
        float minX = CursorSettings.Instance.DiceGrabbingMinX;
        float maxX = CursorSettings.Instance.DiceGrabbingMaxX;
        float minY = CursorSettings.Instance.DiceGrabbingMinY;
        float maxY = CursorSettings.Instance.DiceGrabbingMaxY;

        // Clamp the viewport position
        mouseViewportPos.x = Mathf.Clamp(mouseViewportPos.x, minX, maxX);
        mouseViewportPos.y = Mathf.Clamp(mouseViewportPos.y, minY, maxY);

        // Convert clamped viewport pos back to screen space
        Vector3 clampedScreenPos = Camera.main.ViewportToScreenPoint(mouseViewportPos);

        // Create ray from clamped screen pos
        Ray ray = mainCamera.ScreenPointToRay(clampedScreenPos);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, CursorSettings.Instance.GrabbingCursorRange, AnimationSettings.Instance.AllNonUIElements))
        {
            if (hit.collider != null)
            {
                lastValidCursorPosForHover = new Vector3(hit.point.x, hit.point.y + CursorSettings.Instance.DiceOffsetFromBoardElements, hit.point.z);
                return lastValidCursorPosForHover;
            }
        }
        lastValidCursorPosForHover = ray.GetPoint(CursorSettings.Instance.GrabbingCursorRange);
        return lastValidCursorPosForHover;
    }
    /*

    // TODO(oliver): add in placement of objects onto items.
    /*
    public PlacementPosition RaycastCursorPosPlacementPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, CursorSettings.Instance.PlacementPositionRange, AnimationSettings.Instance.PlaceLayer))
        {
            if (hit.collider != null)
            {
                return hit.collider.gameObject.GetComponent<PlacementPosition>();
            }
        }

        return null;
    }
    */

}
