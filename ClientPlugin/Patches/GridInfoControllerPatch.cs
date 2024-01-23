using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ClientPlugin.Patches
{
    [HarmonyPatch("Sandbox.Game.Gui.MyTerminalInfoController", "ServerLimitInfo_Received")]
    public static class GridInfoControllerPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var gridBuiltByIdInfo = codes[182].operand;
            var GridBuiltByIdInfo_GridName = codes[225].operand;
            var endOfLoop = codes[733].labels[0];

            var instructionsToInsert = new List<CodeInstruction>();

            instructionsToInsert.Insert(0, new CodeInstruction(OpCodes.Ldloc_S, 11));
            instructionsToInsert.Insert(1, new CodeInstruction(OpCodes.Ldfld, GridBuiltByIdInfo_GridName));
            instructionsToInsert.Insert(2, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GridInfoControllerPatch), nameof(GridInfoControllerPatch.ShouldHideGrid))));
            instructionsToInsert.Insert(3, new CodeInstruction(OpCodes.Brtrue, endOfLoop));

            codes.InsertRange(183, instructionsToInsert);
            return codes.AsEnumerable();
        }

        public static bool ShouldHideGrid(string gridName)
        {
            string[] subStrings = GridInfoMenuPatch.SearchBoxText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (subStrings.All(s => gridName.Contains(s, StringComparison.OrdinalIgnoreCase)) == false)
            {
                return true;
            }
            return false;
        }
    }
}
