using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MovingAgent
{
    // Transact resources
    private float _withdrawAmountPerAttempt; // The amount of resource the citz can withdraw per attempt from a ResourceDeposit

    // Cargo
    private ResourceType _resourceType; // The current type of resource held by the citz // TODO Must be changed whenever target is changed
    private float _currentResourcePayload; // Number or resouorce units currently held by citz
    private float _maxResourcePayload; // Maximum resource unit capacity of citz


    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        // Gameobject tag
        gameObject.tag = "Citizen";

        // Health
        _maxHealth = 10;
        ResetHealth();
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

    }

    /// <summary>
    /// Attempt to withdraw _withdrawAmountPerAttempt from a ResourceDeposit
    /// </summary>
    /// <param name="deposit"></param> The deposit to withdraw from. 
    public void WithdrawResource(ResourceDeposit deposit)
    {
        _resourceType = deposit.GetResourceType();
        float amountToStore = deposit.WithdrawResource(_withdrawAmountPerAttempt);
        _currentResourcePayload += amountToStore;
    }


    public void DepositResource(ResourceCollector collector)
    {
        if (_currentResourcePayload > 0)
        {
            float current = _currentResourcePayload;
            _currentResourcePayload = 0;
            collector.DepositResource(_resourceType, current);
        }
    }
}

/// <summary>
/// Describes the type of task the citz is currently assinged tos
/// </summary>
public enum TaskType { Unassigned, Harvester, Woodcutter, Miner };