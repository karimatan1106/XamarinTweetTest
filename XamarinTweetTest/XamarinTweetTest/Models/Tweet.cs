using CoreTweet;
using CoreTweet.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace XamarinTweet.Models
{
    public class Tweet
    {
        #region 認証パラメータ
        private const string Consumer_Key = "Your ConsumerKey";
        private const string Consumer_Secret = "Your ConsumerSecuret";
        private const string Access_Token = "Your AccessToken";
        private const string Access_Token_Secret = "Your AccessTokenSecret";
        #endregion

        #region フィールド
        private readonly Tokens _tokens;
        private static readonly HttpClient _httpClient = new HttpClient();
        #endregion

        #region コンストラクタ
        public Tweet()
        {
            // API にアクセスするためのトークン群
            _tokens = Tokens.Create(
                Consumer_Key,
                Consumer_Secret,
                Access_Token,
                Access_Token_Secret
            );
        }
        #endregion

        #region メソッド

        /// <summary>
        /// 「Hello Twitter」と呟くだけ
        /// </summary>
        public async void SayHelloTwitter()
        {
            await _tokens.Statuses.UpdateAsync(
                status => "Hello Twitter"
            );
        }

        /// <summary>
        /// 指定した文字列をツイートする
        /// </summary>
        /// <param name="tweetText"></param>
        public async void TestTweet(string tweetText)
        {
            //呟く文字が0文字であれば処理を終了
            if (string.IsNullOrEmpty(tweetText)) return;

            //ツイートの取得
            var tweet = await _tokens.Statuses.ShowAsync(id => 839485449638903808);

            //呟いているユーザの画像をStream型で取得
            var response = await _httpClient.GetAsync(
                new Uri(tweet.User.ProfileImageUrl),
                HttpCompletionOption.ResponseContentRead);
            var stream = await response.Content.ReadAsStreamAsync();

            //ユーザの画像付きでツイート
            await _tokens.Statuses.UpdateWithMediaAsync(
                status => tweetText,
                media => stream);
        }

        /// <summary>
        /// 指定したTweetIDのツイートを削除
        /// </summary>
        /// <param name="tweetID"></param>
        public async void DeleteTweet(long tweetID)
        {
            await _tokens.Statuses.DestroyAsync(id => tweetID);
        }

        /// <summary>
        /// 指定したScreenName(TwitterのID名)のツイートを取得
        /// </summary>
        /// <param name="screenName">TwitterのID名</param>
        /// <returns></returns>
        public async Task<ListedResponse<Status>> GetUserTimeline(string screenName)
        {
            //ラムダ式の代わりにDictionaryでも渡せます
            var param = new Dictionary<string, object>
            {
                ["count"] = 100,
                ["screen_name"] = screenName,
            };
            return await _tokens.Statuses.UserTimelineAsync(param);
        }

        /// <summary>
        /// 自分のタイムラインを取得
        /// </summary>
        /// <returns></returns>
        public async Task<ListedResponse<Status>> GetHomeTimeline()
        {
            return await _tokens.Statuses.HomeTimelineAsync();
        }

        /// <summary>
        /// 指定したTweetIDのツイートをリツイートする
        /// </summary>
        public async void Retweet(long tweetID)
        {
            //リツイートしたいツイートを取得
            var tweet = await _tokens
                .Statuses
                .ShowAsync(id => tweetID);

            //ツイートが既にリツイートされていなければリツイートする
            if (tweet?.IsRetweeted != null
                && (bool)!tweet.IsRetweeted)
            {
                await _tokens.Statuses.RetweetAsync(id => tweet.Id);
            }
        }

        /// <summary>
        /// 指定したTweetIDのリツイートを取り消す
        /// </summary>
        /// <param name="tweetID"></param>
        public async void UnRetweet(long tweetID)
        {
            //リツイートを取り消したいツイートを取得する
            var tweet = await _tokens
                .Statuses
                .ShowAsync(
                    id => tweetID,
                    include_my_retweet => true);

            //取得したツイートをリツイートしていればリツイートを取り消す
            if (tweet?.CurrentUserRetweet != null)
            {
                await _tokens.Statuses.DestroyAsync(id => tweet.CurrentUserRetweet);
            }
        }

        /// <summary>
        /// 指定したTweetIDのツイートをファボ(いいね)する
        /// </summary>
        public async void Favorite(long tweetID)
        {
            //ファボ(いいね)したいツイートを取得
            var tweet = await _tokens
                .Statuses
                .ShowAsync(id => tweetID);

            //ツイートが既にファボ(いいね)されていなければファボ(いいね)する
            if (tweet?.IsFavorited != null
                && (bool)!tweet.IsFavorited)
            {
                await _tokens.Favorites.CreateAsync(id => tweetID);
            }
        }

        /// <summary>
        /// 指定したTweetIDのファボ(いいね)を取り消す
        /// </summary>
        /// <param name="tweetID"></param>
        public async void UnFavorite(long tweetID)
        {
            //ファボ(いいね)したいツイートを取得
            var tweet = await _tokens
                .Statuses
                .ShowAsync(id => tweetID);

            //取得したツイートをファボ(いいね)していればファボ(いいね)を取り消す
            if (tweet?.IsFavorited != null
                && (bool)tweet.IsFavorited)
            {
                await _tokens.Favorites.DestroyAsync(id => tweetID);
            }
        }
        #endregion
    }
}