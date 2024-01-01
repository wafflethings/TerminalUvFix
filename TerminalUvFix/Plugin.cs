using System;
using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace TerminalUvFix
{
    [BepInPlugin(Guid, Name, Version)]
    public class Plugin : BaseUnityPlugin
    {
        private const string Name = "Terminal UV Fix";
        private const string Version = "1.0.0";
        private const string Guid = "waffle.ultrakill.terminaluvfix";
        private static AssetBundle _bundle;
        private static Mesh _fixedTerminalModel;

        private static string _modPath => Path.Combine(Assembly.GetCallingAssembly().Location.Substring(0, Assembly.GetCallingAssembly().Location.LastIndexOf(Path.DirectorySeparatorChar)));
        private static string _bundlePath => Path.Combine(_modPath, "fixed_terminal.bundle");

        private void Start()
        {
            new Harmony(Guid).PatchAll(typeof(Plugin));
            _bundle = AssetBundle.LoadFromFile(_bundlePath);
            _fixedTerminalModel = _bundle.LoadAsset<Mesh>("td_terminal.dae");
        }

        [HarmonyPatch(typeof(ShopZone), nameof(ShopZone.Start)), HarmonyPrefix]
        private static void ChangeModel(ShopZone __instance)
        {
            Transform model = __instance.transform.Find("Cube");
            model.GetComponent<MeshFilter>().mesh = _fixedTerminalModel;
        }
    }
}