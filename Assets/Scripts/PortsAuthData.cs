using System;

[Serializable]
public class PortsAuthData
{
    public PortsAuthData(int clientPort, int serverPort)
    {
        this.clientPort = clientPort;
        this.serverPort = serverPort;
    }

    public int serverPort;
    public int clientPort;
}