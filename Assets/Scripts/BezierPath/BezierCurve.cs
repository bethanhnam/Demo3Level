using System.Net;
using UnityEngine;

[System.Serializable]
public class BezierCurve
{
    public Vector3[] Points;

    public BezierCurve()
    {
        Points = new Vector3[4];
    }

    public void SetPosForWinUI(Vector3 startPoint, Vector3 endPoint)
    {
        // Điểm đầu
        Points[0] = startPoint;

        Points[1] = new Vector3(
            startPoint.x + (endPoint.x - startPoint.x) * 9f / 5f,
            startPoint.y + (endPoint.y - startPoint.y) * (-1f / 5f)
        );

        // Điểm thứ hai cách startPosition 4.5/5 quãng đường và độ cao 4/5 của endPosition
        Points[2] = new Vector3(
            startPoint.x + (endPoint.x - startPoint.x) * 6 / 5f,
            startPoint.y + (endPoint.y - startPoint.y) * 4f / 5f
        );

        // Điểm cuối
        Points[3] = endPoint;
    }

    public void SetPosForStarWinUI(Vector3 startPoint, Vector3 endPoint)
    {
        // Điểm đầu
        Points[0] = startPoint;

        Points[1] = new Vector3(
            startPoint.x - (startPoint.x - endPoint.x) * 4f / 5f,
            startPoint.y + (endPoint.y - startPoint.y) * (-3f / 5f)
        );

        // Điểm thứ hai cách startPosition 4.5/5 quãng đường và độ cao 4/5 của endPosition
        Points[2] = new Vector3(
            startPoint.x - (startPoint.x - endPoint.x) * 5 / 5f,
            startPoint.y + (endPoint.y - startPoint.y) * 4f / 5f
        );


        // Điểm cuối
        Points[3] = endPoint;
    }

    public void SetPosForDailyRewardGold(Vector3 startPoint, Vector3 endPoint)
    {
        // Điểm đầu
        Points[0] = startPoint;

        Points[1] = new Vector3(
            startPoint.x + 4,
            startPoint.y + (endPoint.y - startPoint.y) * (-2f / 5f)
        );

        // Điểm thứ hai cách startPosition 4.5/5 quãng đường và độ cao 4/5 của endPosition
        Points[2] = new Vector3(
            startPoint.x + 2f,
            startPoint.y + (endPoint.y - startPoint.y) * 4f / 5f
        );

        // Điểm cuối
        Points[3] = endPoint;
    }

    public void SetPosForDailyRewardBooster(Vector3 startPoint, Vector3 endPoint)
    {
        // Điểm đầu
        Points[0] = startPoint;

        Points[1] = new Vector3(
              startPoint.x + -4,
              startPoint.y + (endPoint.y - startPoint.y) * (-1f / 5f)
          );

        // Điểm thứ hai cách startPosition 4.5/5 quãng đường và độ cao 4/5 của endPosition
        Points[2] = new Vector3(
            startPoint.x - 1,
            endPoint.y + (startPoint.y - endPoint.y) * 1f / 5f
        );

        // Điểm cuối
        Points[3] = endPoint;
    }

    public void SetPosForGiveAwayItem(Vector3 startPoint, Vector3 endPoint)
    {
        // Điểm đầu
        Points[0] = startPoint;

        Points[1] = new Vector3(
              startPoint.x + -4,
              startPoint.y + (endPoint.y - startPoint.y) * (-1f / 5f)
          );

        // Điểm thứ hai cách startPosition 4.5/5 quãng đường và độ cao 4/5 của endPosition
        Points[2] = new Vector3(
            startPoint.x -1,
            endPoint.y + (startPoint.y - endPoint.y) * 1f / 5f
        );

        // Điểm cuối
        Points[3] = endPoint;
    }


    public Vector3 StartPosition
    {
        get { return Points[0]; }
    }

    public Vector3 EndPosition
    {
        get { return Points[3]; }
    }

    public Vector3 GetSegment(float time)
    {
        time = Mathf.Clamp01(time);
        float u = 1 - time;
        return (u * u * u * Points[0]) +
               (3 * u * u * time * Points[1]) +
               (3 * u * time * time * Points[2]) +
               (time * time * time * Points[3]);
    }
    public Vector3[] GetSegments(int Subdivisions)
    {
        Vector3[] segments = new Vector3[Subdivisions];

        float time;
        for (int i = 0; i < Subdivisions; i++)
        {
            time = (float)i / Subdivisions;
            segments[i] = GetSegment(time);
        }

        return segments;
    }
}
