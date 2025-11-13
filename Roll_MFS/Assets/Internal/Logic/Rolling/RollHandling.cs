using UnityEngine;

public class RollHandling : MonoBehaviour
{
    public static RollHandling Instance { get; private set; }

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

    public void HandleDiceRollStopped(Dice dice)
    {
        RollStateLogic.Instance.SetIsRolling(false);
        Session.Instance.GameplayLogic.RollStopped();
    }

    public void HandleBeginDiceHold(Dice dice)
    {
        RollStateLogic.Instance.SetCurrentDice(dice);
    }

    public void HandleDiceRolled(Dice dice)
    {
        RollStateLogic.Instance.SetCurrentDice(dice);
        RollStateLogic.Instance.SetIsRolling(true);
        Session.Instance.GameplayLogic.RollStarted();
    }


}

