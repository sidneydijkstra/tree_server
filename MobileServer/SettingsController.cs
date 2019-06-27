using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct Setting_Info {
    public string name;
    public string ip;
    public int port;
    public string auth_key;
}

public static class SettingsController{
    private static string _filePath = Environment.CurrentDirectory;
    private static string _settingFilePath = Environment.CurrentDirectory + "server_settings.set";

    public static Setting_Info info;

    public static void load() {
        string json = "";
        if (!File.Exists(_settingFilePath)) {
            using (FileStream file = File.Create(_settingFilePath)) {
                json = JSONWriter.ToJson(new Setting_Info() { name = "Tree Of Live", ip = "192.168.1.3", port = 11000, auth_key = "AABBCCDD11223344" });
                Byte[] data = new UTF8Encoding(true).GetBytes(json);
                file.Write(data, 0 , data.Length);
            }
        }else{
            json = "";
            using (StreamReader sr = File.OpenText(_settingFilePath)){
                json = sr.ReadLine();
            }
        }
        info = JSONParser.FromJson<Setting_Info>(json);
        Console.WriteLine(string.Format("[JSON] info file: {0}", json));
        return;
    }

    private static void save() {
        if (File.Exists(_settingFilePath)) {
            File.Delete(_settingFilePath);
        }

        using (FileStream file = File.Create(_settingFilePath)) {
            string json = JSONWriter.ToJson(info);
            Byte[] data = new UTF8Encoding(true).GetBytes(json);
            file.Write(data, 0 , data.Length);
        }
        
        Console.WriteLine(string.Format("[JSON] saved files"));
    }

    public static string getSyncInfo() {
        return "SYNCSET;info;" + JSONWriter.ToJson(info);
    }
    public static void resyncInfo(string _json) {
        Console.WriteLine(string.Format("[JSON] resync info: {0}", _json));
        info = JSONParser.FromJson<Setting_Info>(_json);
        save();
    }

}

