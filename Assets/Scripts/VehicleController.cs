using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VehicleController : MonoBehaviour
{
    Delivery delivery;
    SpriteRenderer spriteRenderer;
    [SerializeField] List<SpecificationSO> specs;
    SpecificationSO currentSpec;
    float steerAmount, moveAmount, nitroAmount;
    [SerializeField] int vehicleLevel;

    [Header("Drive Control")]
    [SerializeField] int moveSpeed;
    [SerializeField] int steerSpeed;
    [SerializeField] float nitroCapacity;
    [SerializeField] float nitroCooldown;

    [Header("Collisions")]
    [SerializeField] int collisionOccured;
    [SerializeField] int collisionsAllowed;
    [SerializeField] int numOfBlinks = 3;
    [SerializeField] float blinkDuration = 0.25f;
    void Start()
    {
        delivery = FindObjectOfType<Delivery>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ModifyVehicle(0);
    }
    void Update()
    {
        steerAmount = Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime;
        moveAmount = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        if (moveAmount != 0) // Enable steering only when vehicle is moving
        {
            transform.Rotate(0, 0, -steerAmount);

            if (moveAmount > 0) // Only when vehicle moves forward
            {
                nitroAmount = Input.GetAxis("Fire1") * (moveSpeed * 0.75f) * Time.deltaTime;
                HandleNitroInput();
            }
        }
        else if (nitroAmount != 0) // Stop nitro when vehicle is not moving
            nitroAmount = 0;

        CooldownNitro();
        moveAmount += nitroAmount;
        transform.Translate(0, moveAmount, 0);
    }
    void HandleNitroInput()
    {
        if (currentSpec.GetNitroCapacity() <= 0)
            nitroAmount = 0;
        else if (nitroAmount != 0)
            ActivateNitro();
    }
    void ActivateNitro()
    {
        if (nitroCapacity > 0 && nitroCooldown > 0)
            nitroCapacity -= Time.deltaTime;
        else if (nitroCooldown <= 0) // Reset nitro capacity and nitro cooldown timer when cooldown has reached
        {
            nitroCapacity = currentSpec.GetNitroCapacity();
            nitroCooldown = currentSpec.GetNitroCooldown();
        }
    }
    void CooldownNitro()
    {
        if (nitroCapacity <= 0 && nitroCooldown > 0)
        {
            nitroAmount = 0;
            nitroCooldown -= Time.deltaTime;
        }
    }
    bool MaxCollisionsExceeded()
    {
        return collisionOccured == collisionsAllowed;
    }
    bool DeliveryGoalReached()
    {
        return delivery.Delivered >= delivery.DeliveryGoal;
    }
    void ModifyVehicle(int level, Sprite skin, int move, int steer, float capacity, float cooldown, int newGoal)
    {
        collisionOccured = 0;
        vehicleLevel += level;
        spriteRenderer.sprite = skin;
        moveSpeed = move;
        steerSpeed = steer;
        nitroCapacity = capacity;
        nitroCooldown = cooldown;
        delivery.DeliveryGoal = newGoal;
    }
    void SetSpecAttributes(SpecificationSO spec)
    {
        currentSpec = spec;
        vehicleLevel = spec.GetLevel();
        spriteRenderer.sprite = spec.GetSkin();
        moveSpeed = spec.GetMoveSpeed();
        steerSpeed = spec.GetSteerSpeed();
        nitroCapacity = spec.GetNitroCapacity();
        nitroCooldown = spec.GetNitroCooldown();
        delivery.DeliveryGoal = spec.GetDeliveryGoal();
        collisionsAllowed = spec.GetCollisionsAllowed();
        collisionOccured = 0;
    }
    void ModifyVehicle(int level)
    {
        SpecificationSO spec = GetVehicleSpecByLevel(level);
        if (spec)
            SetSpecAttributes(spec);
    }
    void ModifyVehicle(string model)
    {
        SpecificationSO spec = GetVehicleSpecByModel(model);
        if (spec)
            SetSpecAttributes(spec);
    }
    void DowngradeVehicle()
    {
        if (vehicleLevel == 0)
            return;

        delivery.Init();
        ModifyVehicle(vehicleLevel - 1);
    }
    void UpgradeVehicle()
    {
        delivery.Init();
        ModifyVehicle(vehicleLevel + 1);
    }
    void SetCollisionPenalty()
    {
        int penalty = (int)(delivery.DeliveryGoal / 2);
        delivery.DeliveryGoal += penalty;
    }
    IEnumerator Blink()
    {
        Color32 transparent = new Color32(255, 255, 255, 0);
        Color32 regular = new Color32(255, 255, 255, 255);
        for (int i = 1; i <= numOfBlinks; i++)
        {
            spriteRenderer.color = transparent;
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = regular;
            yield return new WaitForSeconds(blinkDuration);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        collisionOccured++;

        if (MaxCollisionsExceeded())
        {
            StartCoroutine("Blink");
            DowngradeVehicle();
            SetCollisionPenalty();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Booster":
                break;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Customer":
                if (DeliveryGoalReached())
                    UpgradeVehicle();
                break;
        }
    }
    SpecificationSO GetVehicleSpecByLevel(int level)
    {
        foreach (SpecificationSO spec in specs)
        {
            if (spec.GetLevel() == level)
                return spec;
        }
        return null;
    }
    SpecificationSO GetVehicleSpecByModel(string model)
    {
        foreach (SpecificationSO spec in specs)
        {
            if (spec.GetModel().Equals(model))
                return spec;
        }
        return null;
    }
}
