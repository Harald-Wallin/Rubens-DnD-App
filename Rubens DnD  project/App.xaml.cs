namespace Rubens_DnD__project
{
    public partial class App : Application
    {
        
        public App()
        {
            InitializeComponent();
            UserAppTheme = AppTheme.Light;
            //MainPage = new NavigationPage(new MainPage());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
