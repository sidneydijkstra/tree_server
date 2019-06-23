using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum commandParam
{
    INT = 0, FLOAT = 1, BOOL = 2, STRING = 3
}

public class NetworkCommand{

    public string name;
    public commandParam[] param;

    public NetworkCommand(string _name, params commandParam[] _param) {
        name = _name;
        param = _param;
    }
}

