using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using System.Reflection;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin.Patches
{
    [HarmonyPatch]
    internal static class SpawnMenuPatch
    { 
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method("Sandbox.Game.Gui.MyGuiScreenDebugSpawnMenu:CreateMenu");
        }

        private static void Postfix(MyGuiScreenDebugBase __instance)
        {
            int m_selectedScreen = (int)AccessTools.Field("Sandbox.Game.Gui.MyGuiScreenDebugSpawnMenu:m_selectedScreen").GetValue(__instance);

            switch (m_selectedScreen)
            {
                case 0:
                    {
                        MyGuiControlListbox m_physicalObjectListbox = (MyGuiControlListbox)AccessTools.Field("Sandbox.Game.Gui.MyGuiScreenDebugSpawnMenu:m_physicalObjectListbox").GetValue(__instance);

                        MyGuiControlSearchBox searchBox = new MyGuiControlSearchBox
                        {
                            OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                            Position = m_physicalObjectListbox.Position + new Vector2(0, -0.04f)
                        };
                        searchBox.Size = new Vector2(m_physicalObjectListbox.Size.X, searchBox.Size.Y);
                        searchBox.OnTextChanged += ItemSearchbox_TextChanged;

                        __instance.Controls.Add(searchBox);

                        break;
                    }
                case 1:
                    {
                        break;
                    }
                case 3:
                    {
                        break;
                    }
            }
        }

        private static void ItemSearchbox_TextChanged(string newText)
        {

        }
    }
}
