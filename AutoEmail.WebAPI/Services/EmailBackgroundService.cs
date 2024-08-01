namespace AutoEmail.WebAPI.Services
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IEmailService _emailService;
        private Timer _timer;

        public EmailBackgroundService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Mevcut tarih ve saat bilgilerini alıyoruz.
            var now = DateTime.Now;

            // Bir sonraki çalışma zamanını ayarlıyoruz. Burada, yarının tarihi ve saat 00:00 olarak belirlenmiştir.
            var nextRun = DateTime.Today.AddDays(1).AddHours(0);

            // Eğer mevcut zaman, belirlenen bir sonraki çalışma zamanından (nextRun) sonra ise,
            // bu durumda bir sonraki günün çalışma zamanı olarak nextRun'u güncelliyoruz. 
            // Örneğin, eğer şu an gece 1 ise ve nextRun gece 12 olarak belirlenmişse,
            // nextRun bir gün ileriye alınır.
            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1);
            }

            // Mevcut zamandan bir sonraki çalışma zamanına kadar olan süreyi hesaplıyoruz.
            // Bu süre, zamanlayıcının ilk çalıştırılması için gereken gecikmeyi temsil eder.
            var delay = nextRun - now;

            // Zamanlayıcının ne sıklıkla çalışacağını belirliyoruz. Burada, zamanlayıcının her gün bir kez çalışması için
            // interval olarak 1 gün (TimeSpan.FromDays(1)) belirliyoruz.
            var interval = TimeSpan.FromDays(1);

            // Timer nesnesi oluşturuyoruz ve SendEmail metodunu belirtilen gecikme süresi kadar sonra çalıştıracağız.
            // Zamanlayıcı, her gün bir kez belirlediğimiz interval ile çalışacak.
            _timer = new Timer(SendEmail, null, delay, interval);

            // İşlemi tamamladık, bu noktada asenkron görevimizin tamamlandığını belirtiyoruz.
            await Task.CompletedTask;
        }

        private async void SendEmail(object state)
        {
            string to = "example@example.com";
            string subject = "subject";
            string mailContent = $"Mail From {Environment.MachineName} at {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}";
            await _emailService.SendEmailAsync(to, subject, mailContent);
        }
    }
}