using UnityEngine;

public class NotificationControl : MonoBehaviour
{
    [SerializeField]
    private string[] txtDes;
    [SerializeField]
    private string[] txtTitle;
    public MyNotification myNotification;

    public void PusNoti(string title, string t, System.DateTime d, string id)
    {
        //IGameNotification notification = manager.CreateNotification();
        //notification.Title = Application.productName;
        //notification.Body = t;
        //notification.DeliveryTime = d;

        //AndroidNotificationCenter.SendNotification(new AndroidNotification()
        //{
        //    Title = Application.productName,
        //    Text = t,
        //    FireTime =d,
        //    IntentData = id,
        //    SmallIcon = smallIcon,
        //    LargeIcon = largeIcon
        //}, channelId);
        myNotification.SendPush(title, t, d, id);

        //  manager.ScheduleNotification(notification);
    }
    public void Start()
    {
        myNotification.CreateChannel();
        myNotification.CancelAll();
        Invoke("PushNotification", 2);
        DontDestroyOnLoad(gameObject);
    }

    public void PushNotification()
    {
        System.DateTime d = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 8, 0, 0);

        if (System.DateTime.Now.Hour < 9)
        {
            int a101 = Random.Range(0, txtDes.Length);
            PusNoti(txtTitle[a101], txtDes[a101], d.AddDays(0.167f), "d0");
            int a102 = Random.Range(0, txtDes.Length);
            PusNoti(txtTitle[a102], txtDes[a102], d.AddDays(0.5f), "d0-1");
        }
        else
        {
            if (System.DateTime.Now.Hour < 15)
            {
                int a103 = Random.Range(0, txtDes.Length);
                PusNoti(txtTitle[a103], txtDes[a103], d.AddDays(0.5f), "d0");
            }
        }

        int a = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a], txtDes[a], d.AddDays(1), "d1");
        int a1 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a1], txtDes[a1], d.AddDays(1.167f), "d1-1");
        int a2 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a2], txtDes[a2], d.AddDays(1.5f), "d1-2");



        int a3 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a3], txtDes[a3], d.AddDays(2), "d2");
        int a4 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a4], txtDes[a4], d.AddDays(3), "d3");
        int a5 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a5], txtDes[a5], d.AddDays(5), "d5");
        int a6 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a6], txtDes[a6], d.AddDays(7), "d7");
        int a7 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a7], txtDes[a7], d.AddDays(7.167), "d7-1");
        int a8 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a8], txtDes[a8], d.AddDays(7.5), "d7-2");


        int a9 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a9], txtDes[a9], d.AddDays(14), "d14");
        int a10 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a10], txtDes[a10], d.AddDays(14.167), "d14-1");
        int a11 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a11], txtDes[a11], d.AddDays(14.5), "d14-2");

        int a12 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a12], txtDes[a12], d.AddDays(21), "d21");
        int a13 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a13], txtDes[a13], d.AddDays(21.167), "d21-1");
        int a14 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a14], txtDes[a14], d.AddDays(21.5), "d21-2");

        int a15 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a15], txtDes[a15], d.AddDays(30), "d30");
        int a16 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a16], txtDes[a16], d.AddDays(30.167), "d30-1");
        int a17 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a17], txtDes[a17], d.AddDays(30.5), "d30-2");

        int a18 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a18], txtDes[a18], d.AddDays(40), "d40");
        int a19 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a19], txtDes[a19], d.AddDays(40.167), "d40-1");
        int a20 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a20], txtDes[a20], d.AddDays(40.5), "d40-2");

        int a21 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a21], txtDes[a21], d.AddDays(50), "d50");
        int a22 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a22], txtDes[a22], d.AddDays(50.167), "d50-1");
        int a23 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a23], txtDes[a23], d.AddDays(50.5), "d50-2");

        int a24 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a24], txtDes[a24], d.AddDays(60), "d60");
        int a25 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a25], txtDes[a25], d.AddDays(60.167), "d60-1");
        int a26 = Random.Range(0, txtDes.Length);
        PusNoti(txtTitle[a26], txtDes[a26], d.AddDays(60.5), "d60-2");
    }
}
