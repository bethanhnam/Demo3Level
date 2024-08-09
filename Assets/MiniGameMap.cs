using Coffee.UIExtensions;
using DG.Tweening;
using GoogleMobileAds.Ump.Api;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System;

public class MiniGameMap : MonoBehaviour
{
    public enum Minigame
    {
        Babycold,
        ScaryHouse
    }

    public Minigame minigame1;

    [ShowIf("IsScaryHouse")]
    public SkeletonGraphic skeleton;
    [ShowIf("IsScaryHouse")]
    public GhostMinigame ghostSkeleton;
    [ShowIf("IsScaryHouse")]
    public characterStepBack characterStepBack;
    [ShowIf("IsScaryHouse")]
    public Transform darkDefaultPos;
    [ShowIf("IsScaryHouse")]
    public SpriteRenderer itemImage;

    [ShowIf("IsBabyCold")]
    public HeartSlider HeartSlider;
    [ShowIf("IsBabyCold")]
    public ParticleSystem UIParticle;
    [ShowIf("IsBabyCold")]
    public Image itemImage1;
    [ShowIf("IsBabyCold")]
    public ParticleSystem snowParticle;

    // Phương thức kiểm tra điều kiện hiển thị
    private bool IsScaryHouse()
    {
        return minigame1 == Minigame.ScaryHouse;
    }
    private bool IsBabyCold()
    {
        return minigame1 == Minigame.Babycold;
    }
    public Slider collectSlider;
    public TextMeshProUGUI maxCollectText;
    public TextMeshProUGUI collectText;
    public clockFill clockFill;
    public Sprite sprite;
    public bool hasDone;

    public Transform collectTargetPos;

}
