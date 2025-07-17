using UnityEngine;

public class camsnap : MonoBehaviour
{
    public Transform player; // Assign the player transform
    private float camHeight;
    public int currentLevel = 0; // Tracks which vertical screen we're on

    void Start()
    {
        camHeight = Camera.main.orthographicSize * 2;
        currentLevel = Mathf.FloorToInt(player.position.y / camHeight);
        SnapToCurrentLevel();
    }

    void Update()
    {
        int newLevel = Mathf.FloorToInt(player.position.y / camHeight);

        if (newLevel != currentLevel)
        {
            currentLevel = newLevel;
            SnapToCurrentLevel();
        }
    }

    void SnapToCurrentLevel()
    {
        transform.position = new Vector3(transform.position.x, currentLevel * camHeight + camHeight / 2, transform.position.z);
    }
}
