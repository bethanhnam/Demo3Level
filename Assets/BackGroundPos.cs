using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundPos : MonoBehaviour
{
    public GameObject image;
    public float imageHeight = 610f;

    void Start()
    {
        // Lấy kích thước của màn hình
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Tính tọa độ điểm giữa phía trên của màn hình
        Vector2 screenCenterTop = new Vector2(screenWidth / 2, screenHeight);

        // Chuyển đổi tọa độ từ màn hình sang tọa độ thế giới
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenCenterTop.x, screenCenterTop.y, Camera.main.nearClipPlane));

        // Tính toán vị trí để đặt hình ảnh
        float imageTopY = worldPosition.y;
        float imageBottomY = imageTopY - (imageHeight / 100f); // Chia cho 100 để chuyển từ pixel sang unit

        // Đặt hình ảnh ở vị trí đã tính toán
        image.transform.position = new Vector3(worldPosition.x, imageBottomY + (imageHeight / 200f), worldPosition.z);

        Debug.Log("Vị trí đặt hình ảnh: " + image.transform.position);
    }
}
