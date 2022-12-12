using UnityEngine;
public class DriverController : MonoBehaviour
{
    Delivery delivery;
    SpriteRenderer spriteRenderer;
    Sprite car1Sprite, car2Sprite, car3Sprite, bike1Sprite, bike2Sprite;
    float steerAmount, moveAmount, nitroAmount;
    [SerializeField] int vehicleLevel = 1;
    [Header("Collisions")]
    [SerializeField] int collisionsAllowed = 5;
    [SerializeField] int collisionOccured = 0;
    [Header("Current Speeds")]
    [SerializeField] int moveSpeed = 15;
    [SerializeField] int steerSpeed = 125;
    [Header("Slow Speeds")]
    [SerializeField] int slowMoveSpeed = 10;
    [SerializeField] int slowSteerSpeed = 100;
    [Header("Boost Speeds")]
    [SerializeField] int boostMoveSpeed = 20;
    [SerializeField] int boostSteerSpeed = 150;
    [Header("Fast Speeds")]
    [SerializeField] int fastMoveSpeed = 22;
    [SerializeField] int fastSteerSpeed = 170;
    [Header("Super Speeds")]
    [SerializeField] int superMoveSpeed = 25;
    [SerializeField] int superSteerSpeed = 190;
    [Header("Bike Speeds")]
    [SerializeField] int bikeMoveSpeed = 30;
    [SerializeField] int bikeSteerSpeed = 200;
    [Header("Super Bike Speeds")]
    [SerializeField] int superBikeMoveSpeed = 35;
    [SerializeField] int superBikeSteerSpeed = 250;
    void Start()
    {
        delivery = FindObjectOfType<Delivery>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        LoadSpriteResources();
    }
    void LoadSpriteResources()
    {
        car1Sprite = Resources.Load<Sprite>("Car 1");
        car2Sprite = Resources.Load<Sprite>("Car 2");
        car3Sprite = Resources.Load<Sprite>("Car 3");
        bike1Sprite = Resources.Load<Sprite>("Motorcycle 1");
        bike2Sprite = Resources.Load<Sprite>("Motorcycle 2");
    }
    void Update()
    {
        steerAmount = Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime;
        moveAmount = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        nitroAmount = Input.GetAxis("Fire1") * (moveSpeed / 2) * Time.deltaTime;
        transform.Rotate(0, 0, -steerAmount);
        transform.Translate(0, moveAmount + nitroAmount, 0);
    }
    public bool HasReachedMaxCollisions()
    {
        return collisionOccured == collisionsAllowed;
    }
    public int CollisionsRemainded()
    {
        return collisionsAllowed - collisionOccured;
    }
    void ModifyVehicle(int level, Sprite skin, int move, int steer, int newGoal)
    {
        vehicleLevel += level;
        collisionOccured = 0;
        spriteRenderer.sprite = skin;
        moveSpeed = move;
        steerSpeed = steer;
        delivery.DeliveryGoal = newGoal;
    }
    void BoostVehicleSpeed()
    {
        switch (vehicleLevel)
        {
            case 2:
                moveSpeed = fastMoveSpeed + 5; // 27
                steerSpeed = fastSteerSpeed + 10; // 180
                break;

            case 3:
                moveSpeed = superMoveSpeed + 5; // 30
                steerSpeed = superSteerSpeed + 30; // 220
                break;

            case 4:
                moveSpeed = bikeMoveSpeed + 3; // 33
                steerSpeed = bikeSteerSpeed + 50; // 250
                break;

            case 5:
                moveSpeed = superBikeMoveSpeed + 5; // 40
                steerSpeed = superBikeSteerSpeed + 50; // 300
                break;

            default:
                moveSpeed = boostMoveSpeed; // 20
                steerSpeed = boostSteerSpeed; // 150
                break;
        }
    }
    void Downgrade()
    {
        switch (vehicleLevel)
        {
            case 2:
                ModifyVehicle(-1, car1Sprite, slowMoveSpeed + 3, slowSteerSpeed + 15, 2);
                Debug.Log("Vehicle has been downgraded to level " + vehicleLevel);
                break;

            case 3:
                ModifyVehicle(-1, car2Sprite, slowMoveSpeed + 7, slowSteerSpeed + 30, 5);
                Debug.Log("Vehicle has been downgraded to level " + vehicleLevel);
                break;

            case 4:
                ModifyVehicle(-1, car3Sprite, boostMoveSpeed, boostSteerSpeed, 10);
                Debug.Log("Vehicle has been downgraded to level " + vehicleLevel);
                break;

            case 5:
                ModifyVehicle(-1, bike1Sprite, fastMoveSpeed, fastSteerSpeed, 18);
                Debug.Log("Vehicle has been downgraded to level " + vehicleLevel);
                break;

            default:
                ModifyVehicle(0, car1Sprite, slowMoveSpeed, slowSteerSpeed, 2);
                Debug.Log("Maximum collisions have been reached. Car is broken!");
                break;
        }
    }
    void Upgrade()
    {
        switch (vehicleLevel)
        {
            case 2:
                ModifyVehicle(1, car3Sprite, superMoveSpeed, superSteerSpeed, 10);
                Debug.Log("Vehicle has been upgraded to level " + vehicleLevel);
                break;

            case 3:
                ModifyVehicle(1, bike1Sprite, bikeMoveSpeed, bikeSteerSpeed, 18);
                Debug.Log("Vehicle has been upgraded to level " + vehicleLevel);
                break;

            case 4:
                ModifyVehicle(1, bike2Sprite, superBikeMoveSpeed, superBikeSteerSpeed, 25);
                Debug.Log("Vehicle has been upgraded to level " + vehicleLevel);
                break;

            default:
                ModifyVehicle(1, car2Sprite, fastMoveSpeed, fastSteerSpeed, 5);
                Debug.Log("Vehicle has been upgraded to level " + vehicleLevel);
                break;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        collisionOccured++;

        if (HasReachedMaxCollisions())
        {
            Downgrade();
            CollisionPenalty();
        }
        else
            Debug.Log("It's just a scratch! " + CollisionsRemainded() + " collisions left.");
    }

    void CollisionPenalty()
    {
        int penalty = delivery.Delivered + (delivery.DeliveryGoal / 2);
        delivery.DeliveryGoal = penalty;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Booster":
                BoostVehicleSpeed();
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
}
