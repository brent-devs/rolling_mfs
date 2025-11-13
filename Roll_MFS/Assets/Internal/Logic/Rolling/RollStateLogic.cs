using UnityEngine;

public class RollStateLogic : MonoBehaviour
{
    public static RollStateLogic Instance { get; private set; }

    public Dice CurrentDice { get; private set; }

    public bool IsRolling { get; private set; }

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

    public void SetCurrentDice(Dice die)
    {
        CurrentDice = die;
    }

    public void SetIsRolling(bool isRolling) {
        IsRolling = isRolling;
    }

    public void ClearCurrentDice()
    {
        CurrentDice = null;
    }
}

