using System;
using System.ComponentModel;
using HackathonExample.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HackathonExample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly BaseViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            _viewModel = new RemoteViewModel();
            this.BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                await _viewModel.OnAppearing();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error - OnAppearing", ex.Message, "Ok");
            }
        }

        protected override async void OnDisappearing()
        {            
            try
            {
                base.OnDisappearing();
                await _viewModel.OnDisappearing();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error - OnDisappearing", ex.Message, "Ok");
            }
        }
    }
}
