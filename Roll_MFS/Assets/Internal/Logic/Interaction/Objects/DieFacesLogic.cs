using System.Collections;
using UnityEngine;

public class DieFacesLogic : MonoBehaviour
{
    [Header("Setup")]
    public Material textMaterialPreset;
    public DieFace[] Refs;
    public Transform[] Detectors;
    public int[] Values;
    public Dice Dice;


    void Start()
    {
        Dice = GetComponent<Dice>();
        if (Dice != null)
        {
            UpdateDiceText();
        }
    }

    void Update()
    {
        if (DevSettings.Instance.DevMode && Input.GetKeyDown(KeyCode.P)) {
            RotateToFaceWithValue(1);
        }
    }

    public int FindValueOfFaceFacingTop() {
        int indexOfResult = FindIndexOfFaceOfResult();
        return Values[indexOfResult];
    }

    public int FindIndexOfFaceOfResult()
    {
        int maxIndex = 0;
        for (int i = 1; i < Detectors.Length; i++)
        {
            if (Detectors[maxIndex].position.y < Detectors[i].position.y)
            {
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    public IEnumerator GlitchChangeRemainingFacesAboveValue(int value) {
        ChangeRemainingFacesAboveValue(value);
        yield return null;
    }

    public void ChangeRemainingFacesAboveValue(int value)
    {
        Dice.CurrMax = value;

        for (int i = 0; i < Values.Length; i++)
        {
            Refs[i].Hide();
        }

        for (int i = 0; i < Values.Length; i++)
        {
            if (Values[i] > value)
            {
                Values[i] = -1; // -1 is x value
            }
        }
        UpdateTextValues();

        for (int i = 0; i < Values.Length; i++)
        {
            Refs[i].Show();
        }
    }

    public void UpdateTextValues() {
        for (int i = 0; i < Values.Length; i++)
        {
            Refs[i].UpdateFontSettings(Dice.MyType, textMaterialPreset);
            Refs[i].UpdateValue(Values[i], Dice.MyType);
        }
    }

    public void UpdateDiceText()
    {
        UpdateTextValues();
    }


    public void RotateToFaceWithValue(int value)
    {
        int faceIndex = -1;
        for (int i = 0; i < Values.Length; i++)
        {
            if (Values[i] == value)
            {
                faceIndex = i;
                break;
            }
        }

        if (faceIndex == -1)
        {
            Debug.LogError("Value " + value + " not found on the dice.");
            return;
        }

        Transform faceDetector = Detectors[faceIndex];
        Vector3 faceDirection = transform.InverseTransformPoint(faceDetector.position).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(faceDirection, Vector3.up);

        transform.rotation = targetRotation;
    }

}
