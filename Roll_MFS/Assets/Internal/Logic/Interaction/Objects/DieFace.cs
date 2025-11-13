using UnityEngine;
using TMPro;

public class DieFace : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TMP_Text textObj;

    void Start() {
        textObj = GetComponent<TMP_Text>();
    }

    public void UpdateValue(int val, DieType dieType) {
        textObj = GetComponent<TMP_Text>();

        if (val != -1)
        {
            string text = val.ToString();

            // Handle Dice Edge Cases
            if (dieType == DieType.Ten && val == 10)
            {
                text = "0";
            }
            textObj.text = text;
        }
        else
        {
            textObj.text = "X";
        }

    }

    public void Hide() {
        if (textObj != null) 
        {
            textObj.enabled = false;
        }
    }

    public void Show() {
        if (textObj != null) 
        {
            textObj.enabled = true;
        }
    }

    public void UpdateFontSettings(DieType dieType, Material diceFontMat) {
        if (textObj != null) {
            textObj.font = DiceSettings.Instance.diceFontAsset;
            textObj.fontMaterial = diceFontMat;
            textObj.characterSpacing = 10; 
            switch (dieType)
            {
                case DieType.Six:
                    textObj.fontSize = DiceSettings.Instance.SixFontSize;
                    textObj.fontStyle = TMPro.FontStyles.Normal;
                    break;
                case DieType.Eight:
                    textObj.fontSize = DiceSettings.Instance.EightFontSize;
                    textObj.fontStyle = TMPro.FontStyles.Normal;
                    break;
                case DieType.Ten:
                    textObj.fontSize = DiceSettings.Instance.TenFontSize;
                    textObj.fontStyle = TMPro.FontStyles.Normal;
                    break;
                case DieType.Twenty:
                    textObj.fontSize = DiceSettings.Instance.TwentyFontSize;
                    textObj.fontStyle = TMPro.FontStyles.Normal;
                    break;
            }
        }
    }
}
