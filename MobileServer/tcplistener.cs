using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

public class TcpListener{
    private Thread _listenThread;

    public IPAddress ip;
    public int port;

    private IPEndPoint _localEndPoint;
    private Socket _listener;

    private int _backlog;

    public TcpListener(string _ip, int _port, int _backlog = 10) {
        ip = IPAddress.Parse(_ip);
        port = _port;

        this._backlog = _backlog;

        _localEndPoint = new IPEndPoint(ip, port);
        _listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    }

    public void listen(Action<Socket> _onConnection) {
        _listenThread = new Thread(new ParameterizedThreadStart(_listen));
        _listenThread.Start(_onConnection);
    }

    private void _listen(object _onConnection) {
        Action<Socket> onConnection = (Action<Socket>)_onConnection;
        try{
            _listener.Bind(_localEndPoint);
            _listener.Listen(_backlog);
            Console.WriteLine(string.Format("[SERVER] starting to listen on ip {0} and port {1}", ip.ToString(), port));

            byte[] bytes = new byte[1024];
            while (true){
                Socket handler = _listener.Accept();
                onConnection?.Invoke(handler);
                Console.WriteLine(string.Format("[SERVER] connection++"));
                continue;

                int bytesRec = handler.Receive(bytes);
                string data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                 
                byte[] msg = Encoding.ASCII.GetBytes("message");
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
        catch (Exception e){
            Console.WriteLine(string.Format("[SERVER] connection failed: {0}", e.ToString()));
        }
    }

    public void stop() {
        _listenThread.Abort();
        // TODO stop listening
    }
}

