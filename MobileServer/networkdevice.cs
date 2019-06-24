using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NetworkDevice{

    public string id;
    public string description;

    private TcpConnection _connection;

    public List<NetworkCommand> regcom;
    public List<NetworkCommand> regret;

    public NetworkDevice(TcpConnection _connection, string _id, string _description) {
        this._connection = _connection;
        this.id = _id;
        this.description = _description;

        regcom = new List<NetworkCommand>();
        regret = new List<NetworkCommand>();

        this._connection.OnRecieveTcpPackage = null;
        this.setupPackageHandeling();
    }

    private void setupPackageHandeling() {
        _connection.OnRecieveTcpPackage += (string _data) => {
            string[] formatData = _data.Split(';');
            Console.WriteLine(string.Format("[SERVER] ({1})recieved data: {0}", _data, formatData.Length));
            if (formatData[0] == "REGCOM") {
                regcom.Add(new NetworkCommand(formatData));
            }else if (formatData[0] == "REGRET"){
                regret.Add(new NetworkCommand(formatData));
            }
        };
    }
}

