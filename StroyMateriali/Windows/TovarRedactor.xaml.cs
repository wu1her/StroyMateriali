using Microsoft.Win32;
using StroyMateriali.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace StroyMateriali.Windows
{
    public partial class TovarRedactor : Window
    {
        private readonly StroyMaterialiEntities db = new StroyMaterialiEntities();
        private Tovar tovar;

        public TovarRedactor(Tovar peredanTovar)
        {
            InitializeComponent();
            Closed += (s, e) => db.Dispose();

            try
            {
                KategoriaBox.ItemsSource = db.Kategoria.ToList();
                PostavshikBox.ItemsSource = db.Postavshik.ToList();
                ProizvoditelBox.ItemsSource = db.Proizvoditel.ToList();

                if (peredanTovar == null)
                {
                    Title = "Добавление товара";
                    SkidkaBox.Text = "0";
                    return;
                }

                Title = "Редактирование товара";
                tovar = db.Tovar.Find(peredanTovar.Artikul);

                if (tovar == null)
                {
                    Save.IsEnabled = false;
                    MessageBox.Show("Товар уже удалён.");
                    return;
                }

                ImaBox.Text = tovar.Ima;
                OpisanieBox.Text = tovar.Opisanie;
                CenaBox.Text = tovar.Cena.ToString();
                EdinicaBox.Text = tovar.Edinica;
                KolichestvoBox.Text = tovar.Kolichestvo.ToString();
                SkidkaBox.Text = tovar.Skidka.ToString();

                KategoriaBox.SelectedItem = tovar.Kategoria.FirstOrDefault();
                PostavshikBox.SelectedItem = tovar.Postavshik.FirstOrDefault();
                ProizvoditelBox.SelectedItem = tovar.Proizvoditel.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(tovar.Photo))
                {
                    try
                    {
                        Photo.Source = new BitmapImage(
                            new Uri("/Resources/Image/" + tovar.Photo, UriKind.Relative));
                    }
                    catch (System.IO.IOException)
                    {
                    }
                }
            }
            catch (Exception oshibka)
            {
                Save.IsEnabled = false;
                MessageBox.Show(oshibka.GetBaseException().Message, "Ошибка загрузки");
            }
        }

        private void PhotoAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png";

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                var photo = new BitmapImage(new Uri(dialog.FileName));

                if (photo.PixelWidth > 300 || photo.PixelHeight > 200)
                {
                    MessageBox.Show("Фото должно быть не больше 300×200.");
                    return;
                }

                Photo.Source = photo;
            }
            catch
            {
                MessageBox.Show("Фото не загрузилось.");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var kategoriya = KategoriaBox.SelectedItem as Kategoria;
            var postavshik = PostavshikBox.SelectedItem as Postavshik;
            var proizvoditel = ProizvoditelBox.SelectedItem as Proizvoditel;

            if (string.IsNullOrWhiteSpace(ImaBox.Text) ||
                string.IsNullOrWhiteSpace(OpisanieBox.Text) ||
                string.IsNullOrWhiteSpace(EdinicaBox.Text) ||
                kategoriya == null || postavshik == null || proizvoditel == null)
            {
                MessageBox.Show("Заполни поля и выбери значения из списков.");
                return;
            }

            try
            {
                decimal cena = decimal.Parse(CenaBox.Text);
                int kolichestvo = int.Parse(KolichestvoBox.Text);
                int skidka = int.Parse(SkidkaBox.Text);

                if (tovar == null)
                {
                    tovar = new Tovar();
                    tovar.Artikul = Guid.NewGuid().ToString("N").Substring(0, 10);
                    tovar.Photo = "picture.png";
                    db.Tovar.Add(tovar);
                }

                tovar.Ima = ImaBox.Text.Trim();
                tovar.Opisanie = OpisanieBox.Text.Trim();
                tovar.Edinica = EdinicaBox.Text.Trim();
                tovar.Cena = cena;
                tovar.Kolichestvo = kolichestvo;
                tovar.Skidka = skidka;

                tovar.Kategoria.Clear();
                tovar.Kategoria.Add(kategoriya);

                tovar.Postavshik.Clear();
                tovar.Postavshik.Add(postavshik);

                tovar.Proizvoditel.Clear();
                tovar.Proizvoditel.Add(proizvoditel);

                db.SaveChanges();
                DialogResult = true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Введи числа в поля цены, количества и скидки.");
            }
            catch (Exception oshibka)
            {
                MessageBox.Show(oshibka.GetBaseException().Message, "Ошибка сохранения");
            }
        }
    }
}