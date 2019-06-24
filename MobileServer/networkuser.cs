using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NetworkUser{
    private TcpConnection _connection;

    public NetworkUser(TcpConnection _connection) {
        this._connection = _connection;

        this._connection.OnRecieveTcpPackage = null;
        this.setupPackageHandeling();
    }

    public void updateDevice(NetworkDevice _device) {
        _connection.send("REGDEV;" + _device.id + ";" + _device.description);
        foreach(NetworkCommand comm in _device.regcom){
            _connection.send(comm.formatRegCom());
        }
        foreach(NetworkCommand comm in _device.regret){
            _connection.send(comm.formatRegRet());
        }
    }

    private void setupPackageHandeling() {
        //_connection.OnRecieveTcpPackage += (string _data) => {
            
        //};
    }
}

