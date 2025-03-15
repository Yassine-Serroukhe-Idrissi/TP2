using UnityEngine;
using UnityEngine.Serialization;

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

    private SpriteRenderer _spriteRenderer; // Cache du SpriteRenderer
    private GridShape _grid; // Cache du GridShape
    private static readonly Collider2D[] _nearbyColliders = new Collider2D[10]; // Tableau statique pour éviter les allocations
    private static readonly ContactFilter2D _contactFilter = new ContactFilter2D().NoFilter();



    // Start is called before the first frame update
    private void Start()
    {
        Health = BaseHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>(); // Stocke la référence
        _grid = GameObject.FindFirstObjectByType<GridShape>(); // Stocke la référence

    }

    // Update is called once per frame
    private void Update()
    {
        UpdateColor();
        HealNearbyShapes();
    }

    private void UpdateColor()
    {
        _spriteRenderer.color = _grid.Colors[i, j] * (Health / BaseHealth);
    }

    private void HealNearbyShapes()
    {
        int count = Physics2D.OverlapCircle(transform.position, HealingRange, _contactFilter, _nearbyColliders);

        for (int k = 0; k < count; k++)
        {
            var nearbyCollider = _nearbyColliders[k];


            if (nearbyCollider == null)
                continue;

            if (nearbyCollider.TryGetComponent(out Circle circle))
            {
                circle.ReceiveHp(HealingPerSecond * Time.deltaTime);
            }
        }
    }

    public void ReceiveHp(float hpReceived)
    {
        Health = Mathf.Clamp(Health + hpReceived, 0, BaseHealth);

    }
}
