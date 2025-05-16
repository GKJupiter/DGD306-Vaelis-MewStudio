using UnityEngine;

public class TeleportationZone : MonoBehaviour
{
    public Transform teleportDestination;  // Işınlanacak nokta
    public string playerTag = "Player";    // Oyuncu tag'ini kontrol et

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer çarpışan nesne oyuncuysa
        if (other.CompareTag(playerTag))
        {
            // Oyuncuyu ışınla
            other.transform.position = teleportDestination.position;
        }
    }
}