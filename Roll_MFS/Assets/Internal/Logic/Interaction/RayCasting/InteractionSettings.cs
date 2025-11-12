using UnityEngine;

public class InteractionSettings : MonoBehaviour
{
    public static InteractionSettings Instance { get; private set; }

    [Header("Traits")]
    public float StandardCursorRange = 3.0f;


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
}
