using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ASPEC.Utilities
{
    public static class AppRoot
    {

        public static string GetApplicationRoot()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace("ASPEC.exe", "");
        }
    }
}
