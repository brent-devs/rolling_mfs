using System.Collections.Generic;
using UnityEngine;
public class GameSideHandler : MonoBehaviour
{
    public static GameSideHandler Instance { get; private set; }

    [Header("Dependencies")]
    public List<PlayerSiderHandler> Handlers = new List<PlayerSiderHandler>();

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


    public PlayerSiderHandler GetPlayerSiderHandler(int playerIndex)
    {
        for (int index = 0; index < Handlers.Count; index++)
        {
            if (Handlers[index].Owner == playerIndex)
            {
                return Handlers[index];
            }
        }
        Debug.LogError("No Player Handler for this player index: " + playerIndex);
        return null;
    }
}
