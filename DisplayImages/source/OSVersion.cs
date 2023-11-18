using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DisplayImages.source
{
    internal class OSVersion
    { 
        /// <summary>
        /// <para> passing the address of this struct as a parameter to RtlGetVersion </para>
        /// name of the variables is not important, but the order, in wich they are defined, is
        /// in order for RtlGetVersion to properly assign the values we need
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OS_INFO
        {
            public int OSVersionInfoSize;
            public int majorVer;
        }
        /// <summary>
        /// future proof get windows version using RtlGetVersion
        /// </summary>
        /// <param name="lpVersionInformation"></param>
        /// <returns></returns>
        [DllImport("ntdll.dll", SetLastError = true)]
        static extern int RtlGetVersion(ref OS_INFO lpVersionInformation);

        public static bool Check()
        {
            OS_INFO osVersionInfo = new OS_INFO();
            osVersionInfo.OSVersionInfoSize = Marshal.SizeOf(typeof(OS_INFO));

            int result = RtlGetVersion(ref osVersionInfo);

            if (result == 0)
            {
                if (osVersionInfo.majorVer >= 11)
                    return true;

            }
            else
            {
                DuckGame.DevConsole.Log($"Error: {result}");
            }
            return false;
        }
    }
}
