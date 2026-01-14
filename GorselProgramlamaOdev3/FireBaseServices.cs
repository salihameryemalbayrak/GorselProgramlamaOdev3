using Firebase.Database;
using Firebase.Database.Query;
using GorselProgramlamaOdev3.Models;
using System.Collections.ObjectModel;

namespace GorselProgramlamaOdev3
{
    public static class FireBaseServices
    {
        private static FirebaseClient firebaseClient;
        private static string currentUserId;

        // ⚠️ BU SATIRI KENDİ DATABASE URL'İNİZLE DEĞİŞTİRİN
        private const string FIREBASE_DATABASE_URL = Secrets.DatabaseUrl;

        // Firebase bağlantısını başlat
        public static void Initialize(string userId)
        {
            currentUserId = userId;
            firebaseClient = new FirebaseClient(FIREBASE_DATABASE_URL);
        }

        // Kullanıcı ID'sini ayarla
        public static void SetUserId(string userId)
        {
            currentUserId = userId;
        }

        // Yeni görev ekle
        public static async Task<bool> AddGorevAsync(Gorev gorev)
        {
            try
            {
                if (string.IsNullOrEmpty(currentUserId))
                {
                    throw new Exception("Kullanıcı oturum açmamış!");
                }

                // Görev için benzersiz ID oluştur
                gorev.Id = Guid.NewGuid().ToString();

                // Firebase'e ekle - kullanıcı bazlı
                await firebaseClient
                    .Child("gorevler")
                    .Child(currentUserId)
                    .Child(gorev.Id)
                    .PutAsync(gorev);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Görev ekleme hatası: {ex.Message}");
                throw; // Hatayı üst katmana fırlat
            }
        }

        // Görev güncelle
        public static async Task<bool> UpdateGorevAsync(string gorevId, Gorev gorev)
        {
            try
            {
                if (string.IsNullOrEmpty(currentUserId))
                {
                    throw new Exception("Kullanıcı oturum açmamış!");
                }

                await firebaseClient
                    .Child("gorevler")
                    .Child(currentUserId)
                    .Child(gorevId)
                    .PutAsync(gorev);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Görev güncelleme hatası: {ex.Message}");
                throw;
            }
        }

        // Görev sil
        public static async Task<bool> DeleteGorevAsync(string gorevId)
        {
            try
            {
                if (string.IsNullOrEmpty(currentUserId))
                {
                    throw new Exception("Kullanıcı oturum açmamış!");
                }

                await firebaseClient
                    .Child("gorevler")
                    .Child(currentUserId)
                    .Child(gorevId)
                    .DeleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Görev silme hatası: {ex.Message}");
                throw;
            }
        }

        // Tüm görevleri getir
        public static async Task<ObservableCollection<Gorev>> GetAllGorevlerAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(currentUserId))
                {
                    throw new Exception("Kullanıcı oturum açmamış!");
                }

                var gorevler = await firebaseClient
                    .Child("gorevler")
                    .Child(currentUserId)
                    .OnceAsync<Gorev>();

                var collection = new ObservableCollection<Gorev>();

                foreach (var item in gorevler)
                {
                    var gorev = item.Object;
                    if (gorev != null)
                    {
                        gorev.Id = item.Key;
                        collection.Add(gorev);
                    }
                }

                return collection;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Görev getirme hatası: {ex.Message}");
                return new ObservableCollection<Gorev>();
            }
        }

        // Görev durumunu güncelle (yapıldı/yapılmadı)
        public static async Task<bool> UpdateGorevDurumAsync(string gorevId, bool yapildi)
        {
            try
            {
                if (string.IsNullOrEmpty(currentUserId))
                {
                    throw new Exception("Kullanıcı oturum açmamış!");
                }

                await firebaseClient
                    .Child("gorevler")
                    .Child(currentUserId)
                    .Child(gorevId)
                    .Child("Yapildi")
                    .PutAsync(yapildi);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Görev durum güncelleme hatası: {ex.Message}");
                throw;
            }
        }
    }
}