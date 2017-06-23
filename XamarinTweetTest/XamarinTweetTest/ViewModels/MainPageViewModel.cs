using Prism.Commands;
using Prism.Mvvm;
using XamarinTweet.Models;

namespace XamarinTweetTest.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        #region コマンド
        public DelegateCommand SendHelloTweetCommand { get; }
        public DelegateCommand SendTweetCommand { get; }
        public DelegateCommand DeleteTestTweetCommand { get; }
        public DelegateCommand RetweetCommand { get; }
        public DelegateCommand UnRetweetCommand { get; }
        public DelegateCommand FavoriteCommand { get; }
        public DelegateCommand UnFavoriteCommand { get; }
        public DelegateCommand FollowOXamarinCommand { get; }
        public DelegateCommand UnFollowOXamarinCommand { get; }

        #endregion

        #region プロパティ

        public string TweetContents { get; set; }

        #endregion
        #region  コンストラクタ

        public MainPageViewModel()
        {
            var tweet = new Tweet();

            SendTweetCommand = new DelegateCommand(() => tweet.TestTweet(TweetContents));
            SendHelloTweetCommand = new DelegateCommand(() => tweet.SayHelloTwitter());
            RetweetCommand = new DelegateCommand(() => tweet.Retweet(839485449638903808));
            UnRetweetCommand = new DelegateCommand(() => tweet.UnRetweet(839485449638903808));
            FavoriteCommand = new DelegateCommand(() => tweet.Favorite(839485449638903808));
            UnFavoriteCommand = new DelegateCommand(() => tweet.UnFavorite(839485449638903808));
        }

        #endregion
    }
}
