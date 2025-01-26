using DogApi.MVVM.ViewModel;

namespace DogApi.MVVM.View;

public partial class MainPage : ContentPage
{
    private MainPageViewModel _viewModel = new MainPageViewModel();
    public MainPage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }
}