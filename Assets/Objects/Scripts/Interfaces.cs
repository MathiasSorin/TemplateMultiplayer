using UnityEngine;

public interface IConnectableSource
{
    void ConnectToSource(Connector connector);
    bool UpdateSourceAmount(float amount);
}