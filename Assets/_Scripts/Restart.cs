using _Scripts;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public void GameRestart()
    {
        var oldRoom = GameObject.FindGameObjectWithTag("old_room");
        if (oldRoom)
        {
            Destroy(oldRoom);
        }

        // if (!GameObject.FindGameObjectWithTag("old_room"))
        // {
            GameManger.instance.ReadyToPlay(false);
            
            Debug.Log("Restarted");
        // }
    }
}
