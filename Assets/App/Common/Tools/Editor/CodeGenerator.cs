
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace App.Common.Tools
{
    public static class CodeGenerator
    {
        private const string Marker = "CodGenMarker";

        public static string GetFieldForGenerator<T>(string fieldName, string fieldValue)
        {
            fieldName = fieldName.Replace(' ', '_');
            var oldFieldName = fieldName.Split('.');
            var newFileName = new StringBuilder();

            foreach (var word in oldFieldName)
            {
                var firsChar = Char.ToUpper(word[0]);
                var newWord = $"{firsChar}{word.Remove(0, 1)}";

                newFileName.Append(oldFieldName[0].Equals(word) ? $"{newWord}" : $"_{newWord}");
            }

            var result = $"    public static {typeof(T)} {newFileName} = \"{fieldValue}\";";
            return result;
        }

        public static void Generate(string path, List<string> lines)
        {
            if (File.Exists(path) == false)
            {
                Logger.LogError($"[CodeGenerator] =>Generate: {path} isn't exist");
                return;
            }

            var scriptText = File.ReadLines(path).ToList();
            int lineIndex = 0;

            foreach (var line in scriptText)
            {
                if (line.Contains(Marker))
                {
                    lineIndex = scriptText.IndexOf(line);
                }
            }

            List<string> newScript = new List<string>();

            for (int i = 0; i < scriptText.Count; i++)
            {
                newScript.Add(scriptText[i]);

                if (i == lineIndex)
                {
                    foreach (var line in lines)
                    {
                        newScript.Add(line);
                    }

                    newScript.Add("}");
                    break;
                }
            }
        
            File.WriteAllText(path,"//This script use code generation");
            File.WriteAllLines(path, newScript);
            AssetDatabase.SaveAssets();
        
            Logger.Log($"[CodeGenerator] => Generate: File {path} was updated");
        }
    }
}