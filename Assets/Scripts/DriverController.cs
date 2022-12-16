using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DriverController : MonoBehaviour
{
    Delivery delivery;
    SpriteRenderer spriteRenderer;
    [SerializeField] List<SpecificationSO> specs;
    float steerAmount, moveAmount, nitroAmount;
    [SerializeField] int vehicleLevel;
    [Header("Collisions")]
    [SerializeField] int collisionOccured;
    [SerializeField] int collisionsAllowed;
    [SerializeField] int numOfBlinks = 3;
    [SerializeField] float blinkDuration = 0.25f;
    [Header("Speeds")]
    [SerializeField] int moveSpeed;
    [SerializeField] int steerSpeed;
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
        nitroAmount = Input.GetAxis("Fire1") * (moveSpeed / 2) * Time.deltaTime;
        transform.Rotate(0, 0, -steerAmount);
        transform.Translate(0, moveAmount + nitroAmount, 0);
    }
    bool HasReachedMaxCollisions()
    {
        return collisionOccured == collisionsAllowed;
    }
    int CollisionsRemainded()
    {
        return collisionsAllowed - collisionOccured;
    }
    void ModifyVehicle(int level, Sprite skin, int move, int steer, int newGoal)
    {
        collisionOccured = 0;
        vehicleLevel += level;
        spriteRenderer.sprite = skin;
        moveSpeed = move;
        steerSpeed = steer;
        delivery.DeliveryGoal = newGoal;
    }
    void ModifyVehicle(int level)
    {
        SpecificationSO spec = GetVehicleSpecByIndex(level);
        if (spec)
        {
            vehicleLevel = spec.GetLevel();
            spriteRenderer.sprite = spec.GetSkin();
            moveSpeed = spec.GetMoveSpeed();
            steerSpeed = spec.GetSteerSpeed();
            delivery.DeliveryGoal = spec.GetDeliveryGoal();
            collisionsAllowed = spec.GetCollisionsAllowed();
            collisionOccured = 0;
        }
    }
    void ModifyVehicle(string model)
    {
        collisionOccured = 0;
        SpecificationSO spec = GetVehicleSpecByModel(model);
        if (spec)
        {
            vehicleLevel = spec.GetLevel();
            spriteRenderer.sprite = spec.GetSkin();
            moveSpeed = spec.GetMoveSpeed();
            steerSpeed = spec.GetSteerSpeed();
            delivery.DeliveryGoal = spec.GetDeliveryGoal();
        }
    }
    void Downgrade()
    {
        if (vehicleLevel == 0)
            return;

        delivery.Init();
        ModifyVehicle(vehicleLevel - 1);
    }
    void Upgrade()
    {
        delivery.Init();
        ModifyVehicle(vehicleLevel + 1);
    }
    void CollisionPenalty()
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

        if (HasReachedMaxCollisions())
        {
            StartCoroutine("Blink");
            Downgrade();
            CollisionPenalty();
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
                if (delivery.Delivered >= delivery.DeliveryGoal)
                    Upgrade();
                break;
        }
    }
    SpecificationSO GetVehicleSpecByIndex(int index)
    {
        return specs[index];
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
