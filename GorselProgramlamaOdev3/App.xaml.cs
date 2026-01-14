namespace GorselProgramlamaOdev3;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Authentication servisini başlat
        AuthenticationService.Initialize();

        // Her zaman Login sayfasından başla
        MainPage = new NavigationPage(new LoginPage());
    }

    // Giriş başarılı olduğunda bu metodu çağır
    public void NavigateToMainApp()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            MainPage = new AppShell();
        });
    }

    protected override void OnStart()
    {
        base.OnStart();

        // Uygulama başlarken otomatik giriş kontrolü
        CheckAutoLogin();
    }

    private async void CheckAutoLogin()
    {
        await Task.Delay(100); // UI'ın yüklenmesini bekle

        string userId = Preferences.Get("UserId", string.Empty);

        if (!string.IsNullOrEmpty(userId))
        {
            // Kullanıcı daha önce giriş yapmış - Firebase'i başlat
            FireBaseServices.Initialize(userId);

            // Ana sayfaya yönlendir
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MainPage = new AppShell();
            });
        }
    }
}