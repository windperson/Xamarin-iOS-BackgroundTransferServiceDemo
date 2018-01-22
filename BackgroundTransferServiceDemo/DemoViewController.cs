
using System;
using System.Diagnostics;
using System.Drawing;

using Foundation;
using UIKit;

namespace BackgroundTransferServiceDemo
{
    public partial class DemoViewController : UIViewController
    {

        const string Identifier = "bts_demo";
        const string DownloadUrlString = "https://upload.wikimedia.org/wikipedia/commons/9/97/The_Earth_seen_from_Apollo_17.jpg";


        public NSUrlSession session;
        public NSUrlSessionDownloadTask downloadTask;

        public DemoViewController(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (session == null)
            {
                session = InitBackgroundSession();
            }

            progressView.Progress = 0;
            imageView.Hidden = false;
            progressView.Hidden = true;

            startButton.Clicked += Start;

            // Force the app to crash
            crashButton.Clicked += delegate
            {
                string s = null;
                s.ToString();
            };
        }

      

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion

        private NSUrlSession InitBackgroundSession()
        {
            Console.WriteLine("InitBackgroundSession");

            using (var configuration = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration(Identifier))
            {
                return NSUrlSession.FromConfiguration(configuration, (INSUrlSessionDelegate)new UrlSessionDelegate(this), new NSOperationQueue());
            }
        }

        private void Start(object sender, EventArgs e)
        {
            if (downloadTask != null)
            {
                return;
            }

            using (var request = NSUrlRequest.FromUrl(NSUrl.FromString(DownloadUrlString)))
            {
                downloadTask = session.CreateDownloadTask(request);
                downloadTask.Resume();
            }

            imageView.Hidden = true;
            progressView.Hidden = false;
        }

        public UIProgressView ProgressView => progressView;

        public UIImageView ImageView => imageView;

    }
}