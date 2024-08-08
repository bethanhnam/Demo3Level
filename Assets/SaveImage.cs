using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class SaveImage : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CopyImageToPersistentDataPathAndShowNotification("1v.jpg"));
    }

    private IEnumerator CopyImageToPersistentDataPathAndShowNotification(string fileName)
    {
        string streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, fileName);
        string persistentDataPath = Path.Combine(Application.persistentDataPath, fileName);

        if (System.IO.File.Exists(persistentDataPath))
        {
            ShowNoti(persistentDataPath);
        }
        else
        {
            Debug.LogError("Image not found at path: " + persistentDataPath);
            // Sử dụng UnityWebRequest để sao chép ảnh từ StreamingAssets
            using (UnityWebRequest uwr = UnityWebRequest.Get(streamingAssetsPath))
            {
                yield return uwr.SendWebRequest();
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(uwr.error);
                    yield break;
                }

                byte[] data = uwr.downloadHandler.data;
                System.IO.File.WriteAllBytes(persistentDataPath, data);
            }

            Debug.Log("Ảnh đã được sao chép tới: " + persistentDataPath);

            // Tạo thông báo
            ShowNoti(persistentDataPath);
        }
    }

    private static void ShowNoti(string persistentDataPath)
    {
        var notification = new AndroidNotification("Help her fix the house!", "Complete challenges to receive special rewards!", DateTime.Now.AddMinutes(1));
        notification.BigPicture = new BigPictureStyle
        {
            Picture = persistentDataPath,
        };
        AndroidNotificationCenter.SendNotification(notification, "d0");
    }
}


