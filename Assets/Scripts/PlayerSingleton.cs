using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static GameObject Player;

    private void Awake()
    {
        PlayerSingleton.Player = this.gameObject;
    }
}
