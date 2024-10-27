using makets.helper.EmailSender;
using makets.helper.EmailSender.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace makets.pages
{
    /// <summary>
    /// Логика взаимодействия для PasswordReset.xaml
    /// </summary>
    public partial class PasswordReset : Window
    {

        private int sendedCode = 0;

        public PasswordReset()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //создание нового пароля, предлагаю сделать просто через 
            //if (код совпал){ прям в этом же окне сделаю просто замену верстки, а точнее просто надписей}


        }
        private void SendEmailBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailBox.Text))
            {
                MessageBox.Show("Поле Email не может быть пустым."); return;
            }


            int code = new Random().Next(1000, 10000);
            this.sendedCode = code;

            var isSended = MailSender
                .CreateSender()
                .UseSmtp(SMTPEnum.MailRuDefault)
                .WithDefaultPort()
                .SetRouting(new SMTPRouting("сюда запихните ваш майл", EmailBox.Text))
                .WithAuthorization(new SmtpCredentials("сюда ваш логин", "сюда ваш пароль"))
                .UseContent(new SMTPContent("Запрос на сброс пароля.", $"Ваш код подтверждения: {code}"))
                .SendMail();
            
            if (string.IsNullOrEmpty(ClaimCodeBox.Text) || ClaimCodeBox.Text != sendedCode.ToString())
            {
                MessageBox.Show("Неверно введеный код.");
                return;
            }

        }
    }
}
