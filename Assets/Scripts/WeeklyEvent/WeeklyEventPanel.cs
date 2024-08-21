using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyEventPanel : MonoBehaviour
{
    public Button WeeklyEventButton;

    public RaceTrack raceTrack;
    // Start is called before the first frame update
    public void Appear()
    {
        GameManagerNew.Instance.Bg.sprite = GameManagerNew.Instance.sprites[0];
        GameManagerNew.Instance.Bg.gameObject.SetActive(true);
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
        DOVirtual.DelayedCall(1, () =>
        {
            raceTrack.LoadPos();
        });
    }
    public void Disappear()
    {
        this.gameObject.SetActive(false);
        GameManagerNew.Instance.Bg.gameObject.SetActive(false);
    }
}
