#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

#endregion

namespace ATVCommon.Cached.UrlRewrite
{
    /// <summary>
    /// Parses and serves dynamic stylesheets.
    /// </summary>
    public class CssHandler : IHttpHandler
    {

        #region IHttpHandler implementation

        public void ProcessRequest(HttpContext context)
        {
            //FileInfo file = new FileInfo(context.Request.PhysicalPath);

            //SetDefaultVariables();
            ////ParseVariables(file.FullName);
            //ApplyVariables(context);
            //ReduceSize(file, context);
            //SetHeadersAndCache(file.FullName, context);
            //_Variables.Clear();

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Private members

        private Dictionary<string, string> _Variables = new Dictionary<string, string>();
        private StringBuilder _CleanedCSS = new StringBuilder();
        private StringBuilder _ParsedCSS = new StringBuilder();

        #endregion

        #region Methods

        /// <summary>
        /// Adds the built-in variables to the collection.
        /// </summary>
        private void SetDefaultVariables()
        {
            _Variables.Add("browser", "\"" + HttpContext.Current.Request.Browser.Browser + "\"");
            _Variables.Add("version", HttpContext.Current.Request.Browser.MajorVersion.ToString());
        }

        /// <summary>
        /// Parses the variables defined in the stylesheet
        /// and adds them to the variable collection.
        /// </summary>
        private void ParseVariables(string file)
        {
            try
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    while (reader.Peek() > -1)
                    {
                        string line = reader.ReadLine();
                        if (line.StartsWith("define "))
                        {
                            line = line.Replace("define ", string.Empty);
                            int index = line.IndexOf("=") + 1;
                            string key = line.Substring(0, index - 1).Trim();
                            string value = line.Substring(index, line.Length - index).Replace(";", string.Empty).Trim();

                            foreach (string var in _Variables.Keys)
                            {
                                if (value.Contains(var))
                                    value = value.Replace(var, _Variables[var]);
                            }

                            _Variables.Add(key, value);
                        }
                        else
                        {
                            _CleanedCSS.AppendLine(line);
                        }
                    }
                }
            }
            catch (DriveNotFoundException ex) { }
        }

        /// <summary>
        /// Applies the defined variables to the stylesheet.
        /// </summary>
        private void ApplyVariables(HttpContext context)
        {
            string css = _CleanedCSS.ToString();
            foreach (string variable in _Variables.Keys)
            {
                css = css.Replace(variable, _Variables[variable]);
            }

            _ParsedCSS.Append(css);
        }

        /// <summary>
        /// A simple function to get the result of a C# expression
        /// </summary>
        /// <param name="command">String value containing an expression that can evaluate to a string.</param>
        /// <returns>A string value after evaluating the command string.</returns>


        /// <summary>
        /// Removes all unwanted text from the CSS file,
        /// including comments and whitespace.
        /// </summary>
        private void ReduceSize(FileInfo file, HttpContext context)
        {
            string contenttype = string.Empty;

            switch (file.Extension.ToLower())
            {
                case ".js":
                    context.Response.ContentType = "text/javascript";
                    RenderFile(context);
                    break;
                case ".css":
                    context.Response.ContentType = "text/css";
                    RenderFile(context);
                    break;
                case ".jpg":
                    context.Response.ContentType = "image/jpeg";
                    context.Response.WriteFile(file.FullName);
                    break;
                case ".bmp":
                    context.Response.ContentType = "image/bmp";
                    context.Response.WriteFile(file.FullName);
                    break;
                case ".gif":
                    context.Response.ContentType = "image/gif";
                    context.Response.WriteFile(file.FullName);
                    break;
                case ".png":
                    context.Response.ContentType = "image/png";
                    context.Response.WriteFile(file.FullName);
                    break;
                default: break;
                
            }
        }

        private void RenderFile(HttpContext context)
        {
            string css = _ParsedCSS.ToString();
            css = css.Replace("  ", String.Empty);
            //css = css.Replace(Environment.NewLine, String.Empty);
            css = css.Replace("\t", string.Empty);
            css = css.Replace(" {", "{");
            css = css.Replace(" :", ":");
            css = css.Replace(": ", ":");
            css = css.Replace(", ", ",");
            css = css.Replace("; ", ";");
            css = css.Replace(";}", "}");
            css = Regex.Replace(css, @"/\*[^\*]*\*+([^/\*]*\*+)*/", "$1");
            css = Regex.Replace(css, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&nbsp;)\s{2,}(?=[<])", String.Empty);
            context.Response.Write(css);
            //return css;
        }

        /// <summary>
        /// This will make the browser and server keep the output
        /// in its cache and thereby improve performance.
        /// </summary>
        private void SetHeadersAndCache(string file, HttpContext context)
        {
            //string ext = file.Substring(file.LastIndexOf(".")).ToString().ToLower();

            //TimeSpan maxAge = new TimeSpan(3, 0, 0, 0);

            //context.Response.Cache.SetMaxAge(maxAge);

            //context.Response.AddFileDependency(file);
            //context.Response.Cache.SetCacheability(HttpCacheability.Public);

            //context.Response.Cache.VaryByParams["path"] = true;
            //context.Response.Cache.SetETagFromFileDependencies();
            //context.Response.Cache.SetExpires(DateTime.Now.AddDays(3));
            //context.Response.Expires = 86400000;
            //context.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            //context.Response.Cache.SetETag(string.Empty);
            //context.Response.Cache.SetLastModifiedFromFileDependencies();

            HttpResponse response = context.Response;

            TimeSpan duration = TimeSpan.FromDays(3);

            HttpCachePolicy cache = response.Cache;

            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.Add(duration));
            cache.SetMaxAge(duration);
            cache.AppendCacheExtension("must-revalidate, proxy-revalidate");

            FieldInfo maxAgeField = cache.GetType().GetField("_maxAge", BindingFlags.Instance | BindingFlags.NonPublic);
            maxAgeField.SetValue(cache, duration);
        }

        #endregion

    }

}
