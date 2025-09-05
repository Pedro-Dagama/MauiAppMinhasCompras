using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            lista.Clear(); // Evita duplicação
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue?.Trim() ?? string.Empty;

            lista.Clear();

            List<Produto> tmp = string.IsNullOrEmpty(q)
                ? await App.Db.GetAll()   // Se busca vazia, lista tudo
                : await App.Db.Search(q); // Corrigido o nome do método ;)

            tmp.ForEach(i => lista.Add(i));

            // Opcional: feedback se nada foi encontrado
            if (lista.Count == 0 && !string.IsNullOrEmpty(q))
                await DisplayAlert("Busca", "Nenhum produto encontrado", "Ok");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        string msg = $"Total é {soma:C2}";
        DisplayAlert("Total dos produtos", msg, "Ok");
    }

    private async void MenuItem_Clickedd(object sender, EventArgs e)
    {
        try {
            MenuItem selecionado = sender as MenuItem;
            Produto p = selecionado.BindingContext as Produto;
            bool confirm = await DisplayAlert("Atenção", $"Confirma a exclusão do produto?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch(Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;
            {
                Navigation.PushAsync(new Views.EditarProduto { BindingContext = p});
            }
            ;

        } catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }
}
