using UnityEngine;

[CreateAssetMenu(fileName = "CardRarityConfig", menuName = "ScriptableObjects/Upgrades/CardRarityConfig")]
public class CardRarityConfig : ScriptableObject
{
    public Sprite RarityTitleSprite;
    public GameObject BackgroundSprite;
    public Color TextTintColor;
}