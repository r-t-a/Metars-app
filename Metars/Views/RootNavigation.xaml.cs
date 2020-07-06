using Xamarin.Forms;

namespace Metars.Views
{
    public partial class RootNavigation : NavigationPage
    {
        public RootNavigation() : base()
        {
            InitializeComponent();
        }

        public RootNavigation(Page root) : base(root)
        {

        }
    }
}
