using Shapes;
using SmoothShakeFree;
using UnityEngine;

public class ActionGroupDisplay : MonoBehaviour
{
    [Header("Setup")]
    public GameObject outline;
    public GameObject icon;
    public SmoothShake shaker;
    public Rectangle rect;
    public PlacementPosition placementPos;

    private float dashOffset = 0f;

    public void Show()
    {
        outline.SetActive(true);
        icon.SetActive(true);
        shaker.StartShake();
    }

    public void Hide()
    {
        outline.SetActive(false);
        icon.SetActive(false);
    }

    void Update()
    {
        if (rect != null)
        {
            dashOffset += Time.deltaTime * 1.0f; // Adjust speed with multiplier
            rect.DashOffset = dashOffset;
        }
    }
}
