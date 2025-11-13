using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardFrontDisplay : MonoBehaviour
{
    [Header("Card Dependencies")]
    public Image primaryImage;
    public TMP_Text textField;
    public Image cardFrontImage;

    public void SetupCardType(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.ReplaceSix:
                textField.text = StringRepo.Instance.Replace_6_Name;
                //  primaryImage.sprite = CardEffectRepo.Instance.ReplaceSixImage;
                break;
            case CardType.ReplaceTen:
                textField.text = StringRepo.Instance.Replace_10_Name;
                //  primaryImage.sprite = CardEffectRepo.Instance.ReplaceTenImage;
                break;
            case CardType.Angel:
                textField.text = StringRepo.Instance.Angel_Name;
                //  primaryImage.sprite = CardEffectRepo.Instance.AngelImage;
                break;
            case CardType.Thief:
                textField.text = StringRepo.Instance.Thief_Name;
                //  primaryImage.sprite = CardEffectRepo.Instance.RatImage;
                break;
            case CardType.CopyCat:
                textField.text = StringRepo.Instance.Copycat_Name;
                //  primaryImage.sprite = CardEffectRepo.Instance.CopyCatImage;
                break;
            case CardType.Cage:
                textField.text = StringRepo.Instance.Cage_Name;
                //  primaryImage.sprite = CardEffectRepo.Instance.CageImage;
                break;
            case CardType.Torch:
                textField.text = StringRepo.Instance.Torch_Name;
                //  primaryImage.sprite = CardEffectRepo.Instance.TorchImage;
                break;
            case CardType.Reroll:
                textField.text = StringRepo.Instance.Reroll_Name;
                //  primaryImage.sprite = CardEffectRepo.Instance.RerollImage;
                break;
            case CardType.Treasure:
                textField.text = StringRepo.Instance.Treasure_Name;
                // primaryImage.sprite = CardEffectRepo.Instance.TreasureImage;
                break;
            case CardType.Bonfire:
                textField.text = StringRepo.Instance.Bonfire_Name;
                //  primaryImage.sprite = CardEffectRepo.Instance.BonfireImage;
                break;
        }
    }

    public IEnumerator CardFrontFade()
    {
        float duration = CardSettings.Instance.CardFadeTime;
        float elapsed = 0f;

        Color startTextColor = textField.color;
        Color startPrimaryColor = primaryImage.color;
        Color startFrontColor = cardFrontImage.color;

        Color targetColor = Color.black;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            textField.color = Color.Lerp(startTextColor, targetColor, t);
            primaryImage.color = Color.Lerp(startPrimaryColor, targetColor, t);
            cardFrontImage.color = Color.Lerp(startFrontColor, targetColor, t);

            yield return null;
        }

        textField.color = targetColor;
        primaryImage.color = targetColor;
        cardFrontImage.color = targetColor;
    }
}