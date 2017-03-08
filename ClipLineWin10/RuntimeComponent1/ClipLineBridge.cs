using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace ClipLineBridge
{
    [AllowForWeb]
    public sealed class ClipLineBridge
    {
        public async void pickFile()
        {
            //CameraCaptureUI dialog = new CameraCaptureUI();
            //Size aspectRatio = new Size(16, 9);
            //dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;
            //StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Video);
            FileOpenPicker pick = new FileOpenPicker();
            pick.FileTypeFilter.Add(".mp4");
            StorageFile file = await pick.PickSingleFileAsync();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file2 = await file.CopyAsync(folder);

            await Task.Run( () =>
            {
                File.Copy(file.Path, "C:\\Program Files (x86)\\Apache Software Foundation\\Tomcat 9.0\\webapps\\ROOT\\ClipLine\\a.mp4");
            });
        }
    }
}
