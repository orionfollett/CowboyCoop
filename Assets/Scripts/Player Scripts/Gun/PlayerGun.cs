using Cinemachine;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class PlayerGun : MonoBehaviour
{
    [Header("Required Player Info")]
    public LayerMask hitableLayerMask;
    public Transform cameraTransform;
    public GameObject playerRoot;
    public GameObject gun;
    public Transform gunRestTransform;
    public Transform gunAdsTransform;
    public Camera gunCamera;
    public CinemachineVirtualCamera vCamera;

    [Header("Gun Stats")]
    public int ammoLeft = 10;// ammo left in clip
    public int mags = 3;
    public int maxMags = 0;
    public int maxAmmoInMag = 10;
    public float range = 100;
    public float adsSpeed = 1.0f;
    public int aimFov;

    [Tooltip("Time in seconds between shots")]
    public double rof = 1.0;
    public int damage = 1;

    [Header("Sfx/Vfx")]
    public ParticleSystem muzzleFlash;
    public VisualEffect muzzleFlashVFX;
    public GameObject crosshair;
    public GameObject hitmarker;
    public TMP_Text ammoText;

    //private
    private StarterAssetsInputs _input;
    private bool _ableToShoot = true;
    private bool _ableToReload = true;
    private double _timeSinceLastShot = 0;

    private CanvasGroup _hitmarkerCanvas;
    private float _timeToFadeIn = 1.0f;
    private float _timeToFadeOut = 1.0f;
    private float _timeShowing = 1.0f;
    private string _audioDirection = "m";
    private PlayerMessageRouter _playerRouter;
    private int _defaultFov = 40;
    // Start is called before the first frame update
    void Start()
    {
        _playerRouter = playerRoot.GetComponent<PlayerMessageRouter>();

        _audioDirection = playerRoot.GetComponent<SetupPlayer>().audioDirection;
        _defaultFov = playerRoot.GetComponent<SetupPlayer>().fov;

        hitmarker.SetActive(true);
        _hitmarkerCanvas = hitmarker.GetComponent<CanvasGroup>();
        _hitmarkerCanvas.alpha = 0.0f;
        _timeToFadeIn = hitmarker.GetComponent<HitmarkerParameters>().timeToFadeIn;
        _timeToFadeOut = hitmarker.GetComponent<HitmarkerParameters>().timeToFadeOut;
        _timeShowing = hitmarker.GetComponent<HitmarkerParameters>().timeShowing;

        _input = GetComponent<StarterAssetsInputs>();
        ammoText.SetText(ammoLeft.ToString() + "/" + mags.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        if (!_input.shoot && !_ableToShoot)
        {
            var currTime = Time.fixedTimeAsDouble;
            if (_timeSinceLastShot + rof < currTime) {
                _timeSinceLastShot = currTime;
                _ableToShoot = true;
            }
        }

        if (_input.shoot && _ableToShoot)
        {
            _ableToShoot = false;
            Shoot();
        }

        if (_input.reload && _ableToReload)
        {
            Debug.Log("RELOAD");
            _ableToReload = false;
            Reload();
        }
        else {
            _ableToReload = true;
            _input.reload = false;
        }
    }

    void Aim() {
        if (_input.aim)
        {
            gun.transform.position = Vector3.Lerp(gun.transform.position, gunAdsTransform.position, adsSpeed * Time.deltaTime);
            vCamera.m_Lens.FieldOfView = Mathf.Lerp(vCamera.m_Lens.FieldOfView, aimFov, adsSpeed * Time.deltaTime);
            gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, aimFov, adsSpeed * Time.deltaTime);
        }
        else
        {
            gun.transform.position = Vector3.Lerp(gun.transform.position, gunRestTransform.position,  adsSpeed * Time.deltaTime);
            vCamera.m_Lens.FieldOfView = Mathf.Lerp(vCamera.m_Lens.FieldOfView, _defaultFov, adsSpeed * Time.deltaTime);
            gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, _defaultFov, adsSpeed * Time.deltaTime);
        }
    }

    void Shoot() {
        if (ammoLeft > 0) {
            ammoLeft -= 1;
            ammoText.SetText(ammoLeft.ToString() + "/" + mags.ToString());

            RaycastHit hitInfo;

            muzzleFlash.Play();
            muzzleFlashVFX.Play();
            _playerRouter.soundManager.PlaySound(_audioDirection, "rifleshot");

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo,  range, hitableLayerMask)) {
                GameObject hitObject = hitInfo.transform.gameObject;
                if (hitObject.tag == "Player" && hitObject.GetComponent<BulletDamageable>().playerId != GetComponent<BulletDamageable>().playerId) {
                    int id = hitObject.GetComponent<BulletDamageable>().playerId;
                    _playerRouter.SendDamage(damage, id);
                    StartCoroutine(DelayPlaySound("successfulHit", 0.20f));
                    StartCoroutine(ShowHitmarker());
                }
                else if (hitObject.tag == "Damageable") {
                    hitObject.SendMessage("Damage", damage);
                    StartCoroutine(DelayPlaySound("successfulHit", 0.20f));
                    StartCoroutine(ShowHitmarker());
                }
                else {
                    Debug.Log("Warning: player gun raycast detected something that is not a player: " + hitObject.name);
                }

                
            }
        }
    }

    void Reload() {
        if (mags > 0) {
            mags -= 1;
            ammoLeft = maxAmmoInMag;
            ammoText.SetText(ammoLeft.ToString() + "/" + mags.ToString());
        }
    }

    IEnumerator DelayPlaySound(string soundName, float timeInSeconds) {
        yield return new WaitForSeconds(timeInSeconds);
        _playerRouter.soundManager.PlaySound(_audioDirection, soundName);
    }

    IEnumerator ShowHitmarker() { 
        for (float alpha = 0f; alpha <= 1.0f; alpha += 0.1f)
        {
            _hitmarkerCanvas.alpha = alpha;
            yield return new WaitForSeconds(_timeToFadeIn / 10.0f);
        }
        
        yield return new WaitForSeconds(_timeShowing);

        for (float alpha = 1f; alpha >= 0f; alpha -= 0.1f)
        {
            _hitmarkerCanvas.alpha = alpha;
            yield return new WaitForSeconds(_timeToFadeOut / 10.0f);
        }

        _hitmarkerCanvas.alpha = 0f;
    }

    //IEnumerator LerpToPoint(Transform obj, Vector3 start, Vector3 end, float duration)
    //{
    //    float timeElapsed = 0;
    //    while (timeElapsed < duration)
    //    {
    //        obj.position = Vector3.Lerp(start, end, timeElapsed / duration);
    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    obj.position = end;
    //}


}
