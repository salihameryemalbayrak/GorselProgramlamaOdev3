namespace GorselProgramlamaOdev3;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        AuthenticationService.Initialize();
    }

    // XAML: Clicked="LoginClicked"
    private async void LoginClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                await DisplayAlert("Hata", "E-posta adresi boş olamaz!", "Tamam");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                await DisplayAlert("Hata", "Şifre boş olamaz!", "Tamam");
                return;
            }

            btnLogin.IsEnabled = false;
            btnRegister.IsEnabled = false;
            btnLogin.Text = "Giriş yapılıyor...";

            var (success, userId, error) = await AuthenticationService.LoginAsync(
                txtEmail.Text.Trim(),
                txtPassword.Text
            );

            if (success)
            {
                Preferences.Set("UserId", userId);
                Preferences.Set("UserEmail", txtEmail.Text.Trim());
                FireBaseServices.Initialize(userId);

                await DisplayAlert("Başarılı", "Giriş başarılı!", "Tamam");

                // ✅ AppShell içinde ana sayfaya geç
                await Shell.Current.GoToAsync("AnaSayfa");

            }
            else
            {
                await DisplayAlert("Hata", error ?? "Giriş yapılamadı!", "Tamam");
                btnLogin.IsEnabled = true;
                btnRegister.IsEnabled = true;
                btnLogin.Text = "Giriş Yap";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Beklenmeyen hata: {ex.Message}", "Tamam");
            btnLogin.IsEnabled = true;
            btnRegister.IsEnabled = true;
            btnLogin.Text = "Giriş Yap";
        }
    }

    // XAML: Clicked="RegisterClicked"
    private async void RegisterClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                await DisplayAlert("Hata", "Ad Soyad boş olamaz!", "Tamam");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRegisterEmail.Text))
            {
                await DisplayAlert("Hata", "E-posta adresi boş olamaz!", "Tamam");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRegisterPassword.Text))
            {
                await DisplayAlert("Hata", "Şifre boş olamaz!", "Tamam");
                return;
            }

            if (txtRegisterPassword.Text.Length < 6)
            {
                await DisplayAlert("Hata", "Şifre en az 6 karakter olmalıdır!", "Tamam");
                return;
            }

            btnLogin.IsEnabled = false;
            btnRegister.IsEnabled = false;
            btnRegister.Text = "Kayıt yapılıyor...";

            var (success, userId, error) = await AuthenticationService.RegisterAsync(
                txtRegisterEmail.Text.Trim(),
                txtRegisterPassword.Text
            );

            if (success)
            {
                Preferences.Set("UserId", userId);
                Preferences.Set("UserEmail", txtRegisterEmail.Text.Trim());
                Preferences.Set("user_name", txtName.Text.Trim());
                FireBaseServices.Initialize(userId);

                await DisplayAlert("Başarılı", "Kayıt başarılı!", "Tamam");

                // ✅ AppShell içinde ana sayfaya geç
                await Shell.Current.GoToAsync("//AnaSayfa");
            }
            else
            {
                await DisplayAlert("Hata", error ?? "Kayıt oluşturulamadı!", "Tamam");
                btnLogin.IsEnabled = true;
                btnRegister.IsEnabled = true;
                btnRegister.Text = "Kayıt Ol";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Beklenmeyen hata: {ex.Message}", "Tamam");
            btnLogin.IsEnabled = true;
            btnRegister.IsEnabled = true;
            btnRegister.Text = "Kayıt Ol";
        }
    }

    // XAML: Clicked="ShowRegisterForm"
    private void ShowRegisterForm(object sender, EventArgs e)
    {
        loginStack.IsVisible = false;
        registerStack.IsVisible = true;
    }

    // XAML: Clicked="ShowLoginForm"
    private void ShowLoginForm(object sender, EventArgs e)
    {
        registerStack.IsVisible = false;
        loginStack.IsVisible = true;
    }
}
