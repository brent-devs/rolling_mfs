using UnityEngine;

public class Session : MonoBehaviour
{
    public static Session Instance { get; private set; }

    public SessionSettings Settings;
    [SerializeReference]
    public GameplayLogic GameplayLogic;

    [SerializeReference]
    public CardHandling CardHandling;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (Settings == null)
        {
            Settings = new SessionSettings();
        }
        InitializeGameplayLogic();
    }

    private void InitializeGameplayLogic()
    {
        switch (Settings.GameMode)
        {
            case GameModes.Classic:
                GameplayLogic = new ClassicGameplayLogic(this);
                break;
            default:
                Debug.LogError($"No GameplayLogic implementation for {Settings.GameMode}");
                break;
        }

        GameplayLogic?.Initialize();
    }

    public void RestartSession()
    {
        InitializeGameplayLogic();
    }
}

