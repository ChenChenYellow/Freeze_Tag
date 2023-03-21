using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static float maxTime = 120,
        timer;
    private static int maxSavedAllies = 5,
    savedAllies;
    private static bool isWon = false, isLost = false;
    public static int SavedAllies
    {
        get { return savedAllies; }
        set
        {
            savedAllies = value;
            if (savedAllies >= maxSavedAllies)
            {
                isWon = true;
            }
        }
    }
    public static int MaxSavedAllies { get { return maxSavedAllies; } }
    public static float Timer
    {
        get { return timer; }
        set
        {
            timer = value; if (timer <= 0)
            {
                isWon = true;
            }
        }
    }
    public static bool IsWon { get { return isWon; } }
    public static bool IsLost { get { return isLost; } }

    public static void Reset()
    {
        timer = maxTime;
        savedAllies = 0;
        isWon = false;
        isLost = false;
    }
    public static void Lost()
    {
        isLost = true;
    }
    public static void Save()
    {
        SavedAllies++;
    }

    private void Start()
    {
        Reset();
    }
    private void Update()
    {
        Timer -= Time.deltaTime;
    }



}
