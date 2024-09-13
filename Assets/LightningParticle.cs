using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningParticle : MonoBehaviour
{
    
    public ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        // Bắt đầu Coroutine
        StartCoroutine(RepeatTask());
    }

    IEnumerator RepeatTask()
    {
        while (true)
        {
            active();
            yield return new WaitForSeconds(120); // Đợi 2 phút (120 giây)
            Deactive();
            yield return new WaitForSeconds(120); // Đợi 2 phút (120 giây)
        }
    }
    public void active()
    {
        particleSystem.gameObject.SetActive(true);
    }
    public void Deactive()
    {
        particleSystem.gameObject.SetActive(false);
    }
}
