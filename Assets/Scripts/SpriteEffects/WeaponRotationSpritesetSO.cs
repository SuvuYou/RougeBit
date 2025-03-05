using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponRotationSpritesetSO")]
public class WeaponRotationSpritesetSO : ScriptableObject
{
    [Header("Directions")]
    public Sprite UpSprite;
    public Sprite RightSprite;
    public Sprite DownSprite;

    public Sprite DownRightSprite30, DownRightSprite45, DownRightSprite60;
    public Sprite UpRightSprite30, UpRightSprite45, UpRightSprite60;
}
