using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class NetworkTree{
    private static string AUTH_KEY = "AABBCCDD11223344";

    private TcpListener _listener;

    private List<NetworkDevice> _enabledConnections;
    private List<TcpConnection> _disabledConnections;
    private List<TcpConnection> _userConnections;

    public NetworkTree() {
        _enabledConnections = new List<NetworkDevice>();
        _disabledConnections = new List<TcpConnection>();
        _userConnections = new List<TcpConnection>();

        _listener = new TcpListener("127.0.0.1", 11000);
        _listener.listen(_onConnection);
    }

    private void _onConnection(Socket _socket) {
        TcpConnection connection = new TcpConnection(_socket);
        connection.OnRecieveTcpPackage += (string _data) => {
            string[] formatData = _data.Split(';');
            Console.WriteLine(string.Format("[SERVER] ({1})recieved data: {0}", _data, formatData.Length));
            if (formatData[0] == "AUTHDEV") {
                if (formatData[1] == AUTH_KEY) { // auth key correct ?? init device
                    connection.send("INIT");
                    _enabledConnections.Add(new NetworkDevice(connection, formatData[2], formatData[3]));
                    _disabledConnections.Remove(connection);
                } else {
                    Console.WriteLine(string.Format("[SERVER] device connection got stoped, auth_key : {0}", formatData[1]));
                }
            } else if (formatData[0] == "USERJOIN") {
                _userConnections.Add(connection);
                _disabledConnections.Remove(connection);
                Console.WriteLine(string.Format("[SERVER] new user connection", formatData[1]));
                connection.send("INIT");
            }
        };
        connection.OnConnectionClosed += () => {
            Console.WriteLine("[SERVER] connection--");
        };
        _disabledConnections.Add(connection);
    }
}

