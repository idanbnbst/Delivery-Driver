using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    float steerAmount, moveAmount, nitroAmount;
    [Header("Slow Speeds")]
    [SerializeField] int slowMoveSpeed = 10;
    [SerializeField] int slowSteerSpeed = 100;

    [Header("Basic Speeds")]
    [SerializeField] int moveSpeed = 15;
    [SerializeField] int steerSpeed = 125;

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
    Delivery deliveryVehicle;
    private void Start()
    {
        deliveryVehicle = FindObjectOfType<Delivery>();
    }
    private void Update()
    {
        steerAmount = Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime;
        moveAmount = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        nitroAmount = Input.GetAxis("Fire1") * (moveSpeed / 2) * Time.deltaTime;
        transform.Rotate(0, 0, -steerAmount);
        transform.Translate(0, moveAmount + nitroAmount, 0);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (deliveryVehicle.getNumOfCollisions() > 2)
        {
            switch (deliveryVehicle.getVehicleLevel())
            {
                case 1:
                    moveSpeed = slowMoveSpeed + 3; // 13
                    steerSpeed = slowSteerSpeed + 15; // 115
                    break;
                case 2:
                    moveSpeed = slowMoveSpeed + 7; // 17
                    steerSpeed = slowSteerSpeed + 30; // 130
                    break;
                case 3:
                    moveSpeed = boostMoveSpeed; // 20
                    steerSpeed = boostSteerSpeed; // 150
                    break;
                case 4:
                    moveSpeed = fastMoveSpeed; // 22
                    steerSpeed = fastSteerSpeed; // 170
                    break;
                default:
                    moveSpeed = slowMoveSpeed; // 10
                    steerSpeed = slowSteerSpeed; // 100
                    break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Booster":
                switch (deliveryVehicle.getVehicleLevel())
                {
                    case 1:
                        moveSpeed = fastMoveSpeed + 5; // 27
                        steerSpeed = fastSteerSpeed + 10; // 180
                        break;
                    case 2:
                        moveSpeed = superMoveSpeed + 5; // 30
                        steerSpeed = superSteerSpeed + 30; // 220
                        break;
                    case 3:
                        moveSpeed = bikeMoveSpeed + 3; // 33
                        steerSpeed = bikeSteerSpeed + 50; // 250
                        break;
                    case 4:
                        moveSpeed = superBikeMoveSpeed + 5; // 40
                        steerSpeed = superBikeSteerSpeed + 50; // 300
                        break;
                    default:
                        moveSpeed = boostMoveSpeed; // 20
                        steerSpeed = boostSteerSpeed; // 150
                        break;
                }
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Customer":
                switch (deliveryVehicle.getVehicleLevel())
                {
                    case 1:
                        moveSpeed = fastMoveSpeed;
                        steerSpeed = fastSteerSpeed;
                        break;
                    case 2:
                        moveSpeed = superMoveSpeed;
                        steerSpeed = superSteerSpeed;
                        break;
                    case 3:
                        moveSpeed = bikeMoveSpeed;
                        steerSpeed = bikeSteerSpeed;
                        break;
                    case 4:
                        moveSpeed = superBikeMoveSpeed;
                        steerSpeed = superBikeSteerSpeed;
                        break;
                    default:
                        moveSpeed = boostMoveSpeed - 5; // 15 -> Basic move speed
                        steerSpeed = boostSteerSpeed - 25; // 125 -> Basic steer speed
                        break;
                }
                break;
        }
    }
}
