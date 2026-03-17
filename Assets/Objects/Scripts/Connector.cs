public class Connector : Grabbable
{
    public IConnectableSource connectedWaterSource;

    protected override void Awake()
    {
        base.Awake();
        canBeUsedOnType = typeof(IConnectableSource);
    }

    public override void Interact(Player player)
    {
        if(connectedWaterSource == null)
        {
            Grab(player);
        }
        else
        {
            ConnectConnector(null);
            Grab(player);
        }
    }

    public override void Use(bool isUsed, Interactable target = null)
    {
        if (isUsed)
        {
            if (target is IConnectableSource cws)
            {
                ConnectConnector(cws);
            }
        }
    }

    private void ConnectConnector(IConnectableSource source)
    {
        if (source != null)
        {
            connectedWaterSource = source;
            EnablePhysics(false);
            connectedWaterSource.ConnectToSource(this);
        }
        else
        {
            connectedWaterSource = null;
            EnablePhysics(true);
        }
    }
}
