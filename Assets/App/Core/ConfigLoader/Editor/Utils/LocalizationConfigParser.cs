using System.Collections.Generic;
using System.IO;
using App.Common.Tools;
using App.Core.UI.LocalizationsCustomService;
using Newtonsoft.Json;

namespace App.Core.ConfigLoader
{
    public static class LocalizationConfigParser
    {
        private struct LanguageDataForParse
        {
            public string UID;
            public string EN;
            public string RU;
        }

        private static Dictionary<string, LanguageData> configData = new Dictionary<string, LanguageData>();

        public static void ParseAndSaveData(string csvData)
        {
            var parsedData = CSVParser.Deserialize<LanguageDataForParse>(csvData);

            List<string> contentForCodeGen = new List<string>();
            
            foreach (var parsedLine in parsedData)
            {
                configData.Add(parsedLine.UID, new LanguageData()
                {
                    EN = parsedLine.EN,
                    RU = parsedLine.RU,
                });
                
                contentForCodeGen.Add(CodeGenerator.GetFieldForGenerator<string>(parsedLine.UID, parsedLine.UID));
            }

            var jsonData = JsonConvert.SerializeObject(configData, Formatting.Indented);
            CodeGenerator.Generate(LocalizationsCustom.Scripts.TextLocalizationUidsPath, contentForCodeGen);
            
            configData.Clear();
            File.WriteAllText(LocalizationsCustom.Configs.LocalizationConfigPath, jsonData);
            Logger.Log($"[LocalizationConfigParser] => ParseAndSaveData: LocalizationConfig was updated");
        }
    }
}