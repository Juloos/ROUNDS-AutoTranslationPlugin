using System.IO;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using TMPro;
using UnboundLib;
using UnboundLib.Utils.UI;
using HarmonyLib;
using XUnity.AutoTranslator.Plugin.Core;

namespace AutoTranslationPlugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("gravydevsupreme.xunity.autotranslator", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInProcess("Rounds.exe")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private string Language;
        private GameObject selectLangTxt;
        private string ATPconfigPath = Path.Combine(BepInEx.Paths.ConfigPath, "AutoTranslatorConfig.ini");
        private void Awake()
        {
            Log = base.Logger;

            foreach (string file in Directory.GetFiles(BepInEx.Paths.PluginPath, "*.atplang", SearchOption.AllDirectories))
            {
                string langtxt = Path.GetFileNameWithoutExtension(file);
                string translationFile = Path.Combine(BepInEx.Paths.BepInExRootPath, "Translation", langtxt, "Text", "_Translations.txt");
                if (!Directory.Exists(Path.Combine(BepInEx.Paths.BepInExRootPath, "Translation", langtxt, "Text")))
                {
                    Directory.CreateDirectory(Path.Combine(BepInEx.Paths.BepInExRootPath, "Translation", langtxt, "Text"));
                } 
                else if (File.Exists(translationFile))
                {
                    File.Delete(translationFile);
                }
                File.Copy(file, translationFile);
            }

            // apply patches
            new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();
        }
        private void Start()
        {
            Unbound.RegisterCredits("AutoTranslationPlugin", new string[] { "Juloos" }, new string[] {}, new string[] {});
            Unbound.RegisterCredits("XUnity.AutoTranslator", new string[] { "Bepis" }, new string[] {"github"}, new string[] {"https://github.com/bbepis/XUnity.AutoTranslator"});

            Unbound.RegisterMenu("AutoTranslationPlugin", () => {}, this.NewGUI, null, false);
        }
        private void NewGUI(GameObject menu)
        {

            MenuHandler.CreateText("AutoTranslationPlugin Options", menu, out TextMeshProUGUI _, 60);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            void ChangeLang(string val)
            {
                Language = val;
                selectLangTxt.gameObject.GetComponent<TextMeshProUGUI>().text = "SELECT LANGUAGE TRANSLATION [" + val.ToUpper() + "]";
                Log.LogInfo($"Language changed to '{val}'");
            }
            void ApplyLang()
            {
                if (AutoTranslatorSettings.DestinationLanguage != Language)
                {
                    string text = File.ReadAllText(ATPconfigPath);
                    text = text.Replace("\nLanguage=" + AutoTranslatorSettings.DestinationLanguage, "\nLanguage=" + Language);
                    text = text.Replace("FromLanguage=ja", "FromLanguage=en");
                    text = text.Replace("EnableIMGUI=False", "EnableIMGUI=True");
                    text = text.Replace("EnableTextMesh=False", "EnableTextMesh=True");
                    text = text.Replace("MaxCharactersPerTranslation=200", "MaxCharactersPerTranslation=2500");
                    File.WriteAllText(ATPconfigPath, text);
                    Log.LogInfo($"QUITTING");
                    
                    // Shut the game down to apply the new language
                    Application.Quit(0);
                }
            }
            selectLangTxt = MenuHandler.CreateText("SELECT LANGUAGE TRANSLATION [" + AutoTranslatorSettings.DestinationLanguage.ToUpper() + "]", menu, out TextMeshProUGUI _, 30);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            MenuHandler.CreateButton("ENGLISH [EN]", menu, () => { ChangeLang("en"); }, 30);
            MenuHandler.CreateButton("FRANCAIS [FR]", menu, () => { ChangeLang("fr"); }, 30);
            MenuHandler.CreateButton("ESPAÑOL [ES]", menu, () => { ChangeLang("es"); }, 30);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            MenuHandler.CreateButton("APPLY SELECTED LANGUAGE (WILL EXIT THE GAME)", menu, () => { ApplyLang(); }, 30, true, Color.red);
        }
    }
}
