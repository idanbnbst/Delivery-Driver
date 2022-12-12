using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Sprite car1Sprite, car2Sprite, car3Sprite, bike1Sprite, bike2Sprite;
    [SerializeField] bool inProgress;
    [SerializeField] int totalDelivered = 0;
    [SerializeField] int deliveredCount = 0;
    [SerializeField] float pickupDestroyTime = 0f;
    [Header("Delivery Indication")]
    [SerializeField] Color32 defaultColor;
    [SerializeField] Color32 inProgressColor;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public bool InProgress { get { return inProgress; } set { inProgress = value; } }
    public int Delivered { get { return deliveredCount; } set { deliveredCount = value; } }
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Package":
                if (!inProgress)
                {
                    inProgress = true;
                    spriteRenderer.color = inProgressColor;
                    Destroy(other.gameObject, pickupDestroyTime);
                    Debug.Log("Delivery picked");
                }
                break;

            case "Customer":
                if (inProgress)
                {
                    inProgress = false;
                    totalDelivered++;
                    deliveredCount++;
                    spriteRenderer.color = defaultColor;
                    Debug.Log("Package delivered to Customer");
                }
                break;
        }
    }
}
