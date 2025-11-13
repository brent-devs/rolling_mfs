using UnityEngine;

public class StringRepo : MonoBehaviour
{
    public static StringRepo Instance { get; private set; }

    [Header("Layer Setup")]
    public string ChapterOneCard_Upper = "Chapter 1";
    public string ChapterOneCard_Lower = "Winning";
    public string ChapterTwoCard_Upper = "Chapter 2";
    public string ChapterTwoCard_Lower = "Losing";
    public string ChapterThreeCard_Upper = "Chapter 3";
    public string ChapterThreeCard_Lower = "Desperation";
    public string ChapterFourCard_Upper = "Chapter 4";
    public string ChapterFourCard_Lower = "Hopelessness";

    [Header("Item Selection Setup")]
    public string Candle_Selection_Text = "when used gain an unlit candle";
    public string Gem_Selection_Text = "when used gain three cards";
    public string Lighter_Selection_Text = "when used light an unlit candle";
    public string Magnify_Selection_Text = "when used view opponent hand";
    public string Wishbone_Selection_Text = "when used change all dice to 20 sided dice";

    [Header("Item Setup")]
    public string Candle_Item_Text = "gain an unlit candle";
    public string Gem_Item_Text = "gain three cards";
    public string Lighter_Item_Text = "light an unlit candle";
    public string Magnify_Item_Text = "view opponent hand";
    public string Wishbone_Item_Text = "change all dice to 20 sided dice";

    [Header("Card Descriptions")]
    public string Reroll_Description = "Trigger an additional roll of the rolled die";
    public string Replace_6_Description = "Convert the rolled die to a six sided die";
    public string Replace_10_Description = "Convert the rolled die to a ten sided die";
    public string Angel_Description = "Prevent the roll of a 1 if a 1 is rolled";
    public string Treasure_Description = "Draw 2 cards from the box";
    public string Copycat_Description = "Copy the last card played this roll and return to hand";
    public string Bonfire_Description = "Destroy a random card in enemy hand";
    public string Torch_Description = "Destroy card's played by opponent this roll";
    public string Encage_Description = "Lock the platform of the die rolled for a turn";
    public string Kindling_Description = "Discard the leftmost card of your hand to light a candle";
    public string Rat_Description = "Steal opponent's candle for this roll";
    public string Prophet_Description = "Light a candle instead of extinguishing a candle if a 1 is rolled";

    [Header("Card Description Sizes")]
    public PanelSize Reroll_Description_Size = PanelSize.Small;
    public PanelSize Replace_6_Description_Size = PanelSize.Small;
    public PanelSize Replace_10_Description_Size = PanelSize.Small;
    public PanelSize Angel_Description_Size = PanelSize.Small;
    public PanelSize Treasure_Description_Size = PanelSize.Small;
    public PanelSize Copycat_Description_Size = PanelSize.Medium;
    public PanelSize Bonfire_Description_Size = PanelSize.Small;
    public PanelSize Torch_Description_Size = PanelSize.Small;
    public PanelSize Encage_Description_Size = PanelSize.Small;
    public PanelSize Kindling_Description_Size = PanelSize.Medium;
    public PanelSize Rat_Description_Size = PanelSize.Small;
    public PanelSize Prophet_Description_Size = PanelSize.Medium;

    [Header("Card Names")]
    public string Reroll_Name = "Reroll";
    public string Replace_6_Name = "Replace";
    public string Replace_10_Name = "Replace";
    public string Angel_Name = "Angel";
    public string Treasure_Name = "Treasure";
    public string Copycat_Name = "Copycat";
    public string Bonfire_Name = "Bonfire";
    public string Torch_Name = "Torch";
    public string Cage_Name = "Encage";
    public string Kindling_Name = "Kindling";
    public string Rat_Name = "Rat";
    public string Thief_Name = "Thief";
    public string Prophet_Name = "Prophet";

    [Header("Item Names")]

    public string Candle_Name = "Candle";
    public string Lighter_Name = "Lighter";
    public string Monocle_Name = "Magnifier";
    public string Gem_Name = "Gem";
    public string Wishbone_Name = "Wishbone";

    [Header("Dev Note")]

    public string Note_1 = "hey! your not supposed to see that chapter yet! ";
    public string Note_2 = "hey! your not supposed to see that chapter yet! ";
    public string Note_3 = "hey! your not supposed to see that chapter yet! ";

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
}