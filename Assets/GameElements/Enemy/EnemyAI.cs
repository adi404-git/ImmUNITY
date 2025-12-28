using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float moveSpeed = 2f;       // Bacterium = 2, Fast Parasite = 5
    public float rotationSpeed = 5f;   // How fast it turns to face you
    
    private Transform player;

    void Start()
    {
        // Automatically find the object tagged "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        // If player is dead or not found, stop moving
        if (player == null) return;

        // 1. Calculate Direction
        Vector3 direction = player.position - transform.position;
        direction.Normalize(); // Makes the length 1 so diagonal speed isn't faster

        // 2. Move Towards Player
        // We use Space.World so it moves towards the TARGET, not its own "nose"
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        // 3. Rotate to Face Player (Optional Polish)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // We subtract 180 degrees because usually sprites face Right, but "LookAt" logic varies
        // If your enemy flies backwards, remove the "+ 180" or change it to "- 90"
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}