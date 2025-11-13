using UnityEngine;

[System.Serializable]
public abstract class GameplayLogic
{
    protected Session session;
    public int currentPlayerTurn = 0;

    protected GameplayLogic()
    {
    }

    public GameplayLogic(Session session)
    {
        this.session = session;
    }

    public abstract int GetCurrentPlayerTurn();
    
    public abstract void HandlePlayerHealthLost(int playerIndex, int healthToLose);

    public abstract void Initialize();
    
    public abstract void OnTurnStart();

    public abstract void OnTurnEnd();

    public abstract void RollStarted();
    public abstract void RollStopped();

    public abstract void EndPlayerIndexTurn(int playerIndex);
    public abstract void StartPlayerIndexTurn(int playerIndex);

}

