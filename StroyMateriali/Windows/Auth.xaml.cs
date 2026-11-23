using StroyMateriali.Models;
using System;
using System.Linq;
using System.Windows;

namespace StroyMateriali.Windows
{
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }
        private readonly StroyMaterialiEntities db = new StroyMaterialiEntities();

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isError = false;
                if (string.IsNullOrWhiteSpace(LoginBox.Text))
                    isError = true;
                if (string.IsNullOrWhiteSpace(ParolBox.Text))
                    isError = true;
                if (isError == true)
                    MessageBox.Show("Логин или пароль неверные");
                var user = db.Polzovatel.FirstOrDefault(
                    dannie => dannie.Login == LoginBox.Text && dannie.Parol == ParolBox.Text);
                if (user == null)
                    MessageBox.Show("Логин или пароль неверные");
                else
                {
                    var okno = new StroyMateriali.Windows.Katalog(user.Ima);
                    okno.Show();
                    Close();
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Ошибка авторизации");
            }
        }

        private void AuthGhost_Click(object sender, RoutedEventArgs e)
        {
            var okno = new StroyMateriali.Windows.Katalog(null);
            okno.Show();
            Close();
        }
    }
}
