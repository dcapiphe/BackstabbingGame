using UnityEngine;

/// <summary>Respawns the player at the position where PlayerMotor started in this scene.</summary>
public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMotor player = other.GetComponentInParent<PlayerMotor>();
        if (player != null) player.RespawnAtLevelStart();
    }
}
