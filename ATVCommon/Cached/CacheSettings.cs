using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.Web.Caching;

namespace ATVCommon.Cached
{
    public class CacheSettings
    {
        public static CacheSettings GetCurrentSettings()
        {
            string cacheName = "CommonConfiguration_CacheSettings";
            if (null != HttpContext.Current.Cache[cacheName])
            {
                try
                {
                    return (CacheSettings)HttpContext.Current.Cache[cacheName];
                }
                catch
                {
                    return new CacheSettings();
                }
            }
            else
            {
                try
                {
                    string configFilePath = HttpContext.Current.Server.MapPath("/Config/CacheSettings.config"); //@"D:\Running projects\VC Corporation\Dantri\Dantri.Cached\CacheSettings.config";
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(configFilePath);

                    CacheSettings settings = new CacheSettings();

                    XmlNode nodeFileSettingCacheExpire = xmlDoc.DocumentElement.SelectSingleNode("//Configuration/CacheSettingsFile");
                    settings.FileSettingCacheExpire = Lib.Object2Long(nodeFileSettingCacheExpire.Attributes["cacheExpire"].Value);
                    if (settings.FileSettingCacheExpire <= 0)
                    {
                        settings.FileSettingCacheExpire = 3600;// default 1h
                    }

                    XmlNode nodeEnableCache = xmlDoc.DocumentElement.SelectSingleNode("//Configuration/Cache");
                    settings.EnableCache = Lib.Object2Boolean(nodeEnableCache.Attributes["enable"].Value);

                    List<PageSetting> pageSettings = new List<PageSetting>();

                    XmlNodeList pages = xmlDoc.DocumentElement.SelectNodes("//Pages/Page");

                    for (int i = 0; i < pages.Count; i++)
                    {
                        PageSetting pageSetting = new PageSetting();
                        pageSetting.CacheName = pages[i].Attributes["name"].Value;
                        pageSetting.FilePath = pages[i].Attributes["filePath"].Value;
                        pageSetting.UpdateCacheByWeb = Lib.Object2Boolean(pages[i].Attributes["updateCacheByWeb"].Value);
                        pageSetting.CacheExpire = Lib.Object2Long(pages[i].Attributes["cacheExpire"].Value);
                        pageSetting.EnableCache = Lib.Object2Boolean(pages[i].Attributes["enableCache"].Value);
                        pageSetting.EnableViewState = Lib.Object2Boolean(pages[i].Attributes["enableViewState"].Value);

                        List<ControlSetting> controlSettings = new List<ControlSetting>();

                        XmlNodeList controls = pages[i].SelectNodes("Control");
                        for (int j = 0; j < controls.Count; j++)
                        {
                            ControlSetting controlSetting = new ControlSetting();
                            controlSetting.Assembly = controls[j].Attributes["assembly"].Value;
                            controlSetting.ContainerID = controls[j].Attributes["containerID"].Value;
                            controlSetting.FilePath = controls[j].Attributes["filePath"].Value;
                            controlSetting.CacheExpire = Lib.Object2Long(controls[j].Attributes["cacheExpire"].Value);
                            controlSetting.EnableCache = Lib.Object2Boolean(controls[j].Attributes["enableCache"].Value);
                            controlSetting.EnableViewState = Lib.Object2Boolean(controls[j].Attributes["enableViewState"].Value);
                            controlSetting.UpdateCacheByWeb = Lib.Object2Boolean(controls[j].Attributes["updateCacheByWeb"].Value);

                            controlSettings.Add(controlSetting);
                        }

                        pageSetting.ControlSettings = controlSettings.ToArray();

                        pageSettings.Add(pageSetting);
                    }

                    settings.PageSettings = pageSettings.ToArray();

                    CacheDependency fileDependency = new CacheDependency(configFilePath);
                    HttpContext.Current.Cache.Insert(cacheName, settings, fileDependency, DateTime.Now.AddSeconds(settings.FileSettingCacheExpire), TimeSpan.Zero, CacheItemPriority.Normal, null);

                    return settings;
                }
                catch (Exception ex)
                {
                    return new CacheSettings();
                }
            }
        }

        public PageSetting GetPageSetting(string filePath)
        {
            string url = (filePath.IndexOf("?") > 0 ? filePath.Substring(0, filePath.IndexOf("?")) : filePath);
            url = url.ToLower();
            url = url.Replace("//", "/");
            for (int i = 0; i < this.m_PageSettings.Length; i++)
            {
                PageSetting setting = this.m_PageSettings[i];
                if (string.Compare(setting.FilePath, url, true) == 0)
                {
                    return setting;
                }
            }
            return new PageSetting();
        }

        #region Properties
        private PageSetting[] m_PageSettings;
        public PageSetting[] PageSettings
        {
            set { this.m_PageSettings = value; }
            get { return this.m_PageSettings; }
        }

        private long m_FileSettingCacheExpire;
        public long FileSettingCacheExpire
        {
            set { this.m_FileSettingCacheExpire = value; }
            get { return this.m_FileSettingCacheExpire; }
        }

        private bool m_EnableCache;
        public bool EnableCache
        {
            set { this.m_EnableCache = value; }
            get { return this.m_EnableCache; }
        }
        #endregion

        #region struct
        public struct PageSetting
        {
            public PageSetting(string cacheName, string filePath, long cacheExpire, bool enableCache, bool enableViewState, bool updateCacheByWeb, params ControlSetting[] controlSettings)
            {
                this.CacheName = cacheName;
                this.FilePath = filePath;
                this.CacheExpire = cacheExpire;
                this.EnableCache = enableCache;
                this.EnableViewState = enableViewState;
                this.ControlSettings = controlSettings;
                this.UpdateCacheByWeb = updateCacheByWeb;
            }
            public string CacheName, FilePath;
            public long CacheExpire;
            public bool EnableCache;
            public bool EnableViewState;
            public bool UpdateCacheByWeb;
            public ControlSetting[] ControlSettings;
        }

        public struct ControlSetting
        {
            public ControlSetting(string assembly, string containerID, string filePath, long cacheExpire, bool enableCache, bool enableViewSate, bool updateCacheByWeb)
            {
                this.Assembly = assembly;
                this.ContainerID = containerID;
                this.FilePath = filePath;
                this.CacheExpire = cacheExpire;
                this.EnableCache = enableCache;
                this.EnableViewState = enableViewSate;
                this.UpdateCacheByWeb = updateCacheByWeb;
            }
            public string Assembly, ContainerID, FilePath;
            public long CacheExpire;
            public bool EnableCache;
            public bool UpdateCacheByWeb;
            public bool EnableViewState;
        }
        #endregion
    }
}
