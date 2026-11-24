using StroyMateriali.Models;
using System.Linq;
using System.Windows.Controls;

namespace StroyMateriali.Windows
{
    public partial class ZakazCard : UserControl
    {
        public ZakazCard(Zakaz zakaz)
        {
            InitializeComponent();

            var status = zakaz.Status.FirstOrDefault();
            var pynkt = zakaz.Pynkti.FirstOrDefault();

            NomerBlock.Text = "Артикул заказа: " + zakaz.Nomer;
            StatusBlock.Text = "Статус заказа: " + (status?.Ima ?? "Не указан");
            PynktBlock.Text = "Адрес пункта выдачи: " + (pynkt?.Adres ?? "Не указан");
            DataZakazaBlock.Text = "Дата заказа: " + zakaz.DataZakaza.ToString("dd.MM.yyyy");
            DataDostavkaBlock.Text = "Дата доставки: " + zakaz.DataDostavka.ToString("dd.MM.yyyy");
        }
    }
}