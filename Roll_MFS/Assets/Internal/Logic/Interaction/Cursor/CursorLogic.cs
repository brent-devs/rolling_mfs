using System.Collections;
using UnityEngine;

public enum CursorState
{
    Hidden,
    Visible,
    CanGrabItem,
    CanClickItem,
    Holding
}

public class CursorLogic : MonoBehaviour
{
    public static CursorLogic Instance { get; private set; }

    [Header("Traits")]
    public CursorState State = CursorState.Visible;

    [Header("Dependencies")]
    public CursorDisplayHandling cursorDisplaySettings;

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
        UpdateState(State);
    }

    private void UpdateState(CursorState newState)
    {
        State = newState;
        cursorDisplaySettings.UpdateCursorDisplay(newState);
    }

    public void ShowCursor()
    {
        UpdateState(CursorState.Visible);
    }

    public void ShowHoldingCursor()
    {
        UpdateState(CursorState.Holding);
    }

    public void ShowClickable()
    {
        UpdateState(CursorState.CanClickItem);
    }

    public void HideCursor()
    {
        UpdateState(CursorState.Hidden);

    }

    public void ShowCursorAfterPanelClickAfterStandardDelay()
    {
        // Cursor Handling
        HideCursor();
        // StartCoroutine(ShowCursorWithDelay(AnimationSettings.Instance.CursorHideTimeAfterPanelClick));
    }

    private IEnumerator ShowCursorWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateState(CursorState.Visible);
    }

    public void CenterCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
    }
}