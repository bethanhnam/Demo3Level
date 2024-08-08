using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SaveImage : MonoBehaviour
{
    private void Awake()
    {
        ShowNotification();
    }

    private void ShowNotification()
    {// Get the path to the image in StreamingAssets
        string streamingAssetsPath = "jar:file://" + Application.dataPath + "!/assets/1v.jpg";

        if (!File.Exists(streamingAssetsPath))
        {
            Debug.LogError("Image not found in StreamingAssets: " + streamingAssetsPath);
            return;
        }

        // Read the image file from StreamingAssets
        byte[] bytes = File.ReadAllBytes(streamingAssetsPath);

        // Save the image to Application.persistentDataPath
        string imagePath = Path.Combine(Application.persistentDataPath, "1v.jpg");
        File.WriteAllBytes(imagePath, bytes);

        // Debug the path where the image is saved
        Debug.Log("Image saved to: " + imagePath);

        // Create and show notification
        var notification = new AndroidNotification
        {
            Title = "Image Notification",
            Text = "Using image from StreamingAssets",
            FireTime = System.DateTime.Now.AddMinutes(1),
            LargeIcon = imagePath,
            BigPicture = new BigPictureStyle
            {
                Picture = imagePath
            }
        };
        AndroidNotificationCenter.SendNotification(notification, "d0");
    }
}
    

