namespace Web
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Web;

    public class GetThumbNail : IHttpHandler
    {
        private string savedFolder = "ThumbImages";

        private void BindData(HttpContext context, string imagePath, string width)
        {
            string str5;
            string[] nameOfFileThumb = this.GetNameOfFileThumb(imagePath, width);
            string str = context.Server.MapPath("/" + this.savedFolder);
            string hTTPImages = this.GetHTTPImages(nameOfFileThumb[0]);
            if (!File.Exists(context.Server.MapPath("/" + this.savedFolder + "/" + hTTPImages)))
            {
                Bitmap bmp = null;
                if (imagePath.IndexOf("http://") != -1)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imagePath);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    bmp = new Bitmap(response.GetResponseStream());
                }
                else
                {
                    try
                    {
                        bmp = new Bitmap(context.Server.MapPath(imagePath));
                    }
                    catch
                    {
                        context.Response.Redirect("/NoImages/No.jpg", true);
                        return;
                    }
                }
                Directory.CreateDirectory(str + "/" + nameOfFileThumb[1]);
                string strFileThumb = str + "/" + hTTPImages;
                this.Resize(bmp, strFileThumb, Convert.ToInt32(width));
                str5 = "/" + this.savedFolder + "/" + hTTPImages.TrimStart(new char[] { '/' });
                HttpContext.Current.Cache.Insert(imagePath, str5, null, DateTime.Now.AddDays(3.0), TimeSpan.Zero);
                context.Response.Redirect(str5, true);
            }
            else
            {
                str5 = "/" + this.savedFolder + "/" + hTTPImages.TrimStart(new char[] { '/' });
                HttpContext.Current.Cache.Insert(imagePath, str5, null, DateTime.Now.AddDays(3.0), TimeSpan.Zero);
                context.Response.Redirect(str5, true);
            }
        }

        private static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
        {
            int num10;
            int width = imgPhoto.Width;
            int height = imgPhoto.Height;
            int x = 0;
            int y = 0;
            int num5 = 0;
            int num6 = 0;
            float num7 = 0f;
            float num8 = 0f;
            float num9 = 0f;
            num8 = ((float)Width) / ((float)width);
            num9 = ((float)Height) / ((float)height);
            if (num9 >= num8)
            {
                num7 = num9;
                switch (Anchor)
                {
                    case AnchorPosition.Left:
                        num5 = 0;
                        goto Label_00D1;

                    case AnchorPosition.Right:
                        num5 = Width - ((int)(width * num7));
                        goto Label_00D1;
                }
                num5 = (int)((Width - (width * num7)) / 2f);
            }
            else
            {
                num7 = num8;
                switch (Anchor)
                {
                    case AnchorPosition.Top:
                        num6 = 0;
                        goto Label_00D1;

                    case AnchorPosition.Bottom:
                        num6 = Height - ((int)(height * num7));
                        goto Label_00D1;
                }
                num6 = (int)((Height - (height * num7)) / 2f);
            }
        Label_00D1:
            num10 = (int)(width * num7);
            int num11 = (int)(height * num7);
            Bitmap image = new Bitmap(Width - 1, Height - 1, PixelFormat.Format24bppRgb);
            image.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.White);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(imgPhoto, new Rectangle(num5, num6, num10, num11), new Rectangle(x, y, width, height - 2), GraphicsUnit.Pixel);
            graphics.Dispose();
            return image;
        }

        private string GetHTTPImages(string str)
        {
            if ((str != null) && (str.ToLower().IndexOf("http://") != -1))
            {
                str = str.Substring(8);
                string[] strArray = str.Split(new char[] { '/' });
                str = string.Join("/", strArray, 1, strArray.Length - 1);
            }
            return str;
        }

        public string[] GetNameOfFileThumb(string imagePath, string width)
        {
            imagePath = HttpUtility.UrlDecode(imagePath);
            string[] strArray = new string[2];
            string str = imagePath.Substring(imagePath.LastIndexOf('/') + 1);
            string[] strArray2 = new string[2];
            strArray2[1] = str.Substring(str.LastIndexOf(".") + 1);
            strArray2[0] = str.Substring(0, str.LastIndexOf("."));
            string str2 = strArray2[0] + "_" + width + "." + strArray2[1];
            string str3 = "";
            imagePath = this.GetHTTPImages(imagePath);
            str3 = imagePath.Replace("/" + str, "");
            string str4 = "/" + str3.TrimStart(new char[] { '/' }) + "/" + str2;
            strArray[0] = str4;
            strArray[1] = str3;
            return strArray;
        }

        public void ProcessRequest(HttpContext context)
        {
            string imagePath = context.Request["ImgFilePath"];
            string width = context.Request["width"];
            this.BindData(context, imagePath, width);
            if (HttpContext.Current.Cache[imagePath] != null)
            {
                HttpContext.Current.Response.Redirect(HttpContext.Current.Cache[imagePath].ToString(), false);
            }
        }

        public string Resize(Bitmap bmp, string strFileThumb, int P_Width)
        {
            string str2 = string.Empty;
            try
            {
                float num = ((float)bmp.Width) / ((float)bmp.Height);
                int num2 = 0;
                switch (((HttpContext.Current.Request.QueryString["iz"] != null) ? Convert.ToInt32(HttpContext.Current.Request.QueryString["iz"]) : 0))
                {
                    case 0:
                        num2 = Convert.ToInt32((double)(0.75 * P_Width));
                        break;

                    case 1:
                        num2 = Convert.ToInt32((float)(0.5625f * P_Width));
                        break;

                    default:
                        num2 = Convert.ToInt32((float)(P_Width /num ));
                        break;
                }
                using (Bitmap bitmap = new Bitmap(Crop(bmp, P_Width + 1, num2 + 1, AnchorPosition.Center)))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.High;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        ImageCodecInfo encoder = ImageCodecInfo.GetImageEncoders()[1];
                        EncoderParameters encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 0x5cL);
                        graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                        bitmap.Save(strFileThumb, encoder, encoderParams);
                    }
                    return str2;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str2;
        }

        public bool ThumbCallback()
        {
            return false;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }
    }
}

