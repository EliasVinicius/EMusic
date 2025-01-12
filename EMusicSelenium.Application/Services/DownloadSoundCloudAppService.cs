﻿using EMusic.Application.Interfaces;
using EMusic.Infra.Services.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EMusic.Application.Services
{
    public class DownloadSoundCloudAppService : IDownloadSoundCloudAppService
    {
        const string PathElementsSoundCloud = "//a[@class = 'sc-link-primary soundTitle__title sc-link-dark sc-text-h4']";

        public async Task<MemoryStream> Download(Uri urlSoundCloud)
        {
            if (!urlSoundCloud.IsAbsoluteUri)
            {
                throw new Exception("urlSoundCloud vazia");
            }

            return await ProcessDownload(urlSoundCloud);

        }

        private async Task<MemoryStream> ProcessDownload(Uri? url)
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

             DownloadMusicsSoundCloud(_chromeDriverInstance, AddHrefLinkInList(_chromeDriverInstance));

            var zipFiles =  await UtilShared.ZipFiles(pathSoundCloud);
            await Task.Delay(10000);
            UtilShared.DeleteArquives(pathSoundCloud);

            Dispose(_chromeDriverInstance);

            return zipFiles;
        }

        static void Dispose(ChromeDriver _chromeDriverInstance)
        {
            _chromeDriverInstance.Close();
            _chromeDriverInstance.Quit();
            _chromeDriverInstance.Dispose();
        }

        static List<string> AddHrefLinkInList(ChromeDriver _chromeDriverInstance)
        {
            List<string> liUrlsDeMusicas = new List<string>();

            var elementosHtmlComLinkDasMusicas = _chromeDriverInstance.FindElements(By.XPath(PathElementsSoundCloud));

            foreach (IWebElement linkMusicas in elementosHtmlComLinkDasMusicas)
            {
                liUrlsDeMusicas.Add(string.Format(linkMusicas.GetAttribute("href").ToString()));
            }
            return liUrlsDeMusicas;
        }

        static void DownloadMusicsSoundCloud(ChromeDriver _chromeDriverInstance, List<string> liUrlsMusic)
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


        static bool ExistErrorOnConverter(ChromeDriver _chromeDriverInstance)
        {
            List<IWebElement> liErrorElements = new List<IWebElement>();
            liErrorElements.AddRange(_chromeDriverInstance.FindElements
            (By.XPath("//div[@class='popover-body']")));

            return liErrorElements.Count > 0 ? true : false;
        }
    }
}
