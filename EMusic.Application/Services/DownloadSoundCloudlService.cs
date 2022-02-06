using EMusic.Application.Interfaces;
using Infrastructure.Services.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace EMusic.Application.Services
{
    public class DownloadSoundCloudlService : IDownloadSoundCloudlService
    {
        readonly IZipFileMemoryStream _zipFileMemoryStreamService;
        const string PathElementsSoundCloud = "//a[@class = 'sc-link-primary soundTitle__title sc-link-dark sc-text-h4']";

        public DownloadSoundCloudlService(IZipFileMemoryStream zipFileMemoryStreamService)
        {
            _zipFileMemoryStreamService = zipFileMemoryStreamService;
        }
        public async Task<MemoryStream> Download(Uri urlSoundCloud)
        {
            if (!urlSoundCloud.IsAbsoluteUri)
            {
                throw new Exception("urlSoundCloud vazia");
            }

            return await ProcessDownload(urlSoundCloud);

        }

        public async Task<MemoryStream> ProcessDownload(Uri? url)
        {

            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathSoundCloud = Path.Combine(userPath, @"C:\SoundCloud");


            if (!Directory.Exists(pathSoundCloud))
            {
                Directory.CreateDirectory(pathSoundCloud);
            }

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", pathSoundCloud);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");

            var _chromeDriverInstance = new ChromeDriver(Environment.CurrentDirectory, chromeOptions);

            _chromeDriverInstance.Url = url.AbsoluteUri;

            _chromeDriverInstance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            //IJavaScriptExecutor executorScrollJavascript = _chromeDriverInstance as IJavaScriptExecutor;
            //for (int i = 0; i < 3; i++)
            //{
            //}

            var taskfinal = DownloadMusicsSoundCloud(_chromeDriverInstance, AddHrefLinkInList(_chromeDriverInstance));

            await Task.WhenAll(taskfinal);
            await Task.Delay(60000);

            Dispose(_chromeDriverInstance);

            return await _zipFileMemoryStreamService.ZipFile(pathSoundCloud);
        }
        private static void Dispose(ChromeDriver _chromeDriverInstance)
        {
            _chromeDriverInstance.Close();
            _chromeDriverInstance.Quit();
            _chromeDriverInstance.Dispose();
        }

        private static List<string> AddHrefLinkInList(ChromeDriver _chromeDriverInstance)
        {
            List<string> liUrlsDeMusicas = new List<string>();

            var elementosHtmlComLinkDasMusicas = _chromeDriverInstance.FindElements(By.XPath(PathElementsSoundCloud));

            foreach (IWebElement linkMusicas in elementosHtmlComLinkDasMusicas)
            {
                liUrlsDeMusicas.Add(string.Format(linkMusicas.GetAttribute("href").ToString()));
            }
            return liUrlsDeMusicas;
        }

        private async Task DownloadMusicsSoundCloud(ChromeDriver _chromeDriverInstance, List<string> liUrlsMusic)
        {
            foreach (var item in liUrlsMusic)
            {
                _chromeDriverInstance.Url = "https://scloudtomp3downloader.com/";

                _chromeDriverInstance.FindElement(By.XPath("//input[@name= 'url']")).SendKeys(item);

                _chromeDriverInstance.FindElement(By.XPath("//button[@class= 'btn btn-warning btn-download']")).Click();

                if (ExistErrorOnConverter(_chromeDriverInstance)) continue;

                _chromeDriverInstance.FindElement(By.XPath("//a[@class='btn btn-success btn-sq btn-dl']")).Click();
            }

        }

        private static bool ExistErrorOnConverter(ChromeDriver _chromeDriverInstance)
        {
            List<IWebElement> liErrorElements = new List<IWebElement>();
            liErrorElements.AddRange(_chromeDriverInstance.FindElements
            (By.XPath("//div[@class='popover-body']")));

            return liErrorElements.Count > 0 ? true : false;
        }
    }
}
