using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using DuckGame;

namespace DisplayImages
{
    [HarmonyPatch(typeof(HatSelector), "AllTeams")]
    internal class HatSelectorPatched
    {
        [HarmonyPostfix]
        public static void PatchAllTeams(ref List<Team> __result)  // does not show image Hats in Lobby selector
        {
            List<Team> teams2 = new List<Team>(Teams.core.teams);
            foreach (Team t2 in Teams.core.extraTeams)
            {
                if (t2.name.Contains(AssembleImages.indicatorForPatcher))
                    continue;

                teams2.Add(t2);
            }
            __result = teams2;
        }
    }

}
