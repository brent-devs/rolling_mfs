using UnityEngine;

public class ClassicGameplayLogic : GameplayLogic
{
    public ClassicGameplayLogic(Session session) : base(session)
    {
    }

    public override void Initialize()
    {
        currentPlayerTurn = 0;
        session.Settings.SessionState = SessionStates.Playing;
    }

    public override int GetCurrentPlayerTurn()
    {
        return currentPlayerTurn;
    }

    public override void HandlePlayerHealthLost(int playerIndex, int healthToLose)
    {
        // TODO: Implement player health logic
        Debug.Log($"Player {playerIndex} lost {healthToLose} health");
    }

    public override void OnTurnStart()
    {
        // TODO: Implement turn start logic
        Debug.Log($"Turn started for player {currentPlayerTurn}");
    }

    public override void OnTurnEnd()
    {
        // TODO: Implement turn end logic
        currentPlayerTurn = (currentPlayerTurn + 1) % session.Settings.PlayerCount;
        Debug.Log($"Turn ended, next player: {currentPlayerTurn}");
    }

    public override void RollStarted()
    {
        Session.Instance.CardHandling.OrderHandling.SetupCardOrder(GetCurrentPlayerTurn());
    }

    public override void EndPlayerIndexTurn(int playerIndex)
    {
        // TODO: Implement turn start logic
        Debug.Log($"End turnfor player {playerIndex}");
    }

    public override void StartPlayerIndexTurn(int playerIndex)
    {
        // TODO: Implement turn start logic
        Debug.Log($"Start turn for player {playerIndex}");
    }

    public override void RollStopped()
    {
        // TODO: Implement turn start logic
        Debug.Log("Roll Finished");
    }
}

