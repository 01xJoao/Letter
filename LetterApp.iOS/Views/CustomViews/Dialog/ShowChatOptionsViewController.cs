using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class ShowChatOptionsViewController : UIViewController
    {
        private string _name;
        private string _photo;
        private bool _muted;
        private string[] _resources;
        private Action<Tuple<ChatOptions, bool>> _buttonAction;

        public ShowChatOptionsViewController(string name, string photo, bool muted, string[] resources, Action<Tuple<ChatOptions, bool>> buttonAction) 
            : base("ShowChatOptionsViewController", null)
        {
            _name = name;
            _photo = string.Copy(photo);
            _muted = muted;
            _resources = resources;
            _buttonAction = buttonAction;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.Alpha = 0.3f;
            this.View.BackgroundColor = Colors.Black30;
            _backgroundView.Layer.CornerRadius = 2f;
            _buttonView.Layer.CornerRadius = 2f;
            _backgroundImage.Layer.CornerRadius = 2f;
            CustomUIExtensions.ViewShadow(_backgroundView);

            _buttonView.BackgroundColor = Colors.MainBlue;
            UIButtonExtensions.SetupButtonAppearance(_button, Colors.White, 17f, _resources[_resources.Length-1]);

            _nameLabel.AttributedText = new NSAttributedString(_name, font: UIFont.BoldSystemFontOfSize(16), foregroundColor: Colors.White, shadow: CustomUIExtensions.TextShadow());

            if (!string.IsNullOrEmpty(_photo))
            {
                ImageService.Instance.LoadStream((token) =>
                {
                    return ImageHelper.GetStreamFromImageByte(token, _photo);
                }).ErrorPlaceholder("letter_round_big", ImageSource.CompiledResource).Transform(new BlurredTransformation(25f)).Into(_backgroundImage);

                ImageService.Instance.LoadStream((token) =>
                {
                    return ImageHelper.GetStreamFromImageByte(token, _photo);
                }).ErrorPlaceholder("letter_round_big", ImageSource.CompiledResource).Transform(new RoundedTransformation(30)).Into(_profileImage);

            }
            else
            {
                _backgroundImage.BackgroundColor = Colors.MainBlue;
                _profileImage.Image = UIImage.FromBundle("letter_curved");
                CustomUIExtensions.CornerView(_profileImage, 3);
            }

            _backgroundImage.ContentMode = UIViewContentMode.ScaleToFill;

            _button.TouchUpInside -= OnCloseButton_TouchUpInside;
            _button.TouchUpInside += OnCloseButton_TouchUpInside;

            SetupTableView();
        }

        private void SetupTableView()
        {
            var source = new ChatOptionsSource(_tableView, _muted, _resources);

            _tableView.Source = source;
            _tableView.ReloadData();
            _tableView.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, _tableView.Frame.Size.Width, 1));

            source.OptionMuteEvent -= OnSource_MuteEvent; 
            source.OptionMuteEvent += OnSource_MuteEvent;

            source.OptionSelectedEvent -= OnSource_OptionsEvent;
            source.OptionSelectedEvent += OnSource_OptionsEvent;
        }

        private void OnSource_MuteEvent(object sender, bool mute)
        {
            _muted = mute;
        }

        private void OnSource_OptionsEvent(object sender, ChatOptions option)
        {
            OptionSelected(option);
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            OptionSelected();
        }

        private void OptionSelected(ChatOptions option = ChatOptions.MuteChat)
        {
            _buttonAction?.Invoke(new Tuple<ChatOptions, bool>(option, _muted));
            Dismiss();
        }

        public void Show()
        {
            this.View.Frame = UIApplication.SharedApplication.KeyWindow.Bounds;
            UIApplication.SharedApplication.KeyWindow.AddSubview(this.View);
            UIView.Animate(0.3, () => View.Alpha = 1);
        }

        public void Dismiss()
        {
            UIView.AnimateNotify(0.3, () => View.Alpha = 0, (finished) => CleanFromMemory());
        }

        private void CleanFromMemory()
        {
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

