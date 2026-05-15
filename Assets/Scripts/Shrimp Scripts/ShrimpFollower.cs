using UnityEngine;

public class ShrimpFollower : MonoBehaviour {
    public ShrimpManager shrimpManager;

    [Header("Leader Settings")]
    public Transform leader;
    public float swarmRadius = 3f;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float positionSmoothness = 0.2f;
    public float rotationSmoothness = 5f;

    [Header("Organic Motion")]
    public float noiseFrequency = 0.5f;
    public float noiseAmplitude = 0.5f;
    public float wobbleIntensity = 0.1f;
    public float wobbleSpeed = 2f;

    [Header("Ground Interaction")]
    public float groundHeight = -1.232769f;        // Y-level of ground
    public float squashAmount = 0.4f;      // How much to squash
    public float squashSpeed = 8f;         // How fast it squashes
    public LayerMask groundMask;



    private Vector3 velocity;
    private Vector3 targetOffset;

    void Start() {

        targetOffset = GetRandomOffset();
        leader = GameObject.FindGameObjectWithTag("Invisible").GetComponent<Transform>(); //Set leader reference
        shrimpManager = FindObjectOfType<ShrimpManager>();  
        shrimpManager.AddShrimpToTroupe(gameObject); //Add shrimp to troupe

        // Add a Capsule Collider if none exists
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        if (collider == null) {
            collider = gameObject.AddComponent<CapsuleCollider>();
        }

        // Configure the Capsule Collider
        collider.center = new Vector3(0f, -0.01f, 0f); // Center as specified
        collider.radius = 0.02f;                       // Radius as specified
        collider.height = 0.11f;                       // Height as specified
        collider.direction = 2;                        // Z-axis

        // Add and configure a Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

    }

    void Update() {
        if (leader == null) return;

        Vector3 noiseOffset = new Vector3(
            Mathf.PerlinNoise(Time.time * noiseFrequency, 0) - 0.5f,
            0,
            Mathf.PerlinNoise(0, Time.time * noiseFrequency) - 0.5f
        ) * noiseAmplitude;

        Vector3 targetPosition = leader.position + targetOffset + noiseOffset;

        velocity = Vector3.Lerp(velocity, (targetPosition - transform.position).normalized * moveSpeed, Time.deltaTime / positionSmoothness);
        transform.position += velocity * Time.deltaTime;

        // // Immediately clamp Y so next frame starts above ground
        // if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 1f, groundMask))
        // {
        //     if (transform.position.y < hit.point.y)
        //     {
        //         Vector3 p = transform.position;
        //         p.y = hit.point.y;
        //         transform.position = p;
        //     }
        // }

        // --- MOVEMENT & ROTATION (unchanged above this point) ---

       Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 1f, groundMask))
        {
            float distanceToGround = hit.distance;

            // Clamp position BEFORE squash
            if (transform.position.y < hit.point.y)
            {
                Vector3 p = transform.position;
                p.y = hit.point.y;
                transform.position = p;
            }

            // Squash when close
            if (distanceToGround < 0.1f)
            {
                float t = Mathf.InverseLerp(0.1f, 0f, distanceToGround);
                float squash = Mathf.Lerp(1f, squashAmount, t);

                transform.localScale = Vector3.Lerp(
                    transform.localScale,
                    new Vector3(1f, squash, 1f),
                    Time.deltaTime * squashSpeed
                );
            }
            else
            {
                transform.localScale = Vector3.Lerp(
                    transform.localScale,
                    Vector3.one,
                    Time.deltaTime * squashSpeed
                );
            }
        }

         Quaternion wobble = Quaternion.Euler(
            Mathf.Sin(Time.time * wobbleSpeed) * wobbleIntensity,
            Mathf.Sin(Time.time * wobbleSpeed * 1.5f) * wobbleIntensity,
            0
        );

        if (velocity.magnitude > 0.1f) {
            Quaternion targetRotation = Quaternion.LookRotation(velocity) * wobble;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothness);
        }

        if (Random.value < 0.01f) {
            targetOffset = GetRandomOffset();
        }


    }

    private Vector3 GetRandomOffset() {
        Vector2 randomCircle = Random.insideUnitCircle * swarmRadius;
        return new Vector3(randomCircle.x, 0, Mathf.Abs(randomCircle.y));
    }

    // void OnCollisionEnter(Collision other) {
    //     /* keeps the shrimp from falling through the floor */
    //     // int lowestPosition = 0;
    //     Vector3 p = transform.position;
    //     if (p.y < other.transform.position.y + 2)
    //     {
    //         p.y = other.transform.position.y + 2;
    //         transform.position = p;  // you can set the position as a whole, just not individual fields
        // }
    // }
}
