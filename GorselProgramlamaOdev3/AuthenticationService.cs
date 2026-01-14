using Firebase.Auth;
using Firebase.Auth.Providers;

namespace GorselProgramlamaOdev3
{
    public class AuthenticationService
    {
      
        private const string FIREBASE_API_KEY = Secrets.FirebaseApiKey;

        private static FirebaseAuthClient authClient;

        // Firebase Authentication Client'ı başlat
        public static void Initialize()
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = FIREBASE_API_KEY,
                AuthDomain = Secrets.AuthDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };

            authClient = new FirebaseAuthClient(config);
        }

        // Kullanıcı kaydı
        public static async Task<(bool success, string userId, string error)> RegisterAsync(string email, string password)
        {
            try
            {
                var userCredential = await authClient.CreateUserWithEmailAndPasswordAsync(email, password);

                if (userCredential?.User != null)
                {
                    return (true, userCredential.User.Uid, null);
                }

                return (false, null, "Kayıt başarısız oldu.");
            }
            catch (Exception ex)
            {
                return (false, null, GetUserFriendlyErrorMessage(ex.Message));
            }
        }

        // Kullanıcı girişi
        public static async Task<(bool success, string userId, string error)> LoginAsync(string email, string password)
        {
            try
            {
                var userCredential = await authClient.SignInWithEmailAndPasswordAsync(email, password);

                if (userCredential?.User != null)
                {
                    return (true, userCredential.User.Uid, null);
                }

                return (false, null, "Giriş başarısız oldu.");
            }
            catch (Exception ex)
            {
                return (false, null, GetUserFriendlyErrorMessage(ex.Message));
            }
        }

        // Çıkış yapma
        public static void Logout()
        {
            authClient?.SignOut();
            Preferences.Remove("UserId");
            Preferences.Remove("UserEmail");
        }

        // Kullanıcı dostu hata mesajları
        private static string GetUserFriendlyErrorMessage(string errorMessage)
        {
            if (errorMessage.Contains("EMAIL_EXISTS"))
                return "Bu e-posta adresi zaten kullanılıyor.";

            if (errorMessage.Contains("INVALID_EMAIL"))
                return "Geçersiz e-posta adresi.";

            if (errorMessage.Contains("WEAK_PASSWORD"))
                return "Şifre en az 6 karakter olmalıdır.";

            if (errorMessage.Contains("EMAIL_NOT_FOUND"))
                return "Bu e-posta adresi ile kayıtlı kullanıcı bulunamadı.";

            if (errorMessage.Contains("INVALID_PASSWORD"))
                return "Hatalı şifre.";

            if (errorMessage.Contains("USER_DISABLED"))
                return "Bu hesap devre dışı bırakılmış.";

            return $"Bir hata oluştu: {errorMessage}";
        }

        // Mevcut kullanıcıyı kontrol et
        public static bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(Preferences.Get("UserId", string.Empty));
        }

        // Mevcut kullanıcı ID'sini al
        public static string GetCurrentUserId()
        {
            return Preferences.Get("UserId", string.Empty);
        }
    }
}