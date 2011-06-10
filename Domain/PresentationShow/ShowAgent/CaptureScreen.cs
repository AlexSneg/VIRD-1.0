using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using TechnicalServices.Interfaces;

namespace Domain.PresentationShow.ShowAgent
{
    public class CaptureScreen
    {
        // P/Invoke declarations
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int
        wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, CopyPixelOperation rop);
        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteDC(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteObject(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr ptr);


        private const int ScreeBufferSize = 512 * 1024;

        public static MemoryStream GetScreenShort(IEventLogging log, Guid imageFormat)
        {
            MemoryStream result = null;
            try
            {
                result = new MemoryStream(ScreeBufferSize);
                Size sz = Screen.PrimaryScreen.Bounds.Size;
                IntPtr hDesk = GetDesktopWindow();
                IntPtr hSrce = GetWindowDC(hDesk);
                IntPtr hDest = CreateCompatibleDC(hSrce);
                IntPtr hBmp = CreateCompatibleBitmap(hSrce, sz.Width, sz.Height);
                IntPtr hOldBmp = SelectObject(hDest, hBmp);
                bool b = BitBlt(hDest, 0, 0, sz.Width, sz.Height, hSrce, 0, 0, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);
                using (Image image = Image.FromHbitmap(hBmp))
                {
                    SelectObject(hDest, hOldBmp);
                    DeleteObject(hBmp);
                    DeleteDC(hDest);
                    ReleaseDC(hDesk, hSrce);
                    image.Save(result, new ImageFormat(imageFormat));
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex.Message);
            }
            return result;
        }
    }
}
