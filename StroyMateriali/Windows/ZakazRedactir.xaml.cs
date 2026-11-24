using StroyMateriali.Models;
using System;
using System.Linq;
using System.Windows;

namespace StroyMateriali.Windows
{
    public partial class ZakazRedactir : Window
    {
        private readonly int? nomerZakaza;

        public ZakazRedactir(int? nomer)
        {
            InitializeComponent();
            nomerZakaza = nomer;

            try
            {
                using (var db = new StroyMaterialiEntities())
                {
                    StatusBox.ItemsSource = db.Status.ToList();
                    PynktBox.ItemsSource = db.Pynkti.ToList();

                    if (nomer == null)
                    {
                        Title = "Добавление заказа";
                        NomerBox.Text = "Автоматически";
                        DataZakazaBox.SelectedDate = DateTime.Today;
                        DataDostavkaBox.SelectedDate = DateTime.Today;
                        return;
                    }

                    Title = "Редактирование заказа";
                    var zakaz = db.Zakaz.Find(nomer.Value);

                    if (zakaz == null)
                    {
                        Save.IsEnabled = false;
                        MessageBox.Show("Заказ уже удалён.");
                        return;
                    }

                    NomerBox.Text = zakaz.Nomer.ToString();
                    StatusBox.SelectedItem = zakaz.Status.FirstOrDefault();
                    PynktBox.SelectedItem = zakaz.Pynkti.FirstOrDefault();
                    DataZakazaBox.SelectedDate = zakaz.DataZakaza;
                    DataDostavkaBox.SelectedDate = zakaz.DataDostavka;

                    Udalit.Visibility = Visibility.Visible;
                }
            }
            catch (Exception oshibka)
            {
                Save.IsEnabled = false;
                MessageBox.Show(oshibka.GetBaseException().Message, "Ошибка загрузки заказа");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var status = StatusBox.SelectedItem as Status;
            var pynkt = PynktBox.SelectedItem as Pynkti;
            var dataZakaza = DataZakazaBox.SelectedDate;
            var dataDostavki = DataDostavkaBox.SelectedDate;

            if (status == null || pynkt == null || dataZakaza == null || dataDostavki == null)
            {
                MessageBox.Show("Выбери статус, пункт выдачи и обе даты.");
                return;
            }

            if (dataDostavki.Value < dataZakaza.Value)
            {
                MessageBox.Show("Дата доставки раньше даты заказа.");
                return;
            }

            try
            {
                using (var db = new StroyMaterialiEntities())
                {
                    Zakaz zakaz;

                    if (nomerZakaza == null)
                    {
                        zakaz = new Zakaz();
                        zakaz.Nomer = (db.Zakaz.Max(z => (int?)z.Nomer) ?? 0) + 1;
                        zakaz.Kod = (db.Zakaz.Max(z => (int?)z.Kod) ?? 0) + 1;
                        db.Zakaz.Add(zakaz);
                    }
                    else
                    {
                        zakaz = db.Zakaz.Find(nomerZakaza.Value);

                        if (zakaz == null)
                        {
                            MessageBox.Show("Заказ уже удалён.");
                            return;
                        }
                    }

                    zakaz.DataZakaza = dataZakaza.Value;
                    zakaz.DataDostavka = dataDostavki.Value;

                    zakaz.Status.Clear();
                    zakaz.Status.Add(db.Status.Find(status.ID_Status));

                    zakaz.Pynkti.Clear();
                    zakaz.Pynkti.Add(db.Pynkti.Find(pynkt.ID_Pynkti));

                    db.SaveChanges();
                }

                DialogResult = true;
            }
            catch (Exception oshibka)
            {
                MessageBox.Show(oshibka.GetBaseException().Message, "Ошибка сохранения");
            }
        }

        private void Udalit_Click(object sender, RoutedEventArgs e)
        {
            if (nomerZakaza == null)
                return;

            if (MessageBox.Show("Удалить заказ № " + nomerZakaza + "?",
                "Удаление", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            try
            {
                using (var db = new StroyMaterialiEntities())
                {
                    var zakaz = db.Zakaz.Find(nomerZakaza.Value);

                    if (zakaz != null)
                    {
                        foreach (var stroka in zakaz.ZakazTovar.ToList())
                            db.ZakazTovar.Remove(stroka);

                        zakaz.Status.Clear();
                        zakaz.Pynkti.Clear();
                        zakaz.Polzovatel.Clear();

                        db.Zakaz.Remove(zakaz);
                        db.SaveChanges();
                    }
                }

                DialogResult = true;
            }
            catch (Exception oshibka)
            {
                MessageBox.Show(oshibka.GetBaseException().Message, "Ошибка удаления");
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}