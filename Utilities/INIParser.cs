#region
using System;
using System.Collections;
using System.IO;
using System.Linq;

#endregion

namespace SFMLStart.Utilities
{
    public class INIParser
    {
        private readonly String _iniFilePath;
        private readonly Hashtable _keyPairs = new Hashtable();

        public INIParser(String iniPath)
        {
            TextReader iniFile = null;
            String currentRoot = null;

            _iniFilePath = iniPath;

            if (!File.Exists(iniPath)) Utils.Log("Unable to locate " + iniPath);
            try
            {
                iniFile = new StreamReader(iniPath);

                var strLine = iniFile.ReadLine();

                while (strLine != null)
                {
                    strLine = strLine.Trim().ToUpper();

                    if (strLine != "")
                    {
                        if (strLine.StartsWith("[") &&
                            strLine.EndsWith("]")) currentRoot = strLine.Substring(1, strLine.Length - 2);
                        else
                        {
                            var keyPair = strLine.Split(new[] {'='}, 2);

                            SectionPair sectionPair;
                            String value = null;

                            if (currentRoot == null) currentRoot = "ROOT";

                            sectionPair.Section = currentRoot;
                            sectionPair.Key = keyPair[0];

                            if (keyPair.Length > 1) value = keyPair[1];

                            if (value != null) _keyPairs.Add(sectionPair, value);
                        }
                    }

                    strLine = iniFile.ReadLine();
                }
            }
            finally
            {
                if (iniFile != null) iniFile.Close();
            }
        }

        public String GetSetting(String sectionName, String settingName)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName.ToUpper();
            sectionPair.Key = settingName.ToUpper();

            return (String) _keyPairs[sectionPair];
        }

        public String[] EnumSection(String sectionName)
        {
            var tmpArray = new ArrayList();

            foreach (var pair in
                _keyPairs.Keys.Cast<SectionPair>().Where(pair => pair.Section == sectionName.ToUpper()))
                tmpArray.Add(pair.Key);

            return (String[]) tmpArray.ToArray(typeof (String));
        }

        public void AddSetting(String sectionName, String settingName, String settingValue = null)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName.ToUpper();
            sectionPair.Key = settingName.ToUpper();

            if (_keyPairs.ContainsKey(sectionPair)) _keyPairs.Remove(sectionPair);

            _keyPairs.Add(sectionPair, settingValue);
        }

        public void DeleteSetting(String sectionName, String settingName)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName.ToUpper();
            sectionPair.Key = settingName.ToUpper();

            if (_keyPairs.ContainsKey(sectionPair)) _keyPairs.Remove(sectionPair);
        }

        public void SaveSettings(String newFilePath)
        {
            var sections = new ArrayList();
            var strToSave = "";

            foreach (var sectionPair in
                _keyPairs.Keys.Cast<SectionPair>().Where(sectionPair => !sections.Contains(sectionPair.Section)))
                sections.Add(sectionPair.Section);

            foreach (String section in sections)
            {
                strToSave += ("[" + section + "]\r\n");

                foreach (SectionPair sectionPair in _keyPairs.Keys)
                {
                    if (sectionPair.Section != section) continue;
                    var tmpValue = (String) _keyPairs[sectionPair];

                    if (tmpValue != null) tmpValue = "=" + tmpValue;

                    strToSave += (sectionPair.Key + tmpValue + "\r\n");
                }

                strToSave += "\r\n";
            }

            TextWriter tw = new StreamWriter(newFilePath);
            tw.Write(strToSave);
            tw.Close();
        }

        public void SaveSettings() { SaveSettings(_iniFilePath); }

        #region Nested type: SectionPair
        private struct SectionPair
        {
            public String Key;
            public String Section;
        }
        #endregion
    }
}