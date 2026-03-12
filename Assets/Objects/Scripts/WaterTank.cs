using UnityEngine;

public class WaterTank : Grabbable, IConnectableSource
{
    [SerializeField]
    private float maxWaterAmount;
    [SerializeField]
    private float waterAmount;
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
        if(amount < 0 && waterAmount <= 0)
        {
            return false;
        }
        else
        {
            waterAmount = Mathf.Clamp(waterAmount + amount, 0, maxWaterAmount);
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
        }
        else
        {
            connector.transform.parent = null;
        }
        
    }

    private void SetShaderFill()
    {
        rd.material.SetFloat("_Fill", waterAmount/100f);
    }
}
