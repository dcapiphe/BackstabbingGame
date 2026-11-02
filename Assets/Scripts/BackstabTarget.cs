using UnityEngine;

/// <summary>Simple enemy target with a visible forward direction and health.</summary>
public class BackstabTarget : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] private Transform visual;

    public bool IsDefeated { get; private set; }

    public void Defeat()
    {
        if (IsDefeated) return;
        IsDefeated = true;
        if (visual != null) visual.gameObject.SetActive(false);
        else gameObject.SetActive(false);
    }
}
