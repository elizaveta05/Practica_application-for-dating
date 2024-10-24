using System.Windows;

namespace makets.pages
{
    public partial class UserProfile : Window
    {
        public UserProfile(int userid)
        {
            InitializeComponent();
        }

        private void ChatsTextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Открытие следующего окна
            //var chatsWindow = new ChatsWindow(); 
            //chatsWindow.Show();
        }
    }
}
