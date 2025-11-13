using UnityEngine;


public class DevSettings : MonoBehaviour
{
    public static DevSettings Instance { get; private set; }

    [Header("Dev Flags")]
    public bool ShouldLogRoutines;
    public bool ShouldLogEverything;
    public bool ShouldReturnToPlayerTurn;
    public bool ShouldEnemyForceRollOne;
    public bool FastMode;
    public bool CardTestMode;
    public bool DevMode;

    public bool CanSpawnInModifiers;
    public bool UsePredeterminedCards;

    public bool IsCinematicMode = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (DevMode && Input.GetKeyDown(KeyCode.F))
        {
            
        }
        if (DevMode && Input.GetKeyDown(KeyCode.Z))
        {

        }
        if (DevMode && Input.GetKeyDown(KeyCode.C))
        {
            IsCinematicMode = !IsCinematicMode;
            if (IsCinematicMode)
            {
                
            }
            else
            {
                
            }
        }
        if (DevMode && Input.GetKeyDown(KeyCode.Alpha1))
        {
        }
        if (DevMode && Input.GetKeyDown(KeyCode.Alpha2))
        {
        }
        if (DevMode && Input.GetKeyDown(KeyCode.Alpha3))
        {
        }
        if (DevMode && Input.GetKeyDown(KeyCode.Alpha0))
        {
        }
    }
}