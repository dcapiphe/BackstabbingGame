using TMPro;
using UnityEngine;

/// <summary>Press E near an enemy. A negative forward dot product means you are behind it.</summary>
public class BackstabAttack : MonoBehaviour
{
    [SerializeField] private float range = 2.2f;
    [SerializeField, Range(-1f, 0f)] private float behindDotThreshold = -0.35f;
    [SerializeField] private LayerMask targetLayers = ~0;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private float feedbackSeconds = 2f;

    private float feedbackUntil;

    private void Update()
    {
        if (feedbackText != null && Time.time > feedbackUntil) feedbackText.text = "";
        if (!Input.GetKeyDown(KeyCode.E)) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, range, targetLayers);
        BackstabTarget closest = null;
        float closestDistance = float.MaxValue;
        foreach (Collider hit in hits)
        {
            BackstabTarget candidate = hit.GetComponentInParent<BackstabTarget>();
            if (candidate == null || candidate.IsDefeated) continue;
            float distance = (candidate.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance) { closest = candidate; closestDistance = distance; }
        }

        if (closest == null) { Show("✖ Attack Failed — no enemy in range"); return; }
        Vector3 towardPlayer = (transform.position - closest.transform.position).normalized;
        float dot = Vector3.Dot(closest.transform.forward, towardPlayer);
        if (dot <= behindDotThreshold)
        {
            closest.Defeat();
            Show($"✔ Backstab Successful (dot = {dot:0.00})");
        }
        else Show($"✖ Attack Failed — get behind the enemy (dot = {dot:0.00})");
    }

    private void Show(string message)
    {
        if (feedbackText != null) feedbackText.text = message;
        feedbackUntil = Time.time + feedbackSeconds;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
