using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class NetworkUser{
    private TcpConnection _connection;

    public NetworkUser(TcpConnection _connection) {
        this._connection = _connection;

        this._connection.OnRecieveTcpPackage = null;
        this.setupPackageHandeling();
    }
    

    public void updateDevice(NetworkDevice _device) {
        Thread th = new Thread(new ParameterizedThreadStart(_updateDevice));
        th.Start(_device);
    }
    private void _updateDevice(object b) {
        NetworkDevice _device = (NetworkDevice)b;
        _connection.send("REGDEV;" + _device.id + ";" + _device.description);
        Thread.Sleep(500);
        foreach (NetworkCommand comm in _device.regcom)
        {
            Thread.Sleep(500);
            _connection.send("DEVUPD;" + _device.id + ";" + comm.formatRegCom());
        }
        foreach (NetworkCommand comm in _device.regret)
        {
            Thread.Sleep(500);
            _connection.send("DEVUPD;" + _device.id + ";" + comm.formatRegRet());
        }
    }

    private void setupPackageHandeling() {
        //_connection.OnRecieveTcpPackage += (string _data) => {
            
        //};
    }
}

