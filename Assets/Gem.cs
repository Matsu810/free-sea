using UnityEngine;

public class Gem : MonoBehaviour
{
    public int expAmount = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddExp(expAmount);
            }
            Destroy(gameObject);
        }
    }
}