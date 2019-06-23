using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum commandParam{
    INT = 0, FLOAT = 1, BOOL = 2, STRING = 3
}

public class NetworkCommand{

    public string name;
    public commandParam[] param;

    public NetworkCommand(string[] _formatData) {

        // TODO params sepirate whit [,]
        name = _formatData[1];
        string[] formatParams = _formatData[2].Split(',');
        param = new commandParam[formatParams.Length];
        for (int i = 0; i < formatParams.Length; i++){
            param[i - 2] = (commandParam)int.Parse(formatParams[i]);
        }
    }

    public string formatRegCom() {
        string c = "REGCOM;" + name;
        for (int i = 0; i < param.Length; i++){
            c += ";" + ((int)param[i]).ToString();
        }
        return c; 
    }

    public string formatCom(params object[] values) {
        string c = "COM;" + name;
        for (int i = 0; i < param.Length; i++){
            c += ";" + values[i].ToString();
        }
        return c; 
    }

    public string formatRegRet() {
        return "REGRET;" + name + ";" + ((int)param[0]).ToString(); 
    }

    public string formatRet() {
        return "RET;" + name; 
    }
}

