using UnityEngine;

namespace TopDown.Shooting

{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("Movement stats")]
        [SerializeField] private float speed;
        [SerializeField] private float lifetime;
        private Rigidbody2D body;
        private float lifeTimer;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        public void ShootBullet(Transform shootPoint)
        {
            lifeTimer = 0;
            body.linearVelocity = Vector2.zero; 
            transform.position = shootPoint.position;
            transform.rotation = shootPoint.rotation;
            gameObject.SetActive(true);

            body.AddForce(transform.up * speed, ForceMode2D.Impulse);
        }



        private void Update()
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifetime)
                gameObject.SetActive(false); 
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "enemy")
            {
                Debug.Log("Hit: " + other);
                Destroy(gameObject);
            }

            if (other.tag == "Wall")
            {
                Debug.Log("bullet hit wall");
                Destroy(gameObject);
            }
        }
    }
}