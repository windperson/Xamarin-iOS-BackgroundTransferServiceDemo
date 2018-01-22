// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace BackgroundTransferServiceDemo
{
    [Register ("DemoViewController")]
    partial class DemoViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem crashButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView progressView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem startButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (crashButton != null) {
                crashButton.Dispose ();
                crashButton = null;
            }

            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }

            if (progressView != null) {
                progressView.Dispose ();
                progressView = null;
            }

            if (startButton != null) {
                startButton.Dispose ();
                startButton = null;
            }
        }
    }
}