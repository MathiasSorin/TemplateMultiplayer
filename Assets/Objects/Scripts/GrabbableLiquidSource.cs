using UnityEngine;

public class GrabbableLiquidSource : Grabbable, IConnectableSource
{
    [SerializeField]
    private float maxliquidAmount;
    [SerializeField]
    private float liquidAmount;
    [SerializeField]
    private Transform connectionTransform;

    private Connector connector;
    private Renderer rd;

    protected override void Awake()
    {
        base.Awake();
        rd = GetComponent<Renderer>();
        SetShaderFill();
    }

    public override void Interact(Player player)
    {
        Grab(player);
    }

    public bool UpdateSourceAmount(float amount)
    {
        if(amount < 0 && liquidAmount <= 0)
        {
            return false;
        }
        else
        {
            liquidAmount = Mathf.Clamp(liquidAmount + amount, 0, maxliquidAmount);
            SetShaderFill();
            return true;
        }
    }

    public void ConnectToSource(Connector connector)
    {
        this.connector = connector;
        if (connector != null)
        {
            connector.transform.parent = connectionTransform;
            connector.transform.position = connectionTransform.position;
            connector.transform.rotation = connectionTransform.rotation;
        }
        else
        {
            connector.transform.parent = null;
        }
        
    }

    public void Fill()
    {
        liquidAmount = maxliquidAmount;
        SetShaderFill();
    }

    private void SetShaderFill()
    {
        rd.material.SetFloat("_Fill", liquidAmount/100f);
    }
}
