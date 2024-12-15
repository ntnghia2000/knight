using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectTile : MonoBehaviour
{   
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    private bool hit;
    private bool isWall;
    private float direction;
    private float lifeTime = 3f;
    private BoxCollider2D collider;
    private Animator ani;

    private IObjectPool<ProjectTile> objectPool;
    public IObjectPool<ProjectTile> ObjectPool { set => objectPool = value; }

    private void Awake() {
        ani = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (hit) {
            return;
        }

        float moveSpeed = speed * Time.deltaTime * direction;
        transform.Translate(moveSpeed, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Monster" || col.tag == "Ground") {
            hit = true;
            collider.enabled = false;
            ani.SetTrigger("explote");
            if (col.tag == "Monster") {
                col.GetComponent<Monster>().TakeDamage(damage);
            }
        }
    }

    public void SetDirection(float dir) {
        lifeTime = 0;
        direction = dir;
        gameObject.SetActive(true);
        hit = false;
        collider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != dir) {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Deactivate() {
        StartCoroutine(DeactivateRoutine(lifeTime));
    }

    IEnumerator DeactivateRoutine(float delay) {
        yield return new WaitForSeconds(delay);
        objectPool.Release(this);
    }
}
