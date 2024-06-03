using UnityEngine;

public class SeatManager : MonoBehaviour
{
    public bool isOccupied = false;

    public bool TryToSit()
    {
        if (!isOccupied)
        {
            isOccupied = true;
            return true;
        }
        return false;
    }

    public void StandUp()
    {
        isOccupied = false;
    }
}