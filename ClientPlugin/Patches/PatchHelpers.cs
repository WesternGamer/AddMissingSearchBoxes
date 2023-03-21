using System;
using System.Reflection;
using HarmonyLib;
using ClientPlugin.Logging;

namespace ClientPlugin.Patches
{
    // ReSharper disable once UnusedType.Global
    public static class PatchHelpers
    {
        public static Harmony Instance;

        public static bool HarmonyPatchAll(IPluginLogger log, Harmony harmony)
        {
#if DEBUG
            Harmony.DEBUG = true;
#endif
            Instance = harmony;

            log.Debug("Applying Harmony patches");
            try
            {
                Instance.PatchAll(Assembly.GetExecutingAssembly());
                
            }
            catch (Exception ex)
            {
                log.Critical(ex, "Failed to apply Harmony patches");
                return false;
            }

            if (!ApplyManualPatches(log))
            {
                return false;
            }

            return true;
        }

        private static bool ApplyManualPatches(IPluginLogger log)
        {
            try
            {
                Instance.Patch(AccessTools.Method(AccessTools.TypeByName("Sandbox.Game.Gui.MyTerminalFactionController"), "AddFaction"), new HarmonyMethod(typeof(FactionsMenuPatch), nameof(FactionsMenuPatch.Prefix_AddFaction)));
                Instance.Patch(AccessTools.Method(AccessTools.TypeByName("Sandbox.Game.Gui.MyTerminalChatController"), "RefreshPlayerChatHistory"), new HarmonyMethod(typeof(TerminalChatMenuPatch), nameof(TerminalChatMenuPatch.Prefix_RefreshPlayerChatHistory)));
                Instance.Patch(AccessTools.Method(AccessTools.TypeByName("Sandbox.Game.Gui.MyTerminalChatController"), "RefreshFactionChatHistory"), new HarmonyMethod(typeof(TerminalChatMenuPatch), nameof(TerminalChatMenuPatch.Prefix_RefreshFactionChatHistory)));
                Instance.Patch(AccessTools.Method(AccessTools.TypeByName("Sandbox.Game.Gui.MyTerminalChatController"), "RefreshGlobalChatHistory"), new HarmonyMethod(typeof(TerminalChatMenuPatch), nameof(TerminalChatMenuPatch.Prefix_RefreshGlobalChatHistory)));
                Instance.Patch(AccessTools.Method(AccessTools.TypeByName("Sandbox.Game.Gui.MyTerminalChatController"), "RefreshChatBotHistory"), new HarmonyMethod(typeof(TerminalChatMenuPatch), nameof(TerminalChatMenuPatch.Prefix_RefreshChatBotHistory)));
            }
            catch (Exception ex)
            {
                log.Critical(ex, "Failed to apply manual Harmony patches.");
                return false;
            }

            return true;
        }
    }
}