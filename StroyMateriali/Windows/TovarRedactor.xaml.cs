using StroyMateriali.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace StroyMateriali.Windows
{
    public partial class TovarRedactor : Window
    {
        public TovarRedactor(Tovar tovar)
        {
            InitializeComponent();
                try
                {
                using (var db = new StroyMaterialiEntities())
                {
                    ProizvoditelBox.ItemsSource = db.Proizvoditel.Select(svyaz => svyaz.Ima).ToList();
                    KategoriaBox.ItemsSource = db.Kategoria.Select(svyaz => svyaz.Ima).ToList();
                    PostavshikBox.ItemsSource = db.Postavshik.Select(svyaz => svyaz.Ima).ToList();
                }
                if (tovar == null)
                        Title = "Добавление товара";
                    else
                    {
                        Title = "Редактирование товара";
                        ImaBox.Text = tovar.Ima;
                        OpisanieBox.Text = tovar.Opisanie;
                        ProizvoditelBox.SelectedItem = tovar.ImaProizvoditel;
                        PostavshikBox.SelectedItem = tovar.ImaPostavshika;
                        KategoriaBox.SelectedItem = tovar.ImaKategoria;
                        KolichestvoBox.Text = tovar.Kolichestvo.ToString();
                        CenaBox.Text = tovar.Cena.ToString();
                        EdinicaBox.Text = tovar.Edinica.ToString();
                        SkidkaBox.Text = tovar.Skidka.ToString();
                        try
                        {
                            Photo.Source = new BitmapImage(new Uri("/Resources/Image/" + tovar.Photo, UriKind.Relative));

                            if (string.IsNullOrEmpty(tovar.Photo))
                                Photo.Source = new BitmapImage(new Uri("/Resources/Image/picture.png", UriKind.Relative));
                        }
                        catch
                        {
                            Photo.Source = new BitmapImage(new Uri("/Resources/Image/picture.png", UriKind.Relative));
                        }
                    }
                } catch { MessageBox.Show("Ошибка загрузки"); }
        }
    }
}
