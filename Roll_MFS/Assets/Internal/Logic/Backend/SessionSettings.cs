[System.Serializable]
public class SessionSettings
{
    public GameModes GameMode;
    public SessionStates SessionState;
    public int PlayerCount;
    public bool IsSinglePlayer;

    public SessionSettings()
    {
        GameMode = GameModes.Classic;
        SessionState = SessionStates.Idle;
        PlayerCount = 2;
        IsSinglePlayer = true;
    }
}

