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
        EndPlayerIndexTurn(currentPlayerTurn); 
    }

    public override void RollStarted()
    {
        Session.Instance.CardHandling.OrderHandling.SetupCardOrder(GetCurrentPlayerTurn());
    }

    public override void EndPlayerIndexTurn(int playerIndex)
    {
        Debug.Log($"End turnfor player {playerIndex}");
        ClassicOnTurnEndLogic();
    }

    public override void StartPlayerIndexTurn(int playerIndex)
    {
        // TODO: Implement turn start logic
        Debug.Log($"Start turn for player {playerIndex}");
        ClassicOnTurnStartLogic();
    }

    public override void RollStopped()
    {
        // TODO: Implement turn start logic
        Debug.Log("Roll Finished");
        OnTurnEnd();
    }


    private void ClassicOnTurnEndLogic()
    {
        IncrementPlayerTurn();
        StartPlayerIndexTurn(currentPlayerTurn);
    }


    private void ClassicOnTurnStartLogic()
    {
       SetupInteractionRaycastersForTurn(currentPlayerTurn);
    }

    private void SetupInteractionRaycastersForTurn(int turn)
    {
        if (session.Settings.IsSinglePlayer)
        {
            if (turn == 0)
            {
                InteractionHandling.Instance.CurrState = InteractionState.MyTurn;
            }
            else
            {
                InteractionHandling.Instance.CurrState = InteractionState.None;
            }
        }
    }

    public override void CardDropped(Card card)
    {
        HandleCardDroppedLogic(card);
    }

    private void HandleCardDroppedLogic(Card card)
    {
        PlayerSiderHandler handler = GameSideHandler.Instance.GetPlayerSiderHandler(card.Owner); 
        if (handler == null)
        {
            Debug.LogError("Handler does not exist");
        }

        handler.HandHandler.MoveCardToHand(card);
    }

    public override void CardGrabbed(Card card)
    {
        HandleCardGrabbedLogic(card);
    }

    private void HandleCardGrabbedLogic(Card card)
    {
        PlayerSiderHandler handler = GameSideHandler.Instance.GetPlayerSiderHandler(card.Owner); 
        if (handler == null)
        {
            Debug.LogError("Handler does not exist");
        }

        handler.HandHandler.TryRemoveCard(card);
    }
}

