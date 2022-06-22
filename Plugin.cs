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
        private string _language;
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
            void ChangeLang(string val)
            {
                _language = val;
                _selectLangTxt.gameObject.GetComponent<TextMeshProUGUI>().text = "SELECT LANGUAGE TRANSLATION [" + val.ToUpper() + "]";
                _log.LogInfo($"Language selected : [{val}]");
            }
            
            MenuHandler.CreateText("AutoTranslationPlugin Options", menu, out TextMeshProUGUI _, 60);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            MenuHandler.CreateButton("APPLY SELECTED LANGUAGE (WILL EXIT THE GAME)", menu, () => {
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
            }, 30, true, Color.red);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);
            _selectLangTxt = MenuHandler.CreateText("SELECT LANGUAGE TRANSLATION [" + AutoTranslatorSettings.DestinationLanguage.ToUpper() + "]", menu, out TextMeshProUGUI _, 30);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 30);

            MenuHandler.CreateButton("AFRIKAANS [AF]", menu, () => { ChangeLang("af"); }, 30);
            MenuHandler.CreateButton("ALBANIAN [SQ]", menu, () => { ChangeLang("sq"); }, 30);
            // MenuHandler.CreateButton("AMHARIC [AM]", menu, () => { ChangeLang("am"); }, 30);
            // MenuHandler.CreateButton("ARABIC [AR]", menu, () => { ChangeLang("ar"); }, 30);
            // MenuHandler.CreateButton("ARMENIAN [HY]", menu, () => { ChangeLang("hy"); }, 30);
            // MenuHandler.CreateButton("AZERBAIJANI [AZ]", menu, () => { ChangeLang("az"); }, 30);
            MenuHandler.CreateButton("BASQUE [EU]", menu, () => { ChangeLang("eu"); }, 30);
            MenuHandler.CreateButton("BELARUSIAN [BE]", menu, () => { ChangeLang("be"); }, 30);
            // MenuHandler.CreateButton("BENGALI [BN]", menu, () => { ChangeLang("bn"); }, 30);
            MenuHandler.CreateButton("BOSNIAN [BS]", menu, () => { ChangeLang("bs"); }, 30);
            // MenuHandler.CreateButton("BULGARIAN [BG]", menu, () => { ChangeLang("bg"); }, 30);
            MenuHandler.CreateButton("CATALAN [CA]", menu, () => { ChangeLang("ca"); }, 30);
            MenuHandler.CreateButton("CEBUANO [CEB]", menu, () => { ChangeLang("ceb"); }, 30);
            // MenuHandler.CreateButton("CHINESE [ZH]", menu, () => { ChangeLang("zh"); }, 30);
            MenuHandler.CreateButton("CORSICAN [CO]", menu, () => { ChangeLang("co"); }, 30);
            MenuHandler.CreateButton("CROATIAN [HR]", menu, () => { ChangeLang("hr"); }, 30);
            MenuHandler.CreateButton("CZECH [CS]", menu, () => { ChangeLang("cs"); }, 30);
            MenuHandler.CreateButton("DANISH [DA]", menu, () => { ChangeLang("da"); }, 30);
            MenuHandler.CreateButton("DUTCH [NL]", menu, () => { ChangeLang("nl"); }, 30);
            MenuHandler.CreateButton("ENGLISH [EN]", menu, () => { ChangeLang("en"); }, 30);
            MenuHandler.CreateButton("ESPERANTO [EO]", menu, () => { ChangeLang("eo"); }, 30);
            MenuHandler.CreateButton("ESTONIAN [ET]", menu, () => { ChangeLang("et"); }, 30);
            MenuHandler.CreateButton("FINNISH [FI]", menu, () => { ChangeLang("fi"); }, 30);
            MenuHandler.CreateButton("FRENCH [FR]", menu, () => { ChangeLang("fr"); }, 30);
            // MenuHandler.CreateButton("FRISIAN [FY]", menu, () => { ChangeLang("fy"); }, 30);
            MenuHandler.CreateButton("GALICIAN [GL]", menu, () => { ChangeLang("gl"); }, 30);
            // MenuHandler.CreateButton("GEORGIAN [KA]", menu, () => { ChangeLang("ka"); }, 30);
            MenuHandler.CreateButton("GERMAN [DE]", menu, () => { ChangeLang("de"); }, 30);
            // MenuHandler.CreateButton("GREEK [EL]", menu, () => { ChangeLang("el"); }, 30);
            // MenuHandler.CreateButton("GUJARATI [GU]", menu, () => { ChangeLang("gu"); }, 30);
            MenuHandler.CreateButton("HAITIAN CREOLE [HT]", menu, () => { ChangeLang("ht"); }, 30);
            MenuHandler.CreateButton("HAUSA [HA]", menu, () => { ChangeLang("ha"); }, 30);
            MenuHandler.CreateButton("HAWAIIAN [HAW]", menu, () => { ChangeLang("haw"); }, 30);
            // MenuHandler.CreateButton("HEBREW [IW]", menu, () => { ChangeLang("iw"); }, 30);
            // MenuHandler.CreateButton("HINDI [HI]", menu, () => { ChangeLang("hi"); }, 30);
            MenuHandler.CreateButton("HMONG [HMN]", menu, () => { ChangeLang("hmn"); }, 30);
            MenuHandler.CreateButton("HUNGARIAN [HU]", menu, () => { ChangeLang("hu"); }, 30);
            MenuHandler.CreateButton("ICELANDIC [IS]", menu, () => { ChangeLang("is"); }, 30);
            MenuHandler.CreateButton("IGBO [IG]", menu, () => { ChangeLang("ig"); }, 30);
            MenuHandler.CreateButton("INDONESIAN [ID]", menu, () => { ChangeLang("id"); }, 30);
            MenuHandler.CreateButton("IRISH [GA]", menu, () => { ChangeLang("ga"); }, 30);
            MenuHandler.CreateButton("ITALIAN [IT]", menu, () => { ChangeLang("it"); }, 30);
            // MenuHandler.CreateButton("JAPANESE [JA]", menu, () => { ChangeLang("ja"); }, 30);
            MenuHandler.CreateButton("JAVANESE [JV]", menu, () => { ChangeLang("jv"); }, 30);
            // MenuHandler.CreateButton("KANNADA [KN]", menu, () => { ChangeLang("kn"); }, 30);
            MenuHandler.CreateButton("KAZAKH [KK]", menu, () => { ChangeLang("kk"); }, 30);
            // MenuHandler.CreateButton("KHMER [KM]", menu, () => { ChangeLang("km"); }, 30);
            MenuHandler.CreateButton("KINYARWANDA [RW]", menu, () => { ChangeLang("rw"); }, 30);
            // MenuHandler.CreateButton("KOREAN [KO]", menu, () => { ChangeLang("ko"); }, 30);
            MenuHandler.CreateButton("KURDISH [KU]", menu, () => { ChangeLang("ku"); }, 30);
            // MenuHandler.CreateButton("KYRGYZ [KY]", menu, () => { ChangeLang("ky"); }, 30);
            // MenuHandler.CreateButton("LAO [LO]", menu, () => { ChangeLang("lo"); }, 30);
            MenuHandler.CreateButton("LATIN [LA]", menu, () => { ChangeLang("la"); }, 30);
            MenuHandler.CreateButton("LATVIAN [LV]", menu, () => { ChangeLang("lv"); }, 30);
            MenuHandler.CreateButton("LITHUANIAN [LT]", menu, () => { ChangeLang("lt"); }, 30);
            MenuHandler.CreateButton("LUXEMBOURGISH [LB]", menu, () => { ChangeLang("lb"); }, 30);
            // MenuHandler.CreateButton("MACEDONIAN [MK]", menu, () => { ChangeLang("mk"); }, 30);
            MenuHandler.CreateButton("MALAGASY [MG]", menu, () => { ChangeLang("mg"); }, 30);
            MenuHandler.CreateButton("MALAY [MS]", menu, () => { ChangeLang("ms"); }, 30);
            // MenuHandler.CreateButton("MALAYALAM [ML]", menu, () => { ChangeLang("ml"); }, 30);
            MenuHandler.CreateButton("MALTESE [MT]", menu, () => { ChangeLang("mt"); }, 30);
            // MenuHandler.CreateButton("MAORI [MI]", menu, () => { ChangeLang("mi"); }, 30);
            // MenuHandler.CreateButton("MARATHI [MR]", menu, () => { ChangeLang("mr"); }, 30);
            // MenuHandler.CreateButton("MONGOLIAN [MN]", menu, () => { ChangeLang("mn"); }, 30);
            // MenuHandler.CreateButton("MYANMAR [MY]", menu, () => { ChangeLang("my"); }, 30);
            // MenuHandler.CreateButton("NEPALI [NE]", menu, () => { ChangeLang("ne"); }, 30);
            MenuHandler.CreateButton("NORWEGIAN [NO]", menu, () => { ChangeLang("no"); }, 30);
            // MenuHandler.CreateButton("NYANJA [NY]", menu, () => { ChangeLang("ny"); }, 30);
            // MenuHandler.CreateButton("ODIA [OR]", menu, () => { ChangeLang("or"); }, 30);
            // MenuHandler.CreateButton("PASHTO [PS]", menu, () => { ChangeLang("ps"); }, 30);
            // MenuHandler.CreateButton("PERSIAN [FA]", menu, () => { ChangeLang("fa"); }, 30);
            MenuHandler.CreateButton("POLISH [PL]", menu, () => { ChangeLang("pl"); }, 30);
            MenuHandler.CreateButton("PORTUGUESE [PT]", menu, () => { ChangeLang("pt"); }, 30);
            // MenuHandler.CreateButton("PUNJABI [PA]", menu, () => { ChangeLang("pa"); }, 30);
            MenuHandler.CreateButton("ROMANIAN [RO]", menu, () => { ChangeLang("ro"); }, 30);
            // MenuHandler.CreateButton("RUSSIAN [RU]", menu, () => { ChangeLang("ru"); }, 30);
            MenuHandler.CreateButton("SAMOAN [SM]", menu, () => { ChangeLang("sm"); }, 30);
            MenuHandler.CreateButton("SCOTS GAELIC [GD]", menu, () => { ChangeLang("gd"); }, 30);
            MenuHandler.CreateButton("SERBIAN [SR]", menu, () => { ChangeLang("sr"); }, 30);
            // MenuHandler.CreateButton("SESOTHO [ST]", menu, () => { ChangeLang("st"); }, 30);
            MenuHandler.CreateButton("SHONA [SN]", menu, () => { ChangeLang("sn"); }, 30);
            // MenuHandler.CreateButton("SINDHI [SD]", menu, () => { ChangeLang("sd"); }, 30);
            // MenuHandler.CreateButton("SINHALA [SI]", menu, () => { ChangeLang("si"); }, 30);
            MenuHandler.CreateButton("SLOVAK [SK]", menu, () => { ChangeLang("sk"); }, 30);
            MenuHandler.CreateButton("SLOVENIAN [SL]", menu, () => { ChangeLang("sl"); }, 30);
            MenuHandler.CreateButton("SOMALI [SO]", menu, () => { ChangeLang("so"); }, 30);
            MenuHandler.CreateButton("SPANISH [ES]", menu, () => { ChangeLang("es"); }, 30);
            MenuHandler.CreateButton("SUNDANESE [SU]", menu, () => { ChangeLang("su"); }, 30);
            MenuHandler.CreateButton("SWAHILI [SW]", menu, () => { ChangeLang("sw"); }, 30);
            MenuHandler.CreateButton("SWEDISH [SV]", menu, () => { ChangeLang("sv"); }, 30);
            MenuHandler.CreateButton("TAGALOG [TL]", menu, () => { ChangeLang("tl"); }, 30);
            // MenuHandler.CreateButton("TAJIK [TG]", menu, () => { ChangeLang("tg"); }, 30);
            // MenuHandler.CreateButton("TAMIL [TA]", menu, () => { ChangeLang("ta"); }, 30);
            MenuHandler.CreateButton("TATAR [TT]", menu, () => { ChangeLang("tt"); }, 30);
            // MenuHandler.CreateButton("TELUGU [TE]", menu, () => { ChangeLang("te"); }, 30);
            // MenuHandler.CreateButton("THAI [TH]", menu, () => { ChangeLang("th"); }, 30);
            MenuHandler.CreateButton("TURKISH [TR]", menu, () => { ChangeLang("tr"); }, 30);
            MenuHandler.CreateButton("TURKMEN [TK]", menu, () => { ChangeLang("tk"); }, 30);
            // MenuHandler.CreateButton("UKRAINIAN [UK]", menu, () => { ChangeLang("uk"); }, 30);
            // MenuHandler.CreateButton("URDU [UR]", menu, () => { ChangeLang("ur"); }, 30);
            // MenuHandler.CreateButton("UYGHUR [UG]", menu, () => { ChangeLang("ug"); }, 30);
            MenuHandler.CreateButton("UZBEK [UZ]", menu, () => { ChangeLang("uz"); }, 30);
            MenuHandler.CreateButton("VIETNAMESE [VI]", menu, () => { ChangeLang("vi"); }, 30);
            MenuHandler.CreateButton("WELSH [CY]", menu, () => { ChangeLang("cy"); }, 30);
            MenuHandler.CreateButton("XHOSA [XH]", menu, () => { ChangeLang("xh"); }, 30);
            MenuHandler.CreateButton("YIDDISH [YI]", menu, () => { ChangeLang("yi"); }, 30);
            MenuHandler.CreateButton("YORUBA [YO]", menu, () => { ChangeLang("yo"); }, 30);
            MenuHandler.CreateButton("ZULU [ZU]", menu, () => { ChangeLang("zu"); }, 30);
        }
    }
}
