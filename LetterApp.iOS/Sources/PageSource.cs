﻿using System;
using System.Collections.Generic;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class PageSource : UIPageViewControllerDataSource
    {
        private readonly List<XBoardPageViewController> _viewControllersInPage;

        public PageSource(List<XBoardPageViewController> viewControllersInPage)
        {
            _viewControllersInPage = viewControllersInPage;
        }

        public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var currentViewController = referenceViewController as XBoardPageViewController;
            var index = currentViewController.Index + 1;

            if (index == _viewControllersInPage.Count)
                return null;

            return _viewControllersInPage[index];
        }

        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var currentViewController = referenceViewController as XBoardPageViewController;
            var index = currentViewController.Index;

            if (index == 0)
                return null;

            return _viewControllersInPage[index - 1];
        }
    }
}
