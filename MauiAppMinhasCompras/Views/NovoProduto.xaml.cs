using Microsoft.Maui.Controls;
using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views
{
    public partial class NovoProduto : ContentPage
    {
        public NovoProduto()
        {
            InitializeComponent();
        }

        private async void ToolbarItem_Clickedd(object sender, EventArgs e)
        {
            try {
                Produto p = new Produto
                {
                    Descricao = txt_descricao.Text,
                    Quantidade = Convert.ToDouble(txt_quantidade.Text),
                    Preco = Convert.ToDouble(txt_preco.Text)
                };

                await App.Db.Insert(p);
                await DisplayAlert("sucesso", "registro inserido", "ok");

            }catch(Exception ex)
            {
                await DisplayAlert("opss", ex.Message, "ok");
            }
        }
    }
}