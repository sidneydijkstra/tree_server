using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NetworkDevice{

    public string id;
    public string description;

    private TcpConnection _connection;

    public NetworkDevice(TcpConnection _connection, string _id, string _description) {
        this._connection = _connection;
        this.id = _id;
        this.description = _description;

        this._connection.OnRecieveTcpPackage = null;
    }

    private void setupPackageHandeling() {
        _connection.OnRecieveTcpPackage += (string _data) => {
            string[] formatData = _data.Split(';');
            if (formatData[0] == "AUTHDEV") {
                
            }
        };
    }
}

