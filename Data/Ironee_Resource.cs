using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using UnityEngine;

// Author: Ironee

namespace com.IronicEntertainment.Scripts.Data
{
    public class Ironee_Resource
    {
        [Serializable]
        public struct IR_Parameters
        {
            public string name, variant;
            public bool allNew, isVariant;

            public IR_Parameters(string pfilename = "", bool pisVariant = false, string pvariantname = "", bool pallnew = true)
            {
                name = pfilename;
                variant = pvariantname;
                allNew = pallnew;
                isVariant = pisVariant;
            }
            public IR_Parameters(string pfilename = "", bool pallnew = true)
            {
                string[] split = pfilename.Split('_');
                name = split[0];
                variant = "";
                allNew = pallnew;
                isVariant = split.Length >= 2;

                if (isVariant)
                {
                    for (int i = 1; i < split.Length; i++)
                    {
                        variant += split[i] + "_";
                    }
                    variant = variant.Remove(variant.Length - 1);
                }
            }
        }
        private string _ParentResourcePath = "JSON/"; // Ensure trailing slash consistency
        private string _ProjectDirectory { get; set; }

        private string _FilePath;

        private bool _Imported= false;


        private string _Name = "";
        private string _Variant = "";
        public string Name { get => _Name + _Variant; 
            set
            {
                string[] split = value.Split('_');
                string name = "", variant = "";

                int min = 3;

                if( split.Length < min) min = split.Length;

                for (int i = 0; i < min; i++)
                {
                    name += split[i] + '_';
                }

                if(split.Length <= 3) name = name.Remove(name.Length - 1);
                else
                {
                    for (int i = 3; i < split.Length; i++)
                    {
                        variant += split[i] + "_";
                    }
                    if(variant.Length>0)variant = variant.Remove(variant.Length - 1);
                }

                _Variant = variant;
                _Name = name;

                

                m_PropertiesG[nameof(Name)] = Name;

                Export();
            } }



        protected Dictionary<string,object> m_PropertiesG = new Dictionary<string, object>();

        private void Set(string filename = "", string variantname = "")
        {

            PropertyInfo[] lProperties = GetType().GetProperties();

            foreach (PropertyInfo p in lProperties)
            {
                m_PropertiesG.Add(p.Name, p.GetValue(this));
            }

            _Name = filename;
            _Variant = variantname;
        }

        public Ironee_Resource(IR_Parameters pParams)
        {            
            Set(pParams.name, pParams.variant);

            string path = $"{_ProjectDirectory}{_ParentResourcePath}";

            CreateDirectory(path);

            path = CreateResourceDirectory(path);

            if (pParams.isVariant)
            {
                CreateResourceFileVariant(path, pParams.allNew, pParams.variant);
            }
            else CreateResourceFile(path);
        }
        public Ironee_Resource(string filename = "", bool isVariant = false, string variantname = "", bool allnew =true)
        {
            Set(filename,variantname);

            string path = $"{_ProjectDirectory}{_ParentResourcePath}";

            CreateDirectory(path);

            path = CreateResourceDirectory(path);

            if (isVariant)
            {
                CreateResourceFileVariant(path, allnew, variantname);
            }
            else CreateResourceFile(path);
        }

        private void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    
                    Debug.Log($"Failed to create directory: {e.Message}");
                    // Handle error gracefully
                }
            }

        }

        private string CreateResourceDirectory(string basePath)
        {
            string path = System.IO.Path.Combine(basePath, GetType().Name + "/");

            if (!System.IO.Directory.Exists(path))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    Debug.Log($"Failed to create resource directory: {e.Message}");
                    // Handle error gracefully
                }
            }

            return path;
        }

        private void CreateResourceFile(string basePath)
        {
            string fileName = _Name;
            string initial = "";
            string[] lWords = GetType().Name.Split('_');
            int max = CountFilesInDirectory(basePath);
            if(max < 0) max = 0;

            if (Name == "")
                fileName = $"{max + 1}";

            foreach (string word in lWords) { initial += word[0]; }

            Name = fileName = "R_" + initial + "_" + fileName;

            fileName += ".json";


            _FilePath = System.IO.Path.Combine(basePath, fileName);

            if (!System.IO.File.Exists(_FilePath))
            {
                try
                {
                    // Create and write JSON content to the file
                    using (StreamWriter writer = new StreamWriter(_FilePath))
                    {
                        string jsonContent = $"[\n{ToJSON()}\n]";
                        writer.Write(jsonContent);
                    }
                    Debug.Log("Resource file created successfully.");
                }
                catch (Exception e)
                {
                    Debug.Log($"Failed to create resource file: {e.Message}");
                    // Handle error gracefully
                }
            }

            Import();
        }

        private void CreateResourceFileVariant(string basePath, bool allnew, string variantname= "")
        {
            string fileName = _Name;
            string initial = "";
            string[] lWords = GetType().Name.Split('_');

            int max = CountFilesInDirectory(basePath); 
            if (max < 0) max = 0;

            if (Name == "")
                fileName = $"{max + 1}";

            foreach (string word in lWords) { initial += word[0]; }

            Name = fileName = "R_" + initial + "_" + fileName;




            _FilePath = System.IO.Path.Combine(basePath, fileName + ".json");

            if (!System.IO.File.Exists(_FilePath))
            {
                try
                {
                    string jsonContent;
                    // Create and write JSON content to the file
                    using (StreamWriter writer = new StreamWriter(_FilePath))
                    {
                        jsonContent = $"[\n{ToJSON()}\n]";

                        writer.Write(jsonContent);

                    }

                    Dictionary<string, object>[] jsonObject = JsonUtility.FromJson<Dictionary<string, object>[]>(jsonContent);

                    using (StreamWriter writer = new StreamWriter(_FilePath)) { 
                        jsonContent = jsonContent.Remove(jsonContent.Length - 2);
                        _Variant = "_" + variantname;

                        if (_Variant == "_") _Variant = "_" + jsonObject.Length;

                        Name = Name;


                        jsonContent += $",\n{ToJSON()}\n]";
                        writer.Write(jsonContent);
                    }

                    Debug.Log("Resource file created successfully.");
                }
                catch (Exception e)
                {
                    Debug.Log($"Failed to create resource file: {e.Message}");
                }
            }
            else
            {

                string jsonText = System.IO.File.ReadAllText(_FilePath);

                Dictionary<string, object>[] jsonObject = JsonUtility.FromJson<Dictionary<string, object>[]>(jsonText);
                _Variant = "_" + variantname;

                if (_Variant == "_") _Variant = "_" + jsonObject.Length;

                Name = Name;

                bool exist = false;
                int num = 0;
                string nametocheck;
                string[] ntcSplit;

                foreach (Dictionary<string, object> lDic in jsonObject)
                {
                    nametocheck = lDic[nameof(Name)].ToString();
                    ntcSplit = nametocheck.Split('_');

                    if (ntcSplit.Length <= 3) nametocheck = "";

                    else
                    {
                        nametocheck = "";
                        for (int i = 3; i < ntcSplit.Length; i++)
                        {
                            nametocheck += ntcSplit[i] + "_";
                        }
                        nametocheck = nametocheck.Remove(nametocheck.Length - 1);
                    }
                    if (nametocheck == _Variant)
                    {
                        num++;
                        exist = true;
                        continue;
                    }
                }

                if (allnew)
                {
                    if(exist)Name = Name + "_" + num;
                    string jsonContent = System.IO.File.ReadAllText(_FilePath);

                    using (StreamWriter writer = new StreamWriter(_FilePath))
                    {

                        jsonContent = jsonContent.Remove(jsonContent.Length - 2);

                        jsonContent += $",\n{ToJSON()}\n]";

                        writer.Write(jsonContent);
                    }
                }
                else if (!exist)
                {
                    string jsonContent = System.IO.File.ReadAllText(_FilePath);
                    using (StreamWriter writer = new StreamWriter(_FilePath))
                    {
                        jsonContent = jsonContent.Remove(jsonContent.Length - 2);

                        jsonContent += $",\n{ToJSON()}\n]";

                        writer.Write(jsonContent);
                    }
                }
            }


            Import();
        }

        private int CountFilesInDirectory(string directoryPath)
        {
            string[] files = System.IO.Directory.GetFiles(directoryPath);
            return files.Length;
        }

        public string ToJSON(bool indented = true, Dictionary<string, object> jsonObject = null)
        {
            if (jsonObject == null) return JsonUtility.ToJson(m_PropertiesG, indented);
            else return JsonUtility.ToJson(jsonObject, indented);
        }


        public void Import()
        {
            try
            {
                string jsonText = System.IO.File.ReadAllText(_FilePath);

                Dictionary<string, object>[] jsonObject = JsonUtility.FromJson<Dictionary<string, object>[]>(jsonText);

                foreach (Dictionary<string,object> lDic in jsonObject)
                {
                    if (lDic[nameof(Name)].ToString() != Name) continue;

                    foreach (KeyValuePair<string,object> lKV in lDic)
                    {
                        if (m_PropertiesG.ContainsKey(lKV.Key))
                        {
                            // Get the PropertyInfo of the property
                            PropertyInfo property = GetType().GetProperty(lKV.Key);

                            // Handle arrays, lists, and dictionaries
                            if (property.PropertyType.IsArray)
                            {
                                // Convert the value to an array and set the property
                                Array convertedArray = JsonUtility.FromJson<Array>(lKV.Value.ToString());
                                property.SetValue(this, convertedArray);
                            }
                            else if (property.PropertyType.IsGenericType &&
                                     property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                // Convert the value to a list and set the property
                                Type listType = property.PropertyType.GetGenericArguments()[0];
                                object convertedList = JsonUtility.FromJson(lKV.Value.ToString(), typeof(List<>).MakeGenericType(listType));
                                property.SetValue(this, convertedList);
                            }
                            else if (property.PropertyType.IsGenericType &&
                                     property.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                            {
                                // Convert the value to a dictionary and set the property
                                Type[] dictionaryTypes = property.PropertyType.GetGenericArguments();
                                Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(dictionaryTypes);
                                object convertedDictionary = JsonUtility.FromJson(lKV.Value.ToString(), dictionaryType);
                                property.SetValue(this, convertedDictionary);
                            }
                            else
                            {
                                // Convert the value to the type of the property and set the property

                                if(property.PropertyType == typeof(string))
                                    property.SetValue(this, lKV.Value.ToString());
                                else if (property.PropertyType == typeof(char))
                                    property.SetValue(this, lKV.Value.ToString().ToCharArray()[0]);
                                else property.SetValue(this, JsonUtility.FromJson(lKV.Value.ToString(), property.PropertyType));
                            }
                        }
                        else
                        {
                            Debug.Log($"Property '{lKV.Key}' not found in the class.");
                        }
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("An error occurred during import: " + ex.Message);
            }

            _Imported = true;

        }

        public void Export()
        {
            if (!_Imported) return;
            try
            {
                string jsonText = System.IO.File.ReadAllText(_FilePath);

                Dictionary<string, object>[] jsonObject = JsonUtility.FromJson<Dictionary<string, object>[]>(jsonText);
                for (int i = 0; i < jsonObject.Length; i++)
                {
                    if (jsonObject[i][nameof(Name)].ToString() != Name) continue;
                    jsonObject[i] = m_PropertiesG;
                }

                using (StreamWriter writer = new StreamWriter(_FilePath))
                {

                    jsonText = "[";

                    foreach (Dictionary<string, object> lObj in jsonObject)
                    {
                        jsonText += $"\n{ToJSON(true, lObj)},";
                    }
                    jsonText = jsonText.Remove(jsonText.Length - 1) + "\n]";

                    writer.Write(jsonText);
                }
            }
            catch (Exception ex)
            {
                Debug.Log("An error occurred during export: " + ex.Message);
            }
        }

    }
}
