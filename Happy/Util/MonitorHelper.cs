using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

    public static class MonitorHelper
    {
    [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
    public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

    // Token: 0x060000DB RID: 219 RVA: 0x00007FF4 File Offset: 0x000061F4
    public static double GetWindowsScreenScalingFactor(bool percentage = true)
    {
        Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
        IntPtr hdc = graphics.GetHdc();
        int deviceCaps = MonitorHelper.GetDeviceCaps(hdc, 10);
        double num = Math.Round((double)MonitorHelper.GetDeviceCaps(hdc, 117) / (double)deviceCaps, 2);
        if (percentage)
        {
            num *= 100.0;
        }
        graphics.ReleaseHdc(hdc);
        graphics.Dispose();
        return num;
    }
    public static dynamic MonitorSize()
    {
        object result;
        try
        {
            double windowsScreenScalingFactor = MonitorHelper.GetWindowsScreenScalingFactor(false);
            int num = (int)((double)Screen.PrimaryScreen.Bounds.Width * windowsScreenScalingFactor);
            double num2 = (double)Screen.PrimaryScreen.Bounds.Height * windowsScreenScalingFactor;
            result = new Size(num, (int)num2);
        }
        catch
        {
            result = Screen.PrimaryScreen.Bounds.Size;
        }
        return result;
    }
    private static byte[] ImageToByte(Image img)
    {
        byte[] result;
        try
        {
            if (img == null)
            {
                result = null;
            }
            else
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    img.Save(memoryStream, ImageFormat.Png);
                    result = memoryStream.ToArray();
                }
            }
        }
        catch (Exception)
        {
            result = null;
        }
        return result;
    }

    // Token: 0x02000034 RID: 52
    public enum DeviceCap
    {
        // Token: 0x0400002E RID: 46
        VERTRES = 10,
        // Token: 0x0400002F RID: 47
        DESKTOPVERTRES = 117
    }
}
