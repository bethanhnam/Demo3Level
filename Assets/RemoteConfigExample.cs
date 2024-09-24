using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RemoteConfigExample : MonoBehaviour
{
    //// Tham số Remote Config
    //private const string NumbersListKey = "Level_List";

    //// Danh sách các số từ Remote Config
    //private List<int> numbersList = new List<int>();

    //void Start()
    //{
    //    DontDestroyOnLoad(this);
    //    // Khởi tạo Firebase
    //    InitializeFirebase();
    //}

    //void InitializeFirebase()
    //{
    //    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
    //    {
    //        var dependencyStatus = task.Result;
    //        if (dependencyStatus == DependencyStatus.Available)
    //        {
    //            // Bắt đầu lấy Remote Config
    //            FetchRemoteConfig();
    //        }
    //        else
    //        {
    //            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
    //        }
    //    });
    //}

    //void FetchRemoteConfig()
    //{
    //    // Thiết lập các cấu hình mặc định nếu cần
    //    FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(new Dictionary<string, object>
    //    {
    //        { NumbersListKey, "1,2,3,4,5" } // Giá trị mặc định
    //    }).ContinueWith(task =>
    //    {
    //        // Lấy Remote Config từ Firebase
    //        FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(FetchComplete);
    //    });
    //}

    //void FetchComplete(Task fetchTask)
    //{
    //    if (fetchTask.IsCanceled)
    //    {
    //        Debug.LogError("Fetch was canceled.");
    //        return;
    //    }
    //    if (fetchTask.IsFaulted)
    //    {
    //        Debug.LogError("Fetch encountered an error.");
    //        return;
    //    }

    //    // Kích hoạt các giá trị Remote Config
    //    FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task =>
    //    {
    //        ApplyRemoteSettings();
    //    });
    //}

    //void ApplyRemoteSettings()
    //{
    //    // Lấy giá trị của numbers_list từ Remote Config
    //    string numberListString = FirebaseRemoteConfig.DefaultInstance.GetValue(NumbersListKey).StringValue;
    //    Debug.Log($"Fetched numbers_list: {numberListString}");

    //    // Kiểm tra nếu chuỗi không rỗng
    //    if (!string.IsNullOrEmpty(numberListString))
    //    {
    //        // Chuyển đổi chuỗi thành danh sách số nguyên
    //        numbersList = numberListString.Split(',').Select(int.Parse).ToList();
    //    }
    //    else
    //    {
    //        // Nếu không tìm thấy, sử dụng danh sách hiện tại từ các GameObject
    //        numbersList = Enumerable.Range(1, 30).ToList();
    //        Debug.Log("Using default numbers list based on GameObjects count.");
    //    }

    //    // Kích hoạt các GameObject dựa trên danh sách số
    //    ActivateGameObjects();
    //}

    //void ActivateGameObjects()
    //{
    //    //// Tắt tất cả các GameObject trước
    //    //foreach (var obj in gameObjects)
    //    //{
    //    //    obj.SetActive(false);
    //    //}

    //    // Kích hoạt các GameObject theo danh sách số
    //    foreach (int number in numbersList)
    //    {
    //        // Kiểm tra để tránh lỗi ngoài phạm vi
    //        if (number - 1 < 30 && number - 1 >= 0)
    //        {
    //            Debug.LogWarning($"Number {number} is fetch.");
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"Number {number} is out of range of the GameObjects list.");
    //        }
    //    }
    //}
}
