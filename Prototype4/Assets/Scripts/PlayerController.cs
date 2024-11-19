using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed;
    private float forwardInput;

    private GameObject focalPoint;

    public bool hasPowerup;
    private float powerupStrength = 15.0f;

    public GameObject powerupIndicator;
    public float powerupDuration = 7.0f;

    public float turboSpeedBoost = 10f;
    private bool isTurboBoostActive = false;

    public GameObject turboParticleEffect;
    private ParticleSystem turboParticles;

    private bool isTurboOnCooldown = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.FindGameObjectWithTag("FocalPoint");

        if (focalPoint == null)
        {
            Debug.LogError("FocalPoint not found! Make sure it exists in the scene.");
        }

        if (turboParticleEffect != null)
        {
            turboParticles = turboParticleEffect.GetComponent<ParticleSystem>();
            turboParticleEffect.SetActive(false);
        }
    }

    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");

        if (powerupIndicator.activeSelf)
        {
            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isTurboOnCooldown)
        {
            ActivateTurboBoost();
        }

        if (Input.GetKeyUp(KeyCode.Space) && isTurboBoostActive)
        {
            DeactivateTurboBoost();
        }
    }

    private void FixedUpdate()
    {
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(powerupDuration);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                Vector3 awayFromPlayer = collision.transform.position - transform.position;
                enemyRigidbody.AddForce(awayFromPlayer.normalized * powerupStrength, ForceMode.Impulse);
            }
        }
    }

    private void ActivateTurboBoost()
    {
        isTurboBoostActive = true;
        speed += turboSpeedBoost;

        if (turboParticleEffect != null)
        {
            turboParticleEffect.SetActive(true);
            turboParticles.Play();
        }

        isTurboOnCooldown = true;
        StartCoroutine(TurboBoostCooldown());
    }

    private void DeactivateTurboBoost()
    {
        isTurboBoostActive = false;
        speed -= turboSpeedBoost;

        if (turboParticleEffect != null)
        {
            turboParticleEffect.SetActive(false);
            turboParticles.Stop();
        }
    }

    private IEnumerator TurboBoostCooldown()
    {
        yield return new WaitForSeconds(5f);
        isTurboOnCooldown = false;
    }
}
