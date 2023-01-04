using AudioPlayer.ViewModels;

namespace AudioPlayer.Views;

public partial class HomeView : ContentPage
{
    public HomeView()
    {
        InitializeComponent();
        BindingContext = new HomeVM();
    }
}