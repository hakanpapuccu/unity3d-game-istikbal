using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Animator anim, enemyAnim;
    public AudioSource audioSource;
    public ParticleSystem muzzleEffect, impactStoneEffect, impactFleshSmallEffect;
    public AudioClip shootSoundEffect, reloadSoundEffect, emptySoundEffect;
    public float shootDelay, reloadDelay;
    private bool shooted = false, reloaded = false;
    public Transform rayPoint;
    public float range;
    RaycastHit hit;

    public int projectileCount;
    public int stockProjectileCount;
    private int currentProjectileCount;
    [SerializeField] private TextMeshProUGUI currentProjectileCountText, stockProjectileCountText;
    void Awake()
    {
        anim = GetComponent<Animator>();
        currentProjectileCount = projectileCount;
    }
    private void Start()
    {
       ProjectileText(currentProjectileCount, stockProjectileCount);
    }
    public void ProjectileText(int current, int stock)
    {
        currentProjectileCountText.text = current.ToString();
        stockProjectileCountText.text = "/" + stock.ToString();
    }
    public void Reload()
    {
        if (currentProjectileCount < projectileCount && stockProjectileCount > 0 && !reloaded)
        {
            reloaded = true;
            anim.Play("Reload");
            audioSource.PlayOneShot(reloadSoundEffect);
            if (stockProjectileCount < projectileCount - currentProjectileCount)
            {
                currentProjectileCount = stockProjectileCount;
                stockProjectileCount = 0;
            }
            else
            {
                stockProjectileCount -= projectileCount - currentProjectileCount;
                currentProjectileCount = projectileCount;
            }
            ProjectileText(currentProjectileCount, stockProjectileCount);
            Invoke(nameof(ReloadDelay), reloadDelay);
        }
    }

    public void Shoot()
    {
        if (!shooted && !reloaded && currentProjectileCount > 0)
        {
            shooted = true;
            --currentProjectileCount;
            audioSource.PlayOneShot(shootSoundEffect);
            muzzleEffect.Play();
           ProjectileText(currentProjectileCount, stockProjectileCount);
            if (Physics.Raycast(rayPoint.position, rayPoint.forward, out hit, range))
            {
                if (hit.transform.CompareTag("enemy"))
                {
                    impactFleshSmallEffect.transform.position = hit.point;
                    impactFleshSmallEffect.Play();

                    GameObject enemy = hit.transform.gameObject;
                    enemy.GetComponent<Enemy>().Damage(30);
                }
                else
                {
                    impactStoneEffect.transform.position = hit.point;
                    impactStoneEffect.Play();
                }
            }
            anim.Play("Shoot");
            Invoke(nameof(ShootDelay), shootDelay);
        }
        else if (!shooted && currentProjectileCount == 0)
        {
            audioSource.PlayOneShot(emptySoundEffect);
            shooted = true;
            Invoke(nameof(ShootDelay), 1f);
        }
    }

    private void ShootDelay()
    {
        shooted = false;
    }
    private void ReloadDelay()
    {
        reloaded = false;
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && GameManager.instance.gameActive)
        {
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.R) && GameManager.instance.gameActive)
        {
            Reload();
        }
    }
}
