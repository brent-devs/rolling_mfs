using System.Collections.Generic;
using UnityEngine;

public class CardPrefabRepo : MonoBehaviour
{
    public static CardPrefabRepo Instance { get; private set; }
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
    }

    public Vector3 faceUp;
    public Vector3 faceDown;

    [Header("Card Prefabs")]
    public List<GameObject> GenericCards = new List<GameObject>();

    public GameObject PickRandomGenericCardPrefab()
    {
        if (GenericCards == null || GenericCards.Count == 0)
        {
            Debug.LogWarning("GenericCards list is empty or null.");
            return null;
        }

        int randomIndex = Random.Range(0, GenericCards.Count);
        return GenericCards[randomIndex];
    }
}  
