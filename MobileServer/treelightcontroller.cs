using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class TreeLightController{
    public static void blink(Setting_RGB _rgb, int _amount){
        NetworkTree.treeConnection.send("BLK;" + _rgb.formatToString());
    }
    public static void set(Setting_RGB _rgb, bool _fade) {
        NetworkTree.treeConnection.send("SET;" + _rgb.formatToString());
    }

    public static void party() {
        NetworkTree.treeConnection.send("PAR");
    }
}

