using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingAxe : MonoBehaviour
{
    public void ChoppingWoodSound()
    {
        AudioManager.instance.PlaySFX("ChopingWood");
    }
    public void WoodBreak()
    {
        AudioManager.instance.PlaySFX("WoodBreak");
    }
}
