using Coffee.UIExtensions;
using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameMap : MonoBehaviour
{
    public Slider collectSlider;
    public Slider heartSlider;

    public TextMeshProUGUI maxCollectText;
    public TextMeshProUGUI collectText;

    public clockFill clockFill;
    public HeartSlider HeartSlider;
    public Sprite sprite;

    public SkeletonGraphic skeleton;
    public GhostMinigame ghostSkeleton;
    public UIParticle UIParticle;

    public Image itemImage;

}
