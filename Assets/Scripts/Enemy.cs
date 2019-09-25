using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Health")]
    [SerializeField] int health = 500;
    [SerializeField] float shootCounter;
    [SerializeField] float minTimeBetweenShoot = 0.2f;
    [SerializeField] float maxTimeBetweenShoot = 3f;

    [Header("Laser")]
    [SerializeField] GameObject enemeyLaser;
    [SerializeField] int scoreValue = 200;
    [SerializeField] float laserSpeed = 5f;

    
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;

    [Header("Sound")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0,1)] float deathVoulme = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootVolume = 0.25f;

    private void Start()
    {
        shootCounter = Random.Range(minTimeBetweenShoot, maxTimeBetweenShoot);
    }

    private void Update()
    {
        ShooterCountDown();
    }

    private void ShooterCountDown()
    {

        shootCounter -= Time.deltaTime;

        if(shootCounter <= 0)
        {
            Fire();
            shootCounter = Random.Range(minTimeBetweenShoot, maxTimeBetweenShoot);     
        }

    }

    private void Fire()
    {
        GameObject laser = Instantiate(enemeyLaser,
               transform.position,
               Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        if (!damageDealer) { return; }
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);

        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position,deathVoulme);
       
    }
}
