using UnityEngine;

public class HouseStateManager : MonoBehaviour
{
    private bool isBought = false;

    public void SetBought()
    {
        isBought = true;
        Debug.Log(gameObject.name + " has been purchased!");
    }

    public bool IsBought()
    {
        return isBought;
    }
}
