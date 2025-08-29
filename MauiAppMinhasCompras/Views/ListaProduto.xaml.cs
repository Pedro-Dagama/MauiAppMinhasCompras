using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

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
        base.OnAppearing();

        lista.Clear(); // Evita duplicação
        List<Produto> tmp = await App.Db.GetAll();
        tmp.ForEach(i => lista.Add(i));
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

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        string msg = $"Total é {soma:C2}";
        DisplayAlert("Total dos produtos", msg, "Ok");
    }

    private void MenuItem_Clicked(object sender, EventArgs e)
    {
        // Exemplo: deletar produto ou outra ação de contexto
    }
}
