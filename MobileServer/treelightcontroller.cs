using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


class TreeReader
{
    SerialReader reader;

    bool partyMode;
    public TreeReader()
    {
        partyMode = false;
        using (FileStream s = new FileStream("./comport.txt", FileMode.OpenOrCreate))
        {
            if (s.Length == 0)
            {
                throw (new Exception("Please add to right com port to comport.txt, example: COM4"));
            }
            byte[] bytes = new byte[6];
            s.Read(bytes, 0, 6);
            string newString = Encoding.ASCII.GetString(bytes);
            reader = new SerialReader(newString, 9600);
        }
    }

    public void set(RGB _rgb, bool _fade)
    {
        if (!partyMode)
            reader.SendMessage("set:" + _rgb.ToString() + ":" + _fade);
    }

    public void blink(RGB _rgb, int _amount)
    {
        if (!partyMode)
            reader.SendMessage("blk:" + _rgb.ToString() + ":" + _amount);
    }

    public void partyStart()
    {
        if (!partyMode)
            reader.SendMessage("prt:star");
    }

    public void partyStop()
    {
        if (partyMode)
            reader.SendMessage("prt:stop");
    }
}

public struct RGB
{
    public int r, g, b;
    public override string ToString()
    {
        return r + ":" + g + ":" + b;
    }
}

class SerialReader
{
    SerialPort p;
    bool running;
    Thread thread;
    string com;
    int baud;

    Queue<string> messagesRecieved;
    Queue<string> messagesToSend;

    public SerialReader(string comPort, int baudrate)
    {
        com = comPort;
        baud = baudrate;
        p.ReadTimeout = 100;
        p.WriteTimeout = 100;
        p = new SerialPort(com, baud);

        running = true;
        thread = new Thread(new ThreadStart(StartRead));
        thread.Start();
    }

    private void StartRead()
    {
        while (running)
        {
            try
            {
                string mess = p.ReadLine();
                messagesRecieved.Enqueue(mess);
            }
            catch (Exception e)
            {
                if (!(e is TimeoutException))
                {
                    Console.WriteLine(e);
                }
            }

            bool shouldDequeu = true;
            while (messagesToSend.Count > 0)
                try
                {
                    string toSend = messagesToSend.Peek();
                    p.WriteLine(toSend);
                }
                catch (Exception e)
                {
                    shouldDequeu = false;
                }
                finally
                {
                    if (shouldDequeu)
                        messagesToSend.Dequeue();
                }

        }
    }

    public string LastMessage()
    {
        return (messagesRecieved.Count > 0) ? messagesRecieved.Dequeue() : "";
    }

    public void SendMessage(string s)
    {
        messagesToSend.Enqueue(s);
    }
}

public static class TreeLightController
{
    private static TreeReader reader;

    public static void blink(RGB _rgb, int _amount) { if (reader == null) reader = new TreeReader(); reader.blink(_rgb, _amount); }
    public static void set(RGB _rgb, bool _fade) { if (reader == null) reader = new TreeReader(); reader.set(_rgb, _fade); }
    public static void partyStart() { if (reader == null) reader = new TreeReader(); reader.partyStart(); }
    public static void partyStop() { if (reader == null) reader = new TreeReader(); reader.partyStop(); }
}

