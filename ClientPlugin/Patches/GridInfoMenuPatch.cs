using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin.Patches
{
    [HarmonyPatch(typeof(MyGuiScreenTerminal), "CreateInfoPageControls")]
    internal static class GridInfoMenuPatch
    {
        private static MyGuiScreenTerminal terminalInstance = null;

        public static string SearchBoxText = "";

        private static void Postfix(MyGuiScreenTerminal __instance, MyGuiControlTabPage infoPage)
        {
            terminalInstance = __instance;

            if (MyGuiScreenTerminal.InteractedEntity != null)
            {
                return;
            }

            MyGuiControlSearchBox searchBox = new MyGuiControlSearchBox
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                Position = new Vector2(-0.452f, -0.332f),
            };
            searchBox.Size = new Vector2(0.29f, searchBox.Size.Y);
            searchBox.OnTextChanged += SearchBox_OnTextChanged;

            infoPage.Controls.Add(searchBox);
        }

        private static void SearchBox_OnTextChanged(string newText)
        {
            SearchBoxText = newText;

            AccessTools.Method(AccessTools.TypeByName("Sandbox.Game.Gui.MyTerminalInfoController"), "RequestServerLimitInfo").Invoke(null, null);
        }
    }
}
