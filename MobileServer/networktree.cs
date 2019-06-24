using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class NetworkTree{
    private static int NETWORK_SESSION_ID = 0;
    private static string AUTH_KEY = "AABBCCDD11223344";

    private TcpListener _listener;

    private List<NetworkDevice> _enabledConnections;
    private List<TcpConnection> _disabledConnections;
    private List<NetworkUser> _userConnections;

    public NetworkTree() {
        _enabledConnections = new List<NetworkDevice>();
        _disabledConnections = new List<TcpConnection>();
        _userConnections = new List<NetworkUser>();

        _listener = new TcpListener("192.168.1.3", 11000);
        _listener.listen(_onConnection);
    }

    private void _onConnection(Socket _socket) {
        TcpConnection connection = new TcpConnection(NETWORK_SESSION_ID, _socket);
        NETWORK_SESSION_ID++;
        Console.WriteLine(string.Format("[SERVER] [{0}] connection++", connection.networkId));

        connection.OnRecieveTcpPackage += (string _data) => {
            string[] formatData = _data.Split(';');
            Console.WriteLine(string.Format("[SERVER] UNKNOWN[{0}] recieved data: {1}", connection.networkId, _data));

            if (formatData[0] == "AUTHDEV") {
                if (formatData[1] == AUTH_KEY) { // auth key correct ?? init device
                    this.initDeviceConnection(connection, formatData);
                    Console.WriteLine(string.Format("[SERVER] [{0}] new device connection", connection.networkId));
                } else {
                    Console.WriteLine(string.Format("[SERVER] [{0}] device connection got stoped, auth_key : {1}", connection.networkId, formatData[1]));
                }
            } else if (formatData[0] == "USERJOIN") {
                this.initUserConnection(connection, formatData);
                Console.WriteLine(string.Format("[SERVER] [{0}] new user connection", connection.networkId));
            }
        };
        connection.OnConnectionClosed += () => {
            Console.WriteLine(string.Format("[SERVER] [{0}] connection--", connection.networkId));
        };
        _disabledConnections.Add(connection);
    }

    public void initUserConnection(TcpConnection _conn, string[] _formatData) {
        _disabledConnections.Remove(_conn);
        NetworkUser user = new NetworkUser(_conn);
        _userConnections.Add(user);

        _conn.OnRecieveTcpPackage += (string _data) => {
            string[] formatData = _data.Split(';');
            Console.WriteLine(string.Format("[SERVER] USER[{0}] recieved data: {1}", _conn.networkId, _data));

            if (formatData[0] == "GETDEV") {
                foreach (NetworkDevice device in _enabledConnections){
                    user.updateDevice(device);
                }
            }
        };

        _conn.send("INIT");
    }

    public void initDeviceConnection(TcpConnection _conn, string[] _formatData) {
        _disabledConnections.Remove(_conn);
        NetworkDevice device = new NetworkDevice(_conn, _formatData[2], _formatData[3]);
        _enabledConnections.Add(device);

        _conn.OnRecieveTcpPackage += (string _data)=>{
            string[] formatData = _data.Split(';');
            Console.WriteLine(string.Format("[SERVER] DEVICE[{0}] recieved data: {1}", _conn.networkId, _data));

            if (formatData[0] == "COMPLETE") {
                foreach (NetworkUser user in _userConnections) {
                    user.updateDevice(device);
                }
            }
        };

        _conn.send("INIT");
    }

    
}

