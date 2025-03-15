using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

public class Circle : MonoBehaviour
{
    [FormerlySerializedAs("I")]
    [HideInInspector]
    public int i;

    [FormerlySerializedAs("J")]
    [HideInInspector]
    public int j;

    public float Health { get; private set; }

    private const float BaseHealth = 1000;

    private const float HealingPerSecond = 1;
    private const float HealingRange = 3;

    private Grid grid;
    private SpriteRenderer spriteRenderer;
    private List<Circle> nearbyCircles = new List<Circle>();
    private int numCircles;

    // Start is called before the first frame update
    private void Start()
    {
        Health = BaseHealth;
        grid = FindObjectOfType<Grid>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, HealingRange);

        if (nearbyColliders.Length == 0)
        {
            return;
        }
        for (int i = 0; i < nearbyColliders.Length; i++)
        {
            if (nearbyColliders[i] != null && nearbyColliders[i].TryGetComponent<Circle>(out var circle))
            {
                nearbyCircles.Add(circle);
            }
        }
        numCircles = nearbyCircles.Count;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateColor();
        HealNearbyShapes();
    }

    private void UpdateColor()
    {
        spriteRenderer.color = grid.Colors[i, j] * Health / BaseHealth;
    }

    private void HealNearbyShapes()
    {
        for (int i = 0; i < numCircles; i++)
        {
            nearbyCircles[i].ReceiveHp(HealingPerSecond * Time.deltaTime);
        }

    }

    public void ReceiveHp(float hpReceived)
    {
        Health = Mathf.Clamp(Health + hpReceived, 0, BaseHealth);
    }
}