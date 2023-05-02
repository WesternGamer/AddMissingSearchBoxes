using EmptyKeys.UserInterface.Controls;
using HarmonyLib;
using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VRage.Utils;

namespace ClientPlugin.Patches
{
    [HarmonyPatch("Sandbox.Game.Gui.MyTerminalInfoController", "ServerLimitInfo_Received")]
    public static class GridInfoControllerPatch
    {
        private struct GridBuiltByIdInfo_AddMissingSearchBoxes
        {
            public string GridName;

            public long EntityId;

            public int PCUBuilt;

            public int BlockCount;

            public List<string> UnsafeBlocks;
        }


        /*private static unsafe bool Prefix(ref List<object> gridsWithBuiltById)
        {
            foreach (object grid in gridsWithBuiltById)
            {
                object obj = grid;

                GridBuiltByIdInfo_AddMissingSearchBoxes s2 = *(GridBuiltByIdInfo_AddMissingSearchBoxes*)&obj;

                GridBuiltByIdInfo_AddMissingSearchBoxes safe = s2;

                //string safestring = safe.GridName;

                string[] subStrings = GridInfoMenuPatch.SearchBoxText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                MyLog.Default.WriteLine(safe.GridName);

                if (subStrings.All(s => safe.GridName.Contains(s, StringComparison.OrdinalIgnoreCase)) == false)
                {
                    gridsWithBuiltById.Remove(grid);
                }
            }
            return true;
        }*/

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var gridBuiltByIdInfo = codes[178].operand;
            var GridBuiltByIdInfo_GridName = codes[220].operand;
            var endOfLoop = codes[710].labels[0];
            var String = codes[220].operand;


            var instructionsToInsert = new List<CodeInstruction>();

            instructionsToInsert.Insert(0, new CodeInstruction(OpCodes.Ldloc_S, 11));
            instructionsToInsert.Insert(1, new CodeInstruction(OpCodes.Ldfld, GridBuiltByIdInfo_GridName));
            //instructionsToInsert.Insert(2, new CodeInstruction(OpCodes.Ldloc_S, 15));
            instructionsToInsert.Insert(2, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GridInfoControllerPatch), nameof(GridInfoControllerPatch.ShouldHideGrid))));
            instructionsToInsert.Insert(3, new CodeInstruction(OpCodes.Brtrue, endOfLoop));

            codes.InsertRange(184, instructionsToInsert);
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
