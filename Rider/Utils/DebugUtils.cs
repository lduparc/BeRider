using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rider.Utils
{
    public static class DebugUtils
    {
        public static void Log(string pageTag, string text)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                  System.Diagnostics.Debug.WriteLine("{0} - {1}", pageTag, text);
            }
        }
    }
}
