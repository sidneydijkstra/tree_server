using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct rgb {
    int r;
    int g;
    int b;
}

public static class TreeLightController{
    public static void blink(rgb _rgb, int _amount) { Console.WriteLine("[TREE] blink();"); }
    public static void set(rgb _rgb, bool _fade) { Console.WriteLine("[TREE] set();"); }
    public static void partyStart() { Console.WriteLine("[TREE] partyStart();"); }
    public static void partyStop() { Console.WriteLine("[TREE] partyStop();"); }
}

