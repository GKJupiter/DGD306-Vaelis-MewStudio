using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private bool player2Spawned = false;

    void Update()
    {
        if (!player2Spawned && Input.GetKeyDown(KeyCode.Tab)) // or controller button
        {
            SpawnPlayer2();
            player2Spawned = true;
        }
    }

    void SpawnPlayer2()
    {
        GameObject p2 = Instantiate(playerPrefab, new Vector3(2, 0, 0), Quaternion.identity);
        p2.GetComponent<PlayerMovement>().playerId = 2;
        p2.GetComponent<PlayerShooting>().playerId = 2;
    }
}
