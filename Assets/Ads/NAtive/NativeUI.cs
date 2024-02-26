using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoogleMobileAds.Api;

public class NativeUI : MonoBehaviour
{
    [SerializeField]
    private RawImage adChoice;
    [SerializeField]
    private RawImage img;

    [SerializeField]
    private TextMeshProUGUI txtTitle;
    [SerializeField]
    private TextMeshProUGUI txtDes;
    [SerializeField]
    private TextMeshProUGUI txtCallToAction;

    private NativeAd nativeAd;
    [SerializeField]
    private bool checkEndNAtive;

    public void Init(NativeAd n)
    {
        nativeAd = n;

        if (n == null)
        {
            Deactive();
        }

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        string t = nativeAd.GetHeadlineText();
        if (string.IsNullOrEmpty(t))
        {
            txtTitle.text = string.Empty;
        }
        else
        {
            txtTitle.text = t;
        }

        string d = nativeAd.GetBodyText();
        if (string.IsNullOrEmpty(d))
        {
            txtDes.text = string.Empty;
        }
        else
        {
            txtDes.text = d;
        }

        string c = nativeAd.GetCallToActionText();
        if (string.IsNullOrEmpty(c))
        {
            txtCallToAction.text = "GET IT!";
        }
        else
        {
            txtCallToAction.text = c;
        }

        Texture2D ac = nativeAd.GetAdChoicesLogoTexture();
        if (ac != null)
        {
            if (!adChoice.gameObject.activeSelf)
            {
                adChoice.gameObject.SetActive(true);
            }

            adChoice.texture = ac;

            float sc1 = ac.width / ac.height;
            if (sc1 < 1)
            {
                adChoice.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sc1 * 40);
                adChoice.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);
            }
            else
            {
                adChoice.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sc1 * 40);
                adChoice.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 40);
            }
        }
        else
        {
            if (adChoice.gameObject.activeSelf)
            {
                adChoice.gameObject.SetActive(false);
            }
        }

        List<Texture2D> i = nativeAd.GetImageTextures();

        if (i != null && i.Count > 0)
        {
            img.texture = i[0];
            float sc1 = i[0].width / i[0].height;
            float sc2 = 730f / 400f;
            if (sc1 < sc2)
            {
                img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sc1 * 730f);
                img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400f);
            }
            else
            {
                img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sc1 * 400f);
                img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 730f);
            }
        }

        if (!string.IsNullOrEmpty(t))
        {
            nativeAd.RegisterHeadlineTextGameObject(txtTitle.gameObject);
        }
        if (!string.IsNullOrEmpty(d))
        {
            nativeAd.RegisterBodyTextGameObject(txtDes.gameObject);
        }
        if (!string.IsNullOrEmpty(c))
        {
            nativeAd.RegisterCallToActionGameObject(txtCallToAction.gameObject);
        }

        if (ac != null)
        {
            nativeAd.RegisterAdChoicesLogoGameObject(adChoice.gameObject);
        }
        if (i != null && i.Count > 0)
        {
            nativeAd.RegisterImageGameObjects(new List<GameObject> { img.gameObject });
        }
    }

    public void Deactive()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
     AdsControl.Instance.NativeAdmobManager.CallWhenDestroyNative(checkEndNAtive);
    }

    private void OnDestroy()
    {
        Deactive();
    }
}
