using Xamarin.Forms.Xaml;

namespace Metars.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopup
    {
        public string TitleText { get; }
        public LoadingPopup(string title)
        {
            InitializeComponent();
            TitleText = title;
            BindingContext = this;
        }
    }
}