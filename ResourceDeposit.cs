using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDeposit : NonmovingAgent
{
    private ResourceType _type; // Type of resource deposit holds
    private float _count; // Amount of resource deposit holds

    // Start is called before the first frame update
    void Start()
    {
        _count = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Attempts to withdraw a passed-in amount of the ResourceDeposit's resource. 
    public float WithdrawResource(float amount)
    {
        float r = 0f; // Amount to subtract from _count and return with method

        // Return _count if there isn't enough to withdraw. Otherwise return the amount requested. 
        if (amount > _count)
        {
            r = amount;
        }
        else
        {
            r = _count;
        }

        _count -= r;
        return r;
    }

    /// <summary>
    /// Returns the ResourceType of the ResourceDeposit.
    /// </summary>
    /// <returns></returns> _type
    public ResourceType GetResourceType()
    {
        return _type;
    }
}
