﻿using makets.helper;
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
    /// Логика взаимодействия для NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        public NewUser()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string selectedGender = MaleOrFemale.SelectedItem.ToString().Split(":")[1];
            UserInfoSaver.SelectedGender = selectedGender;
            UserTags userTags = new UserTags();
            userTags.Show();
            this.Close();
        }
    }
}
