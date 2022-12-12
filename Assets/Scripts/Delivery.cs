using UnityEngine;

public class Delivery : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Sprite car1Sprite, car2Sprite, car3Sprite, bike1Sprite, bike2Sprite;
    [SerializeField] bool inProgress;
    [SerializeField] int deliveryGoal = 2;
    [SerializeField] int totalDelivered = 0;
    [SerializeField] float pickupDestroyTime = 0f;
    [Header("Delivery Indication")]
    [SerializeField] Color32 defaultColor;
    [SerializeField] Color32 inProgressColor;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public bool InProgress { get { return inProgress; } set { inProgress = value; } }
    public int Delivered { get { return totalDelivered; } set { totalDelivered = value; } }
    public int DeliveryGoal { get { return deliveryGoal; } set { deliveryGoal = value; } }
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
                    spriteRenderer.color = defaultColor;
                    Debug.Log("Package delivered to Customer");
                }
                break;
        }
    }
}
