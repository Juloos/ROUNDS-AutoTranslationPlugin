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
        private static ManualLogSource _log;
        private string _language = AutoTranslatorSettings.DestinationLanguage;
        private GameObject _selectLangTxt;
        private readonly string _atpConfigPath = Path.Combine(BepInEx.Paths.ConfigPath, "AutoTranslatorConfig.ini");

        private void Awake()
        {
            _log = base.Logger;

            foreach (string file in Directory.GetFiles(BepInEx.Paths.PluginPath, "*.atplang", SearchOption.AllDirectories))
            {
                string langtxt = Path.GetFileNameWithoutExtension(file);
                string translationFile = Path.Combine(BepInEx.Paths.BepInExRootPath, "Translation", langtxt, "Text", "_Translations.txt");
                if (!Directory.Exists(Path.Combine(BepInEx.Paths.BepInExRootPath, "Translation", langtxt, "Text")))
                {
                    Directory.CreateDirectory(Path.Combine(BepInEx.Paths.BepInExRootPath, "Translation", langtxt,
                        "Text"));
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
            Unbound.RegisterCredits("AutoTranslationPlugin", new string[] { "Juloos" }, new string[] { "github" }, new string[] { "https://github.com/Juloos/ROUNDS-AutoTranslationPlugin" });
            Unbound.RegisterCredits("XUnity.AutoTranslator", new string[] { "Bepis" }, new string[] { "github" }, new string[] { "https://github.com/bbepis/XUnity.AutoTranslator" });

            Unbound.RegisterMenu("AutoTranslationPlugin", () => { }, this.NewGUI, null, false);
        }

        private void NewGUI(GameObject menu)
        {
            MenuHandler.CreateText("AutoTranslationPlugin Options", menu, out TextMeshProUGUI _, 60);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            GameObject langSelectMenu = MenuHandler.CreateMenu("SELECT LANGUAGE", () => { }, menu, 50, true, false, menu.transform.parent.gameObject);
            SelectionGUI(langSelectMenu);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            _selectLangTxt = MenuHandler.CreateButton($"APPLY SELECTED LANGUAGE [{_language}]", menu, () =>
            {
                if (AutoTranslatorSettings.DestinationLanguage != _language)
                {
                    string text = File.ReadAllText(_atpConfigPath);
                    text = text.Replace("\nLanguage=" + AutoTranslatorSettings.DestinationLanguage, "\nLanguage=" + _language);
                    text = text.Replace("FromLanguage=ja", "FromLanguage=en");
                    text = text.Replace("EnableIMGUI=False", "EnableIMGUI=True");
                    text = text.Replace("EnableTextMesh=False", "EnableTextMesh=True");
                    text = text.Replace("MaxCharactersPerTranslation=200", "MaxCharactersPerTranslation=2500");
                    File.WriteAllText(_atpConfigPath, text);
                    _log.LogInfo($"QUITTING");

                    // Shut the game down to apply the new language
                    Application.Quit(0);
                }
            }, 50, true, Color.red);
        }

        private void SelectionGUI(GameObject submenu)
        {
            void ChangeLang(string val)
            {
                _language = val;
                _selectLangTxt.GetComponentInChildren<TextMeshProUGUI>().text = $"APPLY SELECTED LANGUAGE [{val.ToUpper()}]";
                _log.LogInfo($"Language selected : [{val}]");
            }
            
            MenuHandler.CreateButton("AFRIKAANS [AF]", submenu, () => { ChangeLang("af"); }, 30);
            MenuHandler.CreateButton("ALBANIAN [SQ]", submenu, () => { ChangeLang("sq"); }, 30);
            // MenuHandler.CreateButton("AMHARIC [AM]", submenu, () => { ChangeLang("am"); }, 30);
            // MenuHandler.CreateButton("ARABIC [AR]", submenu, () => { ChangeLang("ar"); }, 30);
            // MenuHandler.CreateButton("ARMENIAN [HY]", submenu, () => { ChangeLang("hy"); }, 30);
            // MenuHandler.CreateButton("AZERBAIJANI [AZ]", submenu, () => { ChangeLang("az"); }, 30);
            MenuHandler.CreateButton("BASQUE [EU]", submenu, () => { ChangeLang("eu"); }, 30);
            MenuHandler.CreateButton("BELARUSIAN [BE]", submenu, () => { ChangeLang("be"); }, 30);
            // MenuHandler.CreateButton("BENGALI [BN]", submenu, () => { ChangeLang("bn"); }, 30);
            MenuHandler.CreateButton("BOSNIAN [BS]", submenu, () => { ChangeLang("bs"); }, 30);
            // MenuHandler.CreateButton("BULGARIAN [BG]", submenu, () => { ChangeLang("bg"); }, 30);
            MenuHandler.CreateButton("CATALAN [CA]", submenu, () => { ChangeLang("ca"); }, 30);
            MenuHandler.CreateButton("CEBUANO [CEB]", submenu, () => { ChangeLang("ceb"); }, 30);
            // MenuHandler.CreateButton("CHINESE [ZH]", submenu, () => { ChangeLang("zh"); }, 30);
            MenuHandler.CreateButton("CORSICAN [CO]", submenu, () => { ChangeLang("co"); }, 30);
            MenuHandler.CreateButton("CROATIAN [HR]", submenu, () => { ChangeLang("hr"); }, 30);
            MenuHandler.CreateButton("CZECH [CS]", submenu, () => { ChangeLang("cs"); }, 30);
            MenuHandler.CreateButton("DANISH [DA]", submenu, () => { ChangeLang("da"); }, 30);
            MenuHandler.CreateButton("DUTCH [NL]", submenu, () => { ChangeLang("nl"); }, 30);
            MenuHandler.CreateButton("ENGLISH [EN]", submenu, () => { ChangeLang("en"); }, 30);
            MenuHandler.CreateButton("ESPERANTO [EO]", submenu, () => { ChangeLang("eo"); }, 30);
            MenuHandler.CreateButton("ESTONIAN [ET]", submenu, () => { ChangeLang("et"); }, 30);
            MenuHandler.CreateButton("FINNISH [FI]", submenu, () => { ChangeLang("fi"); }, 30);
            MenuHandler.CreateButton("FRENCH [FR]", submenu, () => { ChangeLang("fr"); }, 30);
            // MenuHandler.CreateButton("FRISIAN [FY]", submenu, () => { ChangeLang("fy"); }, 30);
            MenuHandler.CreateButton("GALICIAN [GL]", submenu, () => { ChangeLang("gl"); }, 30);
            // MenuHandler.CreateButton("GEORGIAN [KA]", submenu, () => { ChangeLang("ka"); }, 30);
            MenuHandler.CreateButton("GERMAN [DE]", submenu, () => { ChangeLang("de"); }, 30);
            // MenuHandler.CreateButton("GREEK [EL]", submenu, () => { ChangeLang("el"); }, 30);
            // MenuHandler.CreateButton("GUJARATI [GU]", submenu, () => { ChangeLang("gu"); }, 30);
            MenuHandler.CreateButton("HAITIAN CREOLE [HT]", submenu, () => { ChangeLang("ht"); }, 30);
            MenuHandler.CreateButton("HAUSA [HA]", submenu, () => { ChangeLang("ha"); }, 30);
            MenuHandler.CreateButton("HAWAIIAN [HAW]", submenu, () => { ChangeLang("haw"); }, 30);
            // MenuHandler.CreateButton("HEBREW [IW]", submenu, () => { ChangeLang("iw"); }, 30);
            // MenuHandler.CreateButton("HINDI [HI]", submenu, () => { ChangeLang("hi"); }, 30);
            MenuHandler.CreateButton("HMONG [HMN]", submenu, () => { ChangeLang("hmn"); }, 30);
            MenuHandler.CreateButton("HUNGARIAN [HU]", submenu, () => { ChangeLang("hu"); }, 30);
            MenuHandler.CreateButton("ICELANDIC [IS]", submenu, () => { ChangeLang("is"); }, 30);
            MenuHandler.CreateButton("IGBO [IG]", submenu, () => { ChangeLang("ig"); }, 30);
            MenuHandler.CreateButton("INDONESIAN [ID]", submenu, () => { ChangeLang("id"); }, 30);
            MenuHandler.CreateButton("IRISH [GA]", submenu, () => { ChangeLang("ga"); }, 30);
            MenuHandler.CreateButton("ITALIAN [IT]", submenu, () => { ChangeLang("it"); }, 30);
            // MenuHandler.CreateButton("JAPANESE [JA]", submenu, () => { ChangeLang("ja"); }, 30);
            MenuHandler.CreateButton("JAVANESE [JV]", submenu, () => { ChangeLang("jv"); }, 30);
            // MenuHandler.CreateButton("KANNADA [KN]", submenu, () => { ChangeLang("kn"); }, 30);
            MenuHandler.CreateButton("KAZAKH [KK]", submenu, () => { ChangeLang("kk"); }, 30);
            // MenuHandler.CreateButton("KHMER [KM]", submenu, () => { ChangeLang("km"); }, 30);
            MenuHandler.CreateButton("KINYARWANDA [RW]", submenu, () => { ChangeLang("rw"); }, 30);
            // MenuHandler.CreateButton("KOREAN [KO]", submenu, () => { ChangeLang("ko"); }, 30);
            MenuHandler.CreateButton("KURDISH [KU]", submenu, () => { ChangeLang("ku"); }, 30);
            // MenuHandler.CreateButton("KYRGYZ [KY]", submenu, () => { ChangeLang("ky"); }, 30);
            // MenuHandler.CreateButton("LAO [LO]", submenu, () => { ChangeLang("lo"); }, 30);
            MenuHandler.CreateButton("LATIN [LA]", submenu, () => { ChangeLang("la"); }, 30);
            MenuHandler.CreateButton("LATVIAN [LV]", submenu, () => { ChangeLang("lv"); }, 30);
            MenuHandler.CreateButton("LITHUANIAN [LT]", submenu, () => { ChangeLang("lt"); }, 30);
            MenuHandler.CreateButton("LUXEMBOURGISH [LB]", submenu, () => { ChangeLang("lb"); }, 30);
            // MenuHandler.CreateButton("MACEDONIAN [MK]", submenu, () => { ChangeLang("mk"); }, 30);
            MenuHandler.CreateButton("MALAGASY [MG]", submenu, () => { ChangeLang("mg"); }, 30);
            MenuHandler.CreateButton("MALAY [MS]", submenu, () => { ChangeLang("ms"); }, 30);
            // MenuHandler.CreateButton("MALAYALAM [ML]", submenu, () => { ChangeLang("ml"); }, 30);
            MenuHandler.CreateButton("MALTESE [MT]", submenu, () => { ChangeLang("mt"); }, 30);
            // MenuHandler.CreateButton("MAORI [MI]", submenu, () => { ChangeLang("mi"); }, 30);
            // MenuHandler.CreateButton("MARATHI [MR]", submenu, () => { ChangeLang("mr"); }, 30);
            // MenuHandler.CreateButton("MONGOLIAN [MN]", submenu, () => { ChangeLang("mn"); }, 30);
            // MenuHandler.CreateButton("MYANMAR [MY]", submenu, () => { ChangeLang("my"); }, 30);
            // MenuHandler.CreateButton("NEPALI [NE]", submenu, () => { ChangeLang("ne"); }, 30);
            MenuHandler.CreateButton("NORWEGIAN [NO]", submenu, () => { ChangeLang("no"); }, 30);
            // MenuHandler.CreateButton("NYANJA [NY]", submenu, () => { ChangeLang("ny"); }, 30);
            // MenuHandler.CreateButton("ODIA [OR]", submenu, () => { ChangeLang("or"); }, 30);
            // MenuHandler.CreateButton("PASHTO [PS]", submenu, () => { ChangeLang("ps"); }, 30);
            // MenuHandler.CreateButton("PERSIAN [FA]", submenu, () => { ChangeLang("fa"); }, 30);
            MenuHandler.CreateButton("POLISH [PL]", submenu, () => { ChangeLang("pl"); }, 30);
            MenuHandler.CreateButton("PORTUGUESE [PT]", submenu, () => { ChangeLang("pt"); }, 30);
            // MenuHandler.CreateButton("PUNJABI [PA]", submenu, () => { ChangeLang("pa"); }, 30);
            MenuHandler.CreateButton("ROMANIAN [RO]", submenu, () => { ChangeLang("ro"); }, 30);
            // MenuHandler.CreateButton("RUSSIAN [RU]", submenu, () => { ChangeLang("ru"); }, 30);
            MenuHandler.CreateButton("SAMOAN [SM]", submenu, () => { ChangeLang("sm"); }, 30);
            MenuHandler.CreateButton("SCOTS GAELIC [GD]", submenu, () => { ChangeLang("gd"); }, 30);
            MenuHandler.CreateButton("SERBIAN [SR]", submenu, () => { ChangeLang("sr"); }, 30);
            // MenuHandler.CreateButton("SESOTHO [ST]", submenu, () => { ChangeLang("st"); }, 30);
            MenuHandler.CreateButton("SHONA [SN]", submenu, () => { ChangeLang("sn"); }, 30);
            // MenuHandler.CreateButton("SINDHI [SD]", submenu, () => { ChangeLang("sd"); }, 30);
            // MenuHandler.CreateButton("SINHALA [SI]", submenu, () => { ChangeLang("si"); }, 30);
            MenuHandler.CreateButton("SLOVAK [SK]", submenu, () => { ChangeLang("sk"); }, 30);
            MenuHandler.CreateButton("SLOVENIAN [SL]", submenu, () => { ChangeLang("sl"); }, 30);
            MenuHandler.CreateButton("SOMALI [SO]", submenu, () => { ChangeLang("so"); }, 30);
            MenuHandler.CreateButton("SPANISH [ES]", submenu, () => { ChangeLang("es"); }, 30);
            MenuHandler.CreateButton("SUNDANESE [SU]", submenu, () => { ChangeLang("su"); }, 30);
            MenuHandler.CreateButton("SWAHILI [SW]", submenu, () => { ChangeLang("sw"); }, 30);
            MenuHandler.CreateButton("SWEDISH [SV]", submenu, () => { ChangeLang("sv"); }, 30);
            MenuHandler.CreateButton("TAGALOG [TL]", submenu, () => { ChangeLang("tl"); }, 30);
            // MenuHandler.CreateButton("TAJIK [TG]", submenu, () => { ChangeLang("tg"); }, 30);
            // MenuHandler.CreateButton("TAMIL [TA]", submenu, () => { ChangeLang("ta"); }, 30);
            MenuHandler.CreateButton("TATAR [TT]", submenu, () => { ChangeLang("tt"); }, 30);
            // MenuHandler.CreateButton("TELUGU [TE]", submenu, () => { ChangeLang("te"); }, 30);
            // MenuHandler.CreateButton("THAI [TH]", submenu, () => { ChangeLang("th"); }, 30);
            MenuHandler.CreateButton("TURKISH [TR]", submenu, () => { ChangeLang("tr"); }, 30);
            MenuHandler.CreateButton("TURKMEN [TK]", submenu, () => { ChangeLang("tk"); }, 30);
            // MenuHandler.CreateButton("UKRAINIAN [UK]", submenu, () => { ChangeLang("uk"); }, 30);
            // MenuHandler.CreateButton("URDU [UR]", submenu, () => { ChangeLang("ur"); }, 30);
            // MenuHandler.CreateButton("UYGHUR [UG]", submenu, () => { ChangeLang("ug"); }, 30);
            MenuHandler.CreateButton("UZBEK [UZ]", submenu, () => { ChangeLang("uz"); }, 30);
            MenuHandler.CreateButton("VIETNAMESE [VI]", submenu, () => { ChangeLang("vi"); }, 30);
            MenuHandler.CreateButton("WELSH [CY]", submenu, () => { ChangeLang("cy"); }, 30);
            MenuHandler.CreateButton("XHOSA [XH]", submenu, () => { ChangeLang("xh"); }, 30);
            MenuHandler.CreateButton("YIDDISH [YI]", submenu, () => { ChangeLang("yi"); }, 30);
            MenuHandler.CreateButton("YORUBA [YO]", submenu, () => { ChangeLang("yo"); }, 30);
            MenuHandler.CreateButton("ZULU [ZU]", submenu, () => { ChangeLang("zu"); }, 30);
        }
    }
}