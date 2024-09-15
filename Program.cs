using System.Data;

namespace GunlukKosuMesafesiOlcer_PakizeMisraKirik
{
    internal class Program
    {
        #region Ortak
        // Kullanıcıdan geçerli bir tam sayı alınmasını sağlar.
        // Consola parametre olarak verilen consoleMsg mesajını yazar.
        // Doğru formatta bir tam sayı alınabilmesi için gerekli kontrolleri yapar.
        // Girilen değerin verilen minValue parametre değerinden büyük girilmesini sağlar.
        static int InputIntegerValue(string consoleMsg, int minValue)
        {
            int value = 0;

            while (true)
            {
                try
                {
                    Console.Write($"{consoleMsg}: ");
                    value = Convert.ToInt32(Console.ReadLine());

                    if (value < minValue)
                    {
                        throw new InvalidOperationException($"Lütfen minimum {minValue} değerinden büyük bir değer giriniz.");
                    }

                    break;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Hatalı formatta giriş yaptınız. Lütfen geçerli bir tamsayı giriniz.");
                    Console.WriteLine();
                }
                catch (OverflowException)
                {
                    Console.WriteLine($"Girdiğiniz sayı çok büyük ya da çok küçük. Lütfen {minValue} ile {int.MaxValue} arasında bir tamsayı giriniz.");
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Bilinmeyen bir hata oluştu. Lütfen tekrar deneyin.");
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                }
            }

            return value;
        }
        #endregion

        #region Karşılama Modülü
        // Kullanıcıya programı tanıtan alandır.
        static void SayWelcome()
        {
            Console.WriteLine("========== GÜNLÜK KOŞU MESAFESİ HESAPLAYICI ==========");
            Console.WriteLine("Hoşgeldiniz. Bu uygulama girdiğiniz bilgiler doğrultusunda koşu mesafenizi hesaplayacaktır. Keyifli koşular dileriz.");

            Console.WriteLine();
        }
        // Kullanıcıya başlama ve uygulamadan çıkma talimatı verdiren alandır.
        static bool AskToStartRunning()
        {
            string value = "";

            while (value != "b" && value != "ç")
            {
                Console.Write("Koşu başlat:(B) Çıkış:(Ç) ?= ");

                value = Console.ReadLine().ToLower();

                if (value == "ç")
                {
                    Environment.Exit(0);
                    return false;
                }

                if (value == "b")
                    return true;

                Console.WriteLine("Hatalı giriş.");
            }

            return false;
        }
        //Kullanıcıya koşu esnasında hangi değişiklik yapacağını soran alandır.
        static string AskToContinueRunning()
        {
            string value = "";

            while (value != "b" && value != "h" && value != "ç")
            {
                Console.Write("Koşuyu bitir:(B) Hızı Değiştir:(H) Çıkış:(Ç) ?= ");

                value = Console.ReadLine().ToLower();

                if (value == "ç")
                {
                    Environment.Exit(0);
                    break;
                }

                if (value == "b")
                    break;

                if (value == "h")
                    break;

                Console.WriteLine("Hatalı giriş.");
            }

            return value;
        }
        #endregion

        #region Veri Giriş Modülü
        // Ortalama bir adımınızın kaç santimetre olduğunu kullanıcıdan input olarak alır.
        // Sonuç int cinsinden santimetre olarak döner.
        static int InputAvgStepLength()
        {
            return InputIntegerValue("Bir adımınızın ortalama uzunluğunu giriniz.(cm)", 1);
        }

        // Bir dakikada kaç adım koştuğunu kullıcıdan input olarak alır.
        // Sonuç integer cinsinden adım sayısı döner.
        static int InputStepCountPerMin()
        {
            return InputIntegerValue("Dakikada kaç adım hızla koşacaksınız?", 1);
        }

        // Koşu süresini saat ve dakika cinsinden input olarak kullanıcıdan alır.
        // Sonuç integer cinsinden dakika değerini döner.
        static int InputRunningDurationMin()
        {
            Console.WriteLine("Ne kadar süredir bu hızla koşuyordunuz?");

            // Kullanıcıdan geçerli süre parametrelerinin alındığından emin olana kadar sor.
            while (true)
            {
                // Bilgileri al.
                int hours = InputIntegerValue("Süre saat", 0);
                int mins = InputIntegerValue("Süre dakika", 0);

                // Bilgilerin sıfır olup olmadığını kontrol et.
                if (hours == 0 && mins == 0)
                {
                    // Hem saat hem de dakika bilgisi 0 girildi.
                    // Toplam süre 0 olamaz.
                    // Kullanıcıya uyarı mesajı göster.
                    Console.WriteLine("Toplam süre 0dk olamaz. Lütfen en az 1dk süre giriniz.");

                    // Döngüyü devam ettir. Saat ve dakika bilgisini tekrar sor.
                    continue;
                }

                // If koşuluna girmedi.
                // Toplam süreyi döndür. 
                // Dögüden çık. Methoddan çık.
                return hours * 60 + mins;
            }
        }

        // Koşu süresince koşu verilerini toplayan method.
        // Koşunun farklı hızla koşulan bölümlerinin yönetimi ve veri toplaması da burada yapılır.
        // Methodun çalışma biçimi basitçe, ortalama  adım uzunluğunu tek seferde alır.
        // Sonra sonsuz bir döngü başlatıp kullanıcı dur diyene kadar yeni koşu hızı ve süresi bilgilerini toplar.
        static void InputRunningParams(out int avgStepLength, out int[] stepCountPerMinArr, out int[] runningDurationMinArr)
        {
            avgStepLength = InputAvgStepLength();

            // Koşu bölümleri için hız ve süre bilgilerini tutan listelerdir.
            // Listelerin her bir indislerindeki elemanlar,
            // koşunun o indiste denk gelen bölümünün hız ve süre bilgilerine karşılık gelir.
            // Koşunun kaç bölümden oluşacağı bilgisi net olmadığı için liste yapısını kullan.
            List<int> stepCountPerMinList = new List<int>();
            List<int> runningDurationMinList = new List<int>();

            while (true)
            {
                // Koşu hızı bilgisini input al ve koşu bölümüne ait parametre listesinin içine at.
                int stepCountPerMin = InputStepCountPerMin();
                stepCountPerMinList.Add(stepCountPerMin);

                Console.WriteLine();
                Console.WriteLine($"{stepCountPerMin}adım/dk hızla koşuyorsunuz...");
                Console.WriteLine();

                // Kullanıcıya koşuyla ilgili ne yapmak istediğini sor.
                string response = AskToContinueRunning();

                // Kullanıcı bir komut girdi demektir. 
                // Hangi komutu girmiş olursa olsun (Koşuyu Bitir ya da Hızı Değiştir) 
                // bir önceki hız parametresi ile ne kadar süre koştuğu bilgisini al.
                int runningDurationMin = InputRunningDurationMin();
                runningDurationMinList.Add(runningDurationMin);

                // Kullanıcı hız değiştirmek istiyor.
                // Dögünün kalanını yapma başa dön ve yeni bir bölüm oluşması için
                // kullanıcıya ne hızda koşacağı bilgisini sor. 
                if (response == "h")
                {
                    Console.WriteLine();
                    Console.WriteLine("Hız parametresi değiştiriliyor...");
                    Console.WriteLine();
                    continue;
                }

                // Kullanıcı koşuyu bitirmek istiyor. 
                // Döngüyü kır ve programın devam etmesini sağla.
                // Bu adımdan sonra parametre toplama metodunun işi bitiyor.
                // Toplanan parametreler out ile dışarıya çıkarılacak.
                if (response == "b")
                {
                    Console.WriteLine();
                    Console.WriteLine("Koşu bitti. Mesafe hesaplanıyor...");
                    Console.WriteLine();
                    break;
                }

            }

            // Metodun out parametre tipleri dizi olduğu için listeleri ToArray kullanarak diziye çevir.
            stepCountPerMinArr = stepCountPerMinList.ToArray();
            runningDurationMinArr = runningDurationMinList.ToArray();

        }
        #endregion

        #region Hesaplama Modülü

        // Girilen adım uzunluğu, koşu hızı ve süre parametreleri kullanarak mesafe hesaplar, metre cinsinden döner.
        static void CalculateDailyRunningDistance(int avgStepLength, int[] stepCountPerMinArr, int[] runningDurationMinArr, out int distance)
        {
            // stepCountPerMinArr ve runningDurationMinArr dizilerinin uzunluklarının aynı olduklarından eminiz.
            // Hesaplamamız buna göre yapılacaktır. Öyle gelmezse argüman hatası fırlatılır.
            if (stepCountPerMinArr.Length != runningDurationMinArr.Length)
            {
                throw new ArgumentException("stepCountPerMinArr ve runningDurationMinArr dizilerinin uzunlukları aynı olmalıdır.");
            };

            // ilk değer ata.
            // parametre başka bir ilk değer ile gönderilse bile sıfırlayıp hesapla.
            distance = 0;

            // Her bir koşu bölümündeki hız ve süre parametrelerini kullanarak mesafeyi hesapla.
            for (int i = 0; i < stepCountPerMinArr.Length; i++)
            {
                int stepCountPerMin = stepCountPerMinArr[i];
                int runningDurationMin = runningDurationMinArr[i];

                /**
                 * 
                 * Ortalama bir adımı 50cm olan bir kişi dakikada 10 adımlık hız ile 10dk koşarsa;
                 * Mesafe = Adım uzunluğu x Koşu hızı x Koşu süresi şeklinde;
                 * 50 x 10 x 10 = 5000cm mesafe koşacaktır.
                 * 5000cm = 50m 
                 *
                 * Her bir bölüm için mesafe toplam değere eklenir.
                 */

                distance += avgStepLength * stepCountPerMin * runningDurationMin / 100;
            }
        }
        #endregion

        #region Sonuç Modülü

        //Kullanıcı girdilerini ve hesaplanan mesafeyi ekrana yazdırır.
        static void OutputResult(int avgStepLength, int[] stepCountPerMinArr, int[] runningDurationMinArr, int distance)
        {
            // stepCountPerMinArr ve runningDurationMinArr dizilerinin uzunluklarının aynı olduklarından eminiz.
            // Hesaplamamız buna göre yapılacaktır. Öyle gelmezse argüman hatası fırlatılır.
            if (stepCountPerMinArr.Length != runningDurationMinArr.Length)
            {
                throw new ArgumentException("stepCountPerMinArr ve runningDurationMinArr dizilerinin uzunlukları aynı olmalıdır.");
            }

            Console.WriteLine();
            Console.WriteLine("========== DEĞERLER ==========");
            Console.WriteLine($"Adım Uzunluğu: {avgStepLength}cm");

            for (int i = 0; i < stepCountPerMinArr.Length; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"{i + 1}. Bölüm:");
                Console.WriteLine($"Koşu Hızı: {stepCountPerMinArr[i]}adım/dk");
                Console.WriteLine($"Koşu Süresi: {runningDurationMinArr[i]}dk");
            }

            Console.WriteLine();
            Console.WriteLine("========== SONUÇ ==========");
            Console.WriteLine($"Girilen değerlere göre hesaplanan günlük koşu mesafeniz: {distance}m");
            Console.WriteLine();
            Console.WriteLine();

        }

        #endregion

        static void Main(string[] args)
        {
            SayWelcome();

            while (AskToStartRunning())
            {
                InputRunningParams(out int avgStepLength, out int[] stepCountPerMinArr, out int[] runningDurationMinArr);

                CalculateDailyRunningDistance(avgStepLength, stepCountPerMinArr, runningDurationMinArr, out int distance);

                OutputResult(avgStepLength, stepCountPerMinArr, runningDurationMinArr, distance);
            }
        }
    }
}
