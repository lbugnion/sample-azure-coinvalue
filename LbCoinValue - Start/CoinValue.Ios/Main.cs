using CoinClient.ViewModel;
using UIKit;

namespace CoinValue.Ios
{
    public class Application
    {
        private static ViewModelLocator _locator;

        public static ViewModelLocator Locator => _locator ?? (_locator = new ViewModelLocator());

        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");

            //// This should come before MobileCenter.Start() is called
            //Push.PushNotificationReceived += (sender, e) =>
            //{
            //    // Instantiate the builder and set notification elements:
            //    var builder = new Notification.Builder(this)
            //        .SetContentTitle(e.Title)
            //        .SetContentText(e.Message)
            //        .SetSmallIcon(Resource.Drawable.notification_icon);

            //    // Build the notification:
            //    Notification notification = builder.Build();

            //    // Get the notification manager:
            //    NotificationManager notificationManager
            //        = GetSystemService(NotificationService) as NotificationManager;

            //    // Publish the notification:
            //    const int notificationId = 0;
            //    notificationManager.Notify(notificationId, notification);
            //};

            //MobileCenter.Start("585a1865-5171-45b5-9a5e-40923798232d", typeof(Push));
        }
    }
}