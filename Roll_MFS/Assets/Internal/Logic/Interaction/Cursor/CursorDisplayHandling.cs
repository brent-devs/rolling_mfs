using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CursorDisplayHandling : MonoBehaviour
{
    public static CursorDisplayHandling Instance { get; private set; }

    [Header("Dependencies")]
    public Sprite Pointer = null;
    public Sprite Grabbing = null;
    public Sprite Clicker = null;
    public Image CursorImage;

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

    public void UpdateCursorDisplay(CursorState newState)
    {
        switch (newState)
        {
            case CursorState.Hidden:
                CursorImage.enabled = false;
                break;

            case CursorState.Visible:
                CursorImage.enabled = true;
                CursorImage.sprite = Pointer;
                break;

            case CursorState.CanGrabItem:
                CursorImage.enabled = true;
                CursorImage.sprite = Clicker;
                break;

            case CursorState.CanClickItem:
                CursorImage.enabled = true;
                CursorImage.sprite = Clicker;
                break;

            case CursorState.Holding:
                CursorImage.enabled = true;
                CursorImage.sprite = Grabbing;
                break;
        }
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (CursorImage.enabled)
        {
            CursorImage.rectTransform.position = Input.mousePosition;
        }
    }
}