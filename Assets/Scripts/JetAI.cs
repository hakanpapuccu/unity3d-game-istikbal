using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetAI : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Vector3[] targetPos;
    private bool isDamage;
    private int targetIndex = 0;
    private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private GameObject radar;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(!isDamage)
            Rota();
    }

    void Rota()
    {

        Vector3 target = targetPos[targetIndex];

        // Hedefe doðru yönel
        Vector3 targetDirection = (target - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        // Hareket et
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Hedefe ulaþýldýðýnda bir sonraki hedefe geç
        if (Vector3.Distance(transform.position, target) < 0.2f)
        {
            targetIndex = (targetIndex + 1) % targetPos.Length;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (!isDamage)
        {
            GameManager.instance.EnemyDestroyed();
            isDamage = true;
            GetComponent<Rigidbody>().useGravity = true;
            explosionParticle.Play();
            audioSource.PlayOneShot(explosionSound);
            radar.SetActive(false);
        }
    }


}
