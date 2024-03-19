using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckLoadingAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public void DuckSound()
    {
        AudioManager.instance.PlaySFX("Duck");
    }
}
