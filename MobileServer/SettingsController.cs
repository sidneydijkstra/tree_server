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

public struct Setting_Tree {
    public Setting_RGB normal;
    public Setting_RGB blink;
}

public struct Setting_RGB {
    public int r;
    public int g;
    public int b;

    public void format(string _format) { // r,g,b
        string[] formatData = _format.Split(',');
        r = int.Parse(formatData[0]);
        g = int.Parse(formatData[1]);
        b = int.Parse(formatData[2]);
    }

    public string formatToString() {
        return r + "," + g + "," + b;
    }
}

public enum SettingDeviceCommandType {
    SET = 0,
    BLINK = 1
}

public struct Setting_Device_Commmand {
    public SettingDeviceCommandType type;
    public string deviceName;
    public string id;
}


public static class SettingsController{
    private static string _filePath = Environment.CurrentDirectory;
    private static string _settingServerFilePath = Environment.CurrentDirectory + "/server_settings.set";
    private static string _settingTreeFilePath = Environment.CurrentDirectory + "/tree_settings.set";
    private static string _settingCommandFilePath = Environment.CurrentDirectory + "/command_settings.set";

    public static Setting_Info info;
    public static Setting_Tree tree;
    public static Setting_Device_Commmand[] commands;

    public static void load() {
        string json = "";
        if (!File.Exists(_settingServerFilePath)) {
            using (FileStream file = File.Create(_settingServerFilePath)) {
                json = JSONWriter.ToJson(new Setting_Info() { name = "Tree Of Live", ip = "192.168.1.3", port = 11000, auth_key = "AABBCCDD11223344" });
                Byte[] data = new UTF8Encoding(true).GetBytes(json);
                file.Write(data, 0 , data.Length);
            }
        }else{
            json = "";
            using (StreamReader sr = File.OpenText(_settingServerFilePath)){
                json = sr.ReadLine();
            }
        }
        info = JSONParser.FromJson<Setting_Info>(json);
        Console.WriteLine(string.Format("[JSON] info file: {0}", json));

        json = "";
        if (!File.Exists(_settingTreeFilePath)) {
            using (FileStream file = File.Create(_settingTreeFilePath)) {
                Setting_RGB _normal = new Setting_RGB() { r = 255, g = 255, b = 255 };
                Setting_RGB _blink = new Setting_RGB() { r = 123, g = 123, b = 123 };
                json = JSONWriter.ToJson(new Setting_Tree() { normal=_normal, blink=_blink});
                Byte[] data = new UTF8Encoding(true).GetBytes(json);
                file.Write(data, 0 , data.Length);
            }
        }else{
            json = "";
            using (StreamReader sr = File.OpenText(_settingTreeFilePath)){
                json = sr.ReadLine();
            }
        }
        tree = JSONParser.FromJson<Setting_Tree>(json);
        Console.WriteLine(string.Format("[JSON] info file: {0}", json));

        json = "";
        if (!File.Exists(_settingCommandFilePath)) {
            using (FileStream file = File.Create(_settingCommandFilePath)) {
                json = JSONWriter.ToJson(new Setting_Device_Commmand[0]);
                Byte[] data = new UTF8Encoding(true).GetBytes(json);
                file.Write(data, 0 , data.Length);
            }
        }else{
            json = "";
            using (StreamReader sr = File.OpenText(_settingCommandFilePath)){
                json = sr.ReadLine();
            }
        }
        commands = JSONParser.FromJson<Setting_Device_Commmand[]>(json);
        Console.WriteLine(string.Format("[JSON] info file: {0}", json));

        return;
    }

    private static void save() {
        if (File.Exists(_settingServerFilePath)) {
            File.Delete(_settingServerFilePath);
        }

        using (FileStream file = File.Create(_settingServerFilePath)) {
            string json = JSONWriter.ToJson(info);
            Byte[] data = new UTF8Encoding(true).GetBytes(json);
            file.Write(data, 0 , data.Length);
        }
        
        if (File.Exists(_settingTreeFilePath)) {
            File.Delete(_settingTreeFilePath);
        }

        using (FileStream file = File.Create(_settingTreeFilePath)) {
            string json = JSONWriter.ToJson(tree);
            Byte[] data = new UTF8Encoding(true).GetBytes(json);
            file.Write(data, 0 , data.Length);
        }
        
        if (File.Exists(_settingCommandFilePath)) {
            File.Delete(_settingCommandFilePath);
        }

        using (FileStream file = File.Create(_settingCommandFilePath)) {
            string json = JSONWriter.ToJson(commands);
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

    public static string getSyncTree() {
        return "SYNCSET;tree;" + JSONWriter.ToJson(tree);
    }
    public static void resyncTree(string _json) {
        Console.WriteLine(string.Format("[JSON] resync info: {0}", _json));
        tree = JSONParser.FromJson<Setting_Tree>(_json);
        save();
    }
    public static string getSyncCommands() {
        return "SYNCSET;commands;" + JSONWriter.ToJson(commands);
    }
    public static void resyncCommands(string _json) {
        Console.WriteLine(string.Format("[JSON] resync info: {0}", _json));
        commands = JSONParser.FromJson<Setting_Device_Commmand[]>(_json);
        save();
    }
}

