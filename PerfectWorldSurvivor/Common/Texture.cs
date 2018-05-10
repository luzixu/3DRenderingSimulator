using System;
using System.Drawing.Imaging;
using System.Drawing;
using PerfectWorldSurvivor.Model;
using PerfectWorldSurvivor.Utils;

namespace PerfectWorldSurvivor.Common
{
    public class Texture : IDisposable
    {
        public static Texture Create(string filePath)
        {
            if(StringUtils.IsNullOrEmpty(filePath))
            {
                return null;
            }
            try
            {
                Bitmap bitmap = new Bitmap(filePath, true);
                int width = bitmap.Width;
                int height = bitmap.Height;
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                Texture texture = new Texture();
                texture._Set(bitmap,bitmapData, width, height);
                return texture;
            }
            catch (Exception exception)
            {
                Logger.Log(exception.ToString());
            }
            return null;
        }
        public Colorf Map(float u, float v)
        {
            int x = Math.Abs((int)(u * _width % _width));
            int y = Math.Abs((int)(v * _height % _height));
            unsafe
            {
                byte* b = (byte*)_bitmapData.Scan0 + y * _stride + x * 4;
                Colorf color = new Colorf();
                color.b = b[0] * _colorCoeffecient;
                color.g = b[1] * _colorCoeffecient;
                color.r = b[2] * _colorCoeffecient;
                color.a = 1;
                return color;
            }
        }

        public void Dispose()
        {
            try
            {
                _bitmap.UnlockBits(_bitmapData);
                _bitmap.Dispose();
            }
            catch (Exception exception)
            {
                Logger.Log(exception.ToString());
            }
        }
        private void _Set(Bitmap bitmap,BitmapData bitmapData,int width,int height)
        {
            _bitmap = bitmap;
            _bitmapData = bitmapData;
            _width = width;
            _height = height;
            _stride = _bitmapData.Stride;
        }

        private const float  _colorCoeffecient = 1/ 255f;

        private BitmapData _bitmapData;

        private Bitmap _bitmap;

        private int _width;

        private int _height;

        private int _stride;

    }
}
