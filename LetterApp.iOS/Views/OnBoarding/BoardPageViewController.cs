using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.OnBoarding
{
    public partial class BoardPageViewController : XBoardPageViewController
    {
        private int _index;
        private string _title;
        private string _description;
        private string _image;

        public BoardPageViewController(int index, string title, string description, string image) : base(index, "BoardPageViewController", null)
        {
            _index = index;
            _title = title;
            _description = description;
            _image = image;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _imageView.Image = UIImage.FromBundle(_image);
            _titleLabel.Text = _title;
            _descriptionLabel.Text = _description;
            _backgroundView.BackgroundColor = Colors.MainBlue;
        }
    }
}

