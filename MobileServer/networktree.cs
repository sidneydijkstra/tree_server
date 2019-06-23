using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class NetworkTree{
    private static string AUTH_KEY = "AABBCCDD11223344";

    private TcpListener _listener;

    private List<TcpConnection> _connections;
    private List<NetworkCommand> _commands;

    public NetworkTree() {
        _connections = new List<TcpConnection>();
        _commands = new List<NetworkCommand>();

        _listener = new TcpListener("192.168.1.3", 11000);
        _listener.listen(_onConnection);
    }

    private void _onConnection(Socket _socket) {
        TcpConnection connection = new TcpConnection(_socket);
        connection.OnRecieveTcpPackage += (string _data) => {
            string[] formatData = _data.Split(';');
            Console.WriteLine(string.Format("[SERVER] ({1})recieved data: {0}", _data, formatData.Length));
            if (formatData[0] == "AUTHDEV") {
                if (formatData[1] == AUTH_KEY) {
                    connection.send("INIT");
                }
            } else if (formatData[0] == "REGCOM") {
                string name = formatData[1];

                // TODO params sepirate whit [,]
                //commandParam[] param = new commandParam[formatData.Length - 2];
                //for (int i = 2; i < param.Length; i++)
                //{
                //    param[i - 2] = (commandParam)int.Parse(formatData[i]);
                //}
                //_commands.Add(new NetworkCommand(name, param));

                string c = "COM;";
                c += name;
                c += ";ewa zemmer!!";

                connection.send(c);
            }
        };
        connection.OnConnectionClosed += () => {
            Console.WriteLine("[SERVER] connection--");
        };
        _connections.Add(connection);
    }
}

