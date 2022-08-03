using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    public bool hasPackage;
    public int vehicleLevel, packageCollected, collisionOccured;
    [SerializeField] int collisionAllowed = 3;
    [SerializeField] float pickupDestroyTime = 0f;

    [Header("Quest Colors")]
    [SerializeField] Color32 hasQuestColor;
    [SerializeField] Color32 noQuestColor;
    SpriteRenderer spriteRenderer;
    Sprite car1Sprite, car2Sprite, car3Sprite, bike1Sprite, bike2Sprite;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        car1Sprite = Resources.Load<Sprite>("Car 1");
        car2Sprite = Resources.Load<Sprite>("Car 2");
        car3Sprite = Resources.Load<Sprite>("Car 3");
        bike1Sprite = Resources.Load<Sprite>("Motorcycle 1");
        bike2Sprite = Resources.Load<Sprite>("Motorcycle 2");
        vehicleLevel = packageCollected = collisionOccured = 0;
    }
    public int getVehicleLevel()
    {
        return this.vehicleLevel;
    }
    public int getNumOfCollisions()
    {
        return this.collisionOccured;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        collisionOccured++;
        if (vehicleLevel > 0 && collisionOccured == collisionAllowed) // Downgrade vehicle
        {
            switch (vehicleLevel)
            {
                case 1:
                    spriteRenderer.sprite = car1Sprite;
                    break;
                case 2:
                    spriteRenderer.sprite = car2Sprite;
                    break;
                case 3:
                    spriteRenderer.sprite = car3Sprite;
                    break;
                case 4:
                    spriteRenderer.sprite = bike1Sprite;
                    break;
            }
            vehicleLevel--;
            collisionOccured = 0;
            Debug.Log("Vehicle has downgraded to level " + vehicleLevel);
        }
        else
            Debug.Log("It's just a scratch! " + (collisionAllowed - collisionOccured) + " collisions remain");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Package":
                if (!hasPackage)
                {
                    hasPackage = true;
                    packageCollected++;
                    spriteRenderer.color = hasQuestColor;
                    Destroy(other.gameObject, pickupDestroyTime);
                    Debug.Log("Delivery picked");
                }
                break;

            case "Customer":
                if (hasPackage)
                {
                    hasPackage = false;
                    spriteRenderer.color = noQuestColor;
                    Debug.Log("Package delivered to Customer");
                    switch (vehicleLevel) // Upgrade vehicle
                    {
                        case 1:
                            if (packageCollected >= 5)
                            {
                                vehicleLevel++;
                                spriteRenderer.sprite = car3Sprite;
                                collisionOccured = 0;
                                Debug.Log("Vehicle has upgraded to level " + vehicleLevel);
                            }
                            break;
                        case 2:
                            if (packageCollected >= 10)
                            {
                                vehicleLevel++;
                                spriteRenderer.sprite = bike1Sprite;
                                collisionOccured = 0;
                                Debug.Log("Vehicle has upgraded to level " + vehicleLevel);
                            }
                            break;
                        case 3:
                            if (packageCollected >= 13)
                            {
                                vehicleLevel++;
                                spriteRenderer.sprite = bike2Sprite;
                                collisionOccured = 0;
                                Debug.Log("Vehicle has upgraded to level " + vehicleLevel);
                            }
                            break;
                        default:
                            if (packageCollected >= 2)
                            {
                                vehicleLevel++;
                                spriteRenderer.sprite = car2Sprite;
                                collisionOccured = 0;
                                Debug.Log("Vehicle has upgraded to level " + vehicleLevel);
                            }
                            break;
                    }
                }
                break;
        }
    }
}
