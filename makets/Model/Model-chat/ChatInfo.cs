using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace makets.Model.Model_chat
{
    public class ChatInfo
    {
        public int ChatId { get; set; }
        public OtherUser OtherUser { get; set; }
        public string LastMessage { get; set; }
        public BitmapImage ProfileImage => ConvertToBitmapImage(OtherUser.ProfilePhoto);

        private BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null) return null;
            using (var stream = new System.IO.MemoryStream(imageData))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }
    }
}
