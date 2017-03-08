using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace App1
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class WebviewPage : Page
    {
        ClipLineBridge.ClipLineBridge cb = new ClipLineBridge.ClipLineBridge();
        public WebviewPage()
        {
            this.InitializeComponent();
            this.mainWebview.NavigationStarting += mainWebview_NavigationStarting;
            this.Loaded += WebviewPage_Loaded;

            //List<Uri> allowedUris = new List<Uri>();
            //allowedUris.Add(new Uri("http://localhost:39629/"));
            //mainWebview.AllowedScriptNotifyUris = allowedUris;

            //mainWebview.ScriptNotify += mainWebview_ScriptNotify;

        }

        private void WebviewPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.mainWebview.Navigate(new Uri("ms-appx-web:///html/todo.html", UriKind.RelativeOrAbsolute));
        }

        private void mainWebview_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            this.mainWebview.AddWebAllowedObject("OURBRIDGEOBJ", cb);
        }

        private void mainWebview_ScriptNotify(object sender, NotifyEventArgs e)
        {
            /// カメラを起動して、動画を撮る
            if ("captureVideo".Equals(e.Value))
            {
                captureVideo();
            }
            /// ローカルファイルから動画を選択する
            else if ("pickVideo".Equals(e.Value))
            {
                pickVideo();
            }
            /// HardwareIdを取得する
            else if ("getHardwareId".Equals(e.Value))
            {
                getHardwareId();
            }
        }

        /// <summary>
        /// HardwareIdを取得する
        /// </summary>
        private async void getHardwareId()
        {
            string hardwareId = null;

            /// インスタンス取得
            HardwareToken token = HardwareIdentification.GetPackageSpecificToken(null);
            if (token == null)
            {
                //エラー処理
                return ;
            }

            /// インスタンスからID取得
            var stream = token.Id.AsStream();
            using (var reader = new BinaryReader(stream))
            {
                var bytes = reader.ReadBytes((int)stream.Length);
                hardwareId = BitConverter.ToString(bytes);
            }

            /// Webviewで表示している画面に戻り値を渡す
            var res = await mainWebview.InvokeScriptAsync("setHardwareId", new String[] { hardwareId });

            /// 渡す処理に失敗した場合
            //TODO: 判定条件正しく直す
            if ("".Equals(res))
            {
                //エラー処理
            }
        }

        /// <summary>
        /// カメラを起動して、動画を撮る
        /// </summary>
        private async void captureVideo()
        {
            /// カメラダイアログを作成する
            CameraCaptureUI dialog = new CameraCaptureUI();
            Size aspectRatio = new Size(16, 9);
            dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

            /// 撮った動画を取得する
            StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Video);

            /// カメラダイアログでキャンセルをした場合
            if (file == null)
            {
                //エラー処理
                return;
            }

            /// ファイルをサーバにアップロードする
            string videoURL = "";
            videoURL = uploadVideo(file);
            if ("".Equals(videoURL))
            {
                //エラー処理
                //return;
            }

            /// Webviewで表示している画面に戻り値を渡す
            var res = mainWebview.InvokeScriptAsync("setVideo", new String[] { file.Path });
        }

        /// <summary>
        /// ローカルファイルから動画を選択する
        /// </summary>
        private async void pickVideo()
        {
            /// ファイル選択ダイアログを作成する
            FileOpenPicker pick = new FileOpenPicker();
            pick.FileTypeFilter.Add(".mp4");

            /// 選択したファイルを取得する
            StorageFile file = await pick.PickSingleFileAsync();

            /// ファイル選択ダイアログでキャンセルをした場合
            if (file == null)
            {
                //エラー処理
                return;
            }
            File.Copy(file.Path, "C:\\a.mp4");
            /// ファイルをサーバにアップロードする
            string videoURL = "";
            videoURL = uploadVideo(file);
            if ("".Equals(videoURL))
            {
                //エラー処理
                //return;
            }

            /// Webviewで表示している画面に戻り値を渡す
            var res = mainWebview.InvokeScriptAsync("setVideo", new String[] { file.Path });
        }

        /// <summary>
        /// 動画をサーバにアップロードする
        /// </summary>
        private  string uploadVideo(StorageFile file)
        {
            string videoURL = "";

            FolderPicker pick = new FolderPicker();
            pick.FileTypeFilter.Add(".mp4");

            /// 選択したファイルを取得する
            //StorageFolder folder = await pick.

            //try
            //{
            //    //获取上传的文件数据
            //    HttpPostedFile file = context.Request.Files["Filedata"];
            //    string fileName = file.FileName;
            //    string fileType = Path.GetExtension(fileName).ToLower();
            //    //由于不同浏览器取出的FileName不同（有的是文件绝对路径，有的是只有文件名），故要进行处理
            //    if (fileName.IndexOf(' ') > -1)
            //    {
            //        fileName = fileName.Substring(fileName.LastIndexOf(' ') + 1);
            //    }
            //    else if (fileName.IndexOf('/') > -1)
            //    {
            //        fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
            //    }
            //    //上传的目录
            //    string uploadDir = "~/Content/uploadfile/TMP/" + System.DateTime.Now.ToString("yyyyMM") + "/";
            //    //上传的路径
            //    //生成年月文件夹及日文件夹
            //    if (Directory.Exists(context.Server.MapPath(uploadDir)) == false)
            //    {
            //        Directory.CreateDirectory(context.Server.MapPath(uploadDir));
            //    }
            //    if (Directory.Exists(context.Server.MapPath(uploadDir + System.DateTime.Now.ToString("dd") + "/")) == false)
            //    {
            //        Directory.CreateDirectory(context.Server.MapPath(uploadDir + System.DateTime.Now.ToString("dd") + "/"));
            //    }

            //    uploadDir = uploadDir + System.DateTime.Now.ToString("dd") + "/";

            //    string uploadPath = uploadDir + FormsAuthentication.HashPasswordForStoringInConfigFile(fileName, "MD5").Substring(0, 8) + fileType;
            //    //保存文件
            //    file.SaveAs(context.Server.MapPath(uploadPath));
            //    //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
            //    //DbHelperOleDb.ExecuteSql("insert into [temp](temp_sn,temp_Content) values('" + sn + "','" + uploadPath + "')");

            //    //Response.Write("1");
            //    //context.Response.Write("{'IsError':false, 'Data':'" + uploadPath + "'}");
            //    r = "{'IsError':false, 'Data':'" + uploadPath + "'}";
            //}
            //catch (Exception ex)
            //{
            //    //Response.Write("0");
            //    //throw ex;
            //    //context.Response.Write("{IsError: true, data:'" + ex.Message + "'}");
            //    r = "{'IsError':true, 'Data':'" + ex.Message + "'}";
            //}
            //finally
            //{
            //    r = r.Replace("'", "\"");
            //    context.Response.Write(r);
            //    context.Response.End();
            //}

            //テストに使うWebAPI http://www.ekidata.jp/api/api_pref.php
            //HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible;MSIE 10.0;Windows 10 ;WOW64;Trident/6.0)");
            //if (String.IsNullOrEmpty(tbUrl.Text))
            //{
            //    tbstatus.Text = "Please enter a valid url address pointing to the web API.";
            //}
            //else
            //{
            //    try
            //    {
            //        tbstatus.Text = " Waiting for response......";
            //        HttpResponseMessage response = await httpClient.GetAsync(new Uri(tbUrl.Text));

            //        response.EnsureSuccessStatusCode();
            //        tbstatus.Text = response.StatusCode + " " + response.ReasonPhrase;

            //        string result = await response.Content.ReadAsStringAsync();
            //        webView1.NavigateToString(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        tbstatus.Text = ex.ToString();
            //    }
            //}

            return videoURL;
        }
    }
}
