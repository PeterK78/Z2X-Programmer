using Z2XProgrammer.Pages;
using Z2XProgrammer.ViewModel;

namespace Z2XProgrammer
{
    public partial class AppShell : Shell
    {
        internal AppShell(AppShellViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

        }
    }
}
