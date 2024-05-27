using UnityEngine;
[CreateAssetMenu(menuName = "Vehicle Specification", fileName = "New Specification")]
public class SpecificationSO : ScriptableObject
{
    [SerializeField] string model = "Model Name";
    [SerializeField] int level;
    [SerializeField] Sprite skin;
    [SerializeField] int moveSpeed;
    [SerializeField] int steerSpeed;
    [SerializeField] float nitroCapacity;
    [SerializeField] float nitroCooldown;
    [SerializeField] int deliveryGoal;
    [SerializeField] int collisionsAllowed;
    public string GetModel()
    {
        return model;
    }
    public int GetLevel()
    {
        return level;
    }
    public Sprite GetSkin()
    {
        return skin;
    }
    public int GetMoveSpeed()
    {
        return moveSpeed;
    }
    public int GetSteerSpeed()
    {
        return steerSpeed;
    }
    public float GetNitroCapacity()
    {
        return nitroCapacity;
    }
    public float GetNitroCooldown()
    {
        return nitroCooldown;
    }
    public int GetDeliveryGoal()
    {
        return deliveryGoal;
    }
    public int GetCollisionsAllowed()
    {
        return collisionsAllowed;
    }
}
