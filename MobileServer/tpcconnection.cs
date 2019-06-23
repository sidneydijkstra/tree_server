using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class TcpConnection{
    private Thread _listenThread;

    public Action<string> OnRecieveTcpPackage;
    public Action OnConnectionClosed;

    private Socket _socket;

    public TcpConnection(Socket _socket) {
        this._socket = _socket;

        _listenThread = new Thread(new ThreadStart(_listen));
        _listenThread.Start();
    }

    public void send(string _data) {
        Console.WriteLine(string.Format("[SERVER] send data: {0}", _data));
        byte[] msg = Encoding.ASCII.GetBytes(_data);
        _socket.Send(msg);
    }

    private void _listen() {
        byte[] bytes = new byte[1024];
        while (true){
            try{
                int bytesRec = _socket.Receive(bytes);
                string data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.Length > 5)
                    OnRecieveTcpPackage?.Invoke(data);
            }
            catch (Exception){
                _socket.Close();
                _socket = null;
                OnConnectionClosed?.Invoke();
                break;
            }
        }
    }

    //TODO julia fixen

    public void stop() {
        _listenThread.Abort();
        _socket.Close();
        _listenThread = null;
        _socket = null;
    }
}

