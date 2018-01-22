using System;
using Foundation;
using UIKit;

namespace BackgroundTransferServiceDemo
{
    internal class UrlSessionDelegate : NSUrlSessionDownloadDelegate
    {
        private readonly DemoViewController _controller;

        public UrlSessionDelegate(DemoViewController demoViewController)
        {
            _controller = demoViewController;
        }

        public override void DidWriteData(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long bytesWritten,
            long totalBytesWritten, long totalBytesExpectedToWrite)
        {
            Console.WriteLine("Set Progress");
            if (Equals(downloadTask, _controller.downloadTask))
            {
                float progress = totalBytesWritten / (float)totalBytesExpectedToWrite;
                Console.WriteLine($"DownloadTask: {downloadTask}  progress: {progress}");
                InvokeOnMainThread(() =>
                {
                    _controller.ProgressView.Progress = progress;
                });
            }
        }

        public override void DidFinishDownloading(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
        {
            Console.WriteLine("Finished");
            Console.WriteLine("File downloaded in : {0}", location);
            NSFileManager fileManager = NSFileManager.DefaultManager;

            var URLs = fileManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
            if (URLs.Length < 1) { throw new Exception("Cannot access document directory"); }

            NSUrl documentDirectory = URLs[0];
            //var originURL = downloadTask.OriginalRequest.Url;
            NSUrl destinationUrl = documentDirectory.Append("image1.png", false);

            if (fileManager.FileExists(destinationUrl.Path))
            {
                var sucessRemove = fileManager.Remove(destinationUrl, out var removeExistError);
                if (!sucessRemove)
                {
                    Console.WriteLine("Error during remove destination existing file: {0}", removeExistError.LocalizedDescription);
                    return;
                }
            }


            var successCopy = fileManager.Copy(location, destinationUrl, out var copyError);

            if (successCopy)
            {
                UIImage image = UIImage.FromFile(destinationUrl.Path);

                InvokeOnMainThread(() =>
                {
                    _controller.ImageView.Image = image;
                    _controller.ImageView.Hidden = false;
                    _controller.ProgressView.Hidden = true;
                });
            }
            else
            {
                Console.WriteLine("Error during the copy: {0}", copyError.LocalizedDescription);
            }
        }

        public override void DidCompleteWithError(NSUrlSession session, NSUrlSessionTask task, NSError error)
        {
            Console.WriteLine("DidComplete");
            if (error == null)
            {
                Console.WriteLine("Task: {0} completed successfully", task);
            }
            else
            {
                Console.WriteLine("Task: {0} completed with error: {1}", task, error.LocalizedDescription);
            }

            float progress = task.BytesReceived / (float)task.BytesExpectedToReceive;

            InvokeOnMainThread(() => { _controller.ProgressView.Progress = progress; });
            _controller.downloadTask = null;
        }

        public override void DidResume(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long resumeFileOffset,
            long expectedTotalBytes)
        {
            Console.WriteLine("DidResume");
        }

        public override void DidFinishEventsForBackgroundSession(NSUrlSession session)
        {
            Console.WriteLine("DidFinishEventsForBackgroundSession");

            AppDelegate appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            var handler = appDelegate?.BackgroundSessionCompleteHandler;
            if (handler != null)
            {
                appDelegate.BackgroundSessionCompleteHandler = null;
                handler.Invoke();
            }

            Console.WriteLine("All tasks are finished");
        }
    }
}