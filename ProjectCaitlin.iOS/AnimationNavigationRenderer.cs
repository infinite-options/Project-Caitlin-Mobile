﻿using System;
using ProjectCaitlin.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(AnimationNavigationRenderer))]
namespace ProjectCaitlin.iOS
{
    public class AnimationNavigationRenderer : Xamarin.Forms.Platform.iOS.NavigationRenderer
    {
        private AnimationNavigationControllerDelegate _delegate;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _delegate = new AnimationNavigationControllerDelegate();
            Delegate = _delegate;
        }
    }
}