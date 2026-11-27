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
                var user = db.Polzovatel.FirstOrDefault(
                    dannie => dannie.Login == LoginBox.Text && dannie.Parol == ParolBox.Text);

                    if (user != null)
                    {
                    new Katalog(user).Show();
                    Close();
                    } else { MessageBox.Show("Логин или пароль неверные");}
            }
            catch(Exception)
            {
                MessageBox.Show("Логин или пароль неверные");
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
