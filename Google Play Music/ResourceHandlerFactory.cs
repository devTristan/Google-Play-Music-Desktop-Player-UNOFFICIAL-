﻿using CefSharp;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Google_Play_Music
{
    class GPMResouceHandlerFactory : IResourceHandlerFactory
    {
        private bool firstJSOnly = true;
        public IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
        {
            if (Regex.Match(request.Url, @".js", RegexOptions.IgnoreCase).Success && Regex.Match(request.Url, @"http", RegexOptions.IgnoreCase).Success && firstJSOnly)
            {
                firstJSOnly = false;
                using (WebClient webClient = new WebClient())
                {
                    // These are the JS files to inject into GPM
                    string dark_theme = Google_Play_Music.Properties.Resources.dark_theme;
                    string custom_interface = Google_Play_Music.Properties.Resources.custom_interface;
                    string mini_player = Google_Play_Music.Properties.Resources.mini_player;

                    Color c = Properties.Settings.Default.CustomColor;
                    string RGB = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
                    string custom_color = ";(function() {window.CustomColor = '" + RGB + "';})();";
                    return ResourceHandler.FromStream(new MemoryStream(Encoding.UTF8.GetBytes(webClient.DownloadString(request.Url) + ";" + custom_color + dark_theme + custom_interface + mini_player)), webClient.ResponseHeaders["Content-Type"]);
                }
            }
            return null;
        }
        public bool HasHandlers
        {
            get
            {
                return true;
            }
        }
    }
}
