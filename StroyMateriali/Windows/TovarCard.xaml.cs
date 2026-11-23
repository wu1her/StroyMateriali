using StroyMateriali.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StroyMateriali.Windows
{
    public partial class TovarCard : UserControl
    {
        public TovarCard(Tovar tovar)
        {
            InitializeComponent();

            ArticulImaBlock.Text = "Артикул: "+ tovar.Artikul + " | Имя:" + tovar.Ima;
            OpisanieBlock.Text = "Описание: "+ tovar.Opisanie;
            ProizvoditelBlock.Text = "Производитель: "+ tovar.ImaProizvoditel;
            PostavshikBlock.Text = "Поставщик: " + tovar.ImaPostavshika;
            CenaBlock.Text = "Цена: " + tovar.Cena.ToString();
            EdinicaBlock.Text = "Единица измерения: "+ tovar.Edinica;
            KolichestvoBlock.Text = "Количество на складе брат: " + tovar.Kolichestvo.ToString();
            SkidkaBlock.Text = "Скидка: " + tovar.Skidka;
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


            if (tovar.Skidka > 12)
                Background = (Brush)new BrushConverter().ConvertFromString("#F4A460");
            if (tovar.Skidka != 0)
            {
                CenaBlock.Foreground = Brushes.Red;
                CenaBlock.TextDecorations = TextDecorations.Strikethrough;
                
                var itogcena = tovar.Cena - tovar.Cena * tovar.Skidka / 100;
                ItogCenaBlock.Text = itogcena.ToString();
            }
            if (tovar.Kolichestvo == 0)
            {
                Background = (Brush)new BrushConverter().ConvertFromString("LightBlue");
            }
        }
    }
}
