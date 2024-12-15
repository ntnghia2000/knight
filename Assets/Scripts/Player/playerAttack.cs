using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Pool;

public class playerAttack : MonoBehaviour
{
    [Header("Attributed")]
    [SerializeField] private float fireCoolDown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ProjectTile projectTilePrefab;
    [SerializeField] private bool collectionCheck;
    [SerializeField] private int maxProjectTile;
    [Header("Sound")]
    [SerializeField] private AudioSource shootSound;

    private IObjectPool<ProjectTile> objectPool;
    private Animator ani;
    private Knight knight;
    private float coolDownTimer = Mathf.Infinity;

    private void Awake() {
        ani = GetComponent<Animator>();
        knight = GetComponent<Knight>();
        objectPool = new ObjectPool<ProjectTile>(CreateProjectTile, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, collectionCheck, 10, maxProjectTile);
    }

    private ProjectTile CreateProjectTile() {
        ProjectTile projectTileInstance = Instantiate(projectTilePrefab);
        projectTileInstance.ObjectPool = objectPool;
        return projectTileInstance;
    }

    private void OnGetFromPool(ProjectTile projectTile) {
        projectTile.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(ProjectTile projectTile) {
        projectTile.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(ProjectTile projectTile) {
        Destroy(projectTile.gameObject);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.C) && coolDownTimer > fireCoolDown &&
            knight.canShoot() && objectPool != null) {
            CastingFireBall();
        }

        coolDownTimer += Time.deltaTime;
    }

    private void CastingFireBall() {
        ani.SetTrigger("cast");
        coolDownTimer = 0;

        ProjectTile fireBall = objectPool.Get();

        if (fireBall != null) {
            fireBall.transform.position = firePoint.position;
            if (knight.GetIsWall() == true) {
                fireBall.SetDirection(Mathf.Sign(-transform.localScale.x));
            } else {
                fireBall.SetDirection(Mathf.Sign(transform.localScale.x));
            }
            fireBall.Deactivate();
        }
    }
}
