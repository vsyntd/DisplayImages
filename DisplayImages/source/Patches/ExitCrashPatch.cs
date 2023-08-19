using DisplayImages.source.UserInterface;
using DuckGame;
using HarmonyLib;
using System;

namespace DisplayImages.source.Patches
{
    
    [HarmonyPatch(typeof(Program), nameof(Program.HandleGameCrash))]
    internal class ExitCrashPatch
    {

        [HarmonyPostfix]
        public static void OnExit()
        {
            try
            {
                DIWindow.DestroyWindow(DIWindow.globalWindowHandle);
                DIWindow.g.Dispose();
            }
            catch (Exception)
            {
            }         
        }
    }

    [HarmonyPatch(typeof(MonoMain), "OnExiting")]
    internal class ExitPatch
    {
        [HarmonyPostfix]
        public static void OnExit()
        {
            try
            {
                DIWindow.DestroyWindow(DIWindow.globalWindowHandle);
                DIWindow.g.Dispose();
            }
            catch (Exception)
            {
            }
        }
    }
}
