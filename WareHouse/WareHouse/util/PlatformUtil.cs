using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.util
{
    public static class PlatformUtil
    {
        static string[] sRVLExts = { ".brres", ".brlyt", ".brlan", ".bdof", ".bfog", ".blight", ".blmap" };
        public enum Platform
        {
            RVL,
            CTR,
            Cafe,
            NX
        }

        public static void SetPlatform(Platform plat)
        {
            sPlatform = plat;
        }

        public static Platform GetPlatform()
        {
            return sPlatform;
        }

        public static string[]? GetFileExtsForCurrentPlatform()
        {
            switch (sPlatform)
            {
                case Platform.RVL:
                    return sRVLExts;
            }

            return null;
        }

        private static Platform sPlatform;
    }
}
