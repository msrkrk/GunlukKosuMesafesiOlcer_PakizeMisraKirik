namespace GunlukKosuMesafesiOlcer_PakizeMisraKirik
{
    internal class Program
    {
        #region Ortak
        // Kullanıcıdan geçerli bir tam sayı alınmasını sağlar.
        static int InputIntegerValue(string consoleMsg, int minValue)
        {
            int value = 0;

            while (true)
            {
                try
                {
                    Console.WriteLine(consoleMsg);
                    value = Convert.ToInt32(Console.ReadLine());

                    if (value < minValue)
                    {
                        Console.WriteLine($"Lütfen minimum {minValue} değerinden büyük bir değer giriniz.");
                        Console.WriteLine();
                        continue;
                    }

                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Hatalı giriş yaptınız. Lütfen geçerli bir tamsayı giriniz.");
                    Console.WriteLine();
                }
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

        // Koşunun kaç bölümden oluşacağını tamsayı olarak alır.
        static int InputRunningStageCount()
        {
            return InputIntegerValue("Koşunuz kaç bölümden oluşmaktadır.", 1);
        }

        // Bir dakikada kaç adım koştuğunu kullıcıdan input olarak alır.
        // Sonuç integer cinsinden adım sayısı döner.
        static int InputStepCountPerMin()
        {
            return InputIntegerValue("Bir dakikada kaç adım koştuğunuzu giriniz.", 1);
        }

        // Koşu süresini saat ve dakika cinsinden input olarak kullanıcıdan alır.
        // Sonuç integer cinsinden dakika değerini döner.
        static int InputRunningDurationMin()
        {
            int totalDuration = 0;

            while (totalDuration == 0)
            {
                int hours = InputIntegerValue("Koşu süresi saat giriniz.", 0);

                int min = InputIntegerValue("Koşu süresi dakika giriniz.", 0);

                totalDuration = hours * 60 + min;

                if (totalDuration == 0)
                {
                    Console.WriteLine("Toplam süre 0dk olamaz. Lütfen en az 1dk süre giriniz.");
                }
            }

            return totalDuration;
        }

        // Kullanıcıdan girdileri toplayan metoddur.
        static void InputRunningParams(out int avgStepLength, out int[] stepCountPerMinArr, out int[] runningDurationMinArr)
        {
            Console.WriteLine("========== GÜNLÜK KOŞU MESAFESİ HESAPLAYICI ==========");
            Console.WriteLine("Hoşgeldiniz. Bu uygulama girdiğiniz bilgiler doğrultusunda koşu mesafenizi hesaplayacaktır. Keyifli koşular dileriz.");

            Console.WriteLine();

            avgStepLength = InputAvgStepLength();

            int runningStageCount = InputRunningStageCount();

            stepCountPerMinArr = new int[runningStageCount];
            runningDurationMinArr = new int[runningStageCount];

            for (int i = 0; i < runningStageCount; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"==> Koşunun {i + 1}. bölümü için:");
                stepCountPerMinArr[i] = InputStepCountPerMin();
                runningDurationMinArr[i] = InputRunningDurationMin();
            }
        }
        #endregion

        #region Hesaplama Modülü

        // Girilen adım uzunluğu, koşu hızı ve süre parametreleri kullanarak mesafe hesaplar, metre cinsinden döner.
        static void CalculateDailyRunningDistance(int avgStepLength, int[] stepCountPerMinArr, int[] runningDurationMinArr, out int distance)
        {
            // stepCountPerMinArr ve runningDurationMinArr dizilerinin uzunluklarının aynı olduklarından eminiz. Hesaplamamız buna göre yapılacaktır. Öyle gelmezse argüman hatası fırlatılır.
            if (stepCountPerMinArr.Length != runningDurationMinArr.Length)
            {
                throw new ArgumentException("stepCountPerMinArr ve runningDurationMinArr dizilerinin uzunlukları aynı olmalıdır.");
            };

            distance = 0;

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
            // stepCountPerMinArr ve runningDurationMinArr dizilerinin uzunluklarının aynı olduklarından eminiz. Hesaplamamız buna göre yapılacaktır. Öyle gelmezse argüman hatası fırlatılır.
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

        }

        #endregion

        static void Main(string[] args)
        {
            InputRunningParams(out int avgStepLength, out int[] stepCountPerMinArr, out int[] runningDurationMinArr);

            CalculateDailyRunningDistance(avgStepLength, stepCountPerMinArr, runningDurationMinArr, out int distance);

            OutputResult(avgStepLength, stepCountPerMinArr, runningDurationMinArr, distance);
        }
    }
}
