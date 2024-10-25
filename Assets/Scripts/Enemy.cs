using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    Animator anim;

    private Rigidbody enemyRb;
    private GameObject player;
    private SpawnManager spawnManager;

    public float knockBackPower = 1300;
    public float speed = 3f;
    public bool isGround = true;
    public float gameOverHeight = -5f;

    private bool fallEnemy = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spawnManager = FindObjectOfType<SpawnManager>();
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        knockBackPower = 1300;
        speed = 3f;
        isGround = true;
        gameOverHeight = -5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        //적이 플레이어를 향해 이동
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        //적이 플레이어를 향해 바라보도록 회전
        transform.LookAt(player.transform);
        if (transform.position.y < gameOverHeight)
        {
            fallEnemy = true;
            StartCoroutine(destroyEnemy());
        }
    }
    private void OnCollisionEnter(Collision collision) //물체 충돌시 호출
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Hit", true);
            StartCoroutine(ResetHitAnimation());

            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            //충돌한 객체의 rigidbody 컴포넌트 가져오기
            Vector3 awayFromEemey = (collision.gameObject.transform.position - transform.position);
            //충돌한 객체와 플레이어 간의 방향벡터 계산
            awayFromEemey.Normalize();//방향 벡터 정규화, 벡터의 크기를 1로 만들어 방향 정보 유지, 힘을 가할때 방향만 고려

            StartCoroutine(knockBack(playerRigidbody, awayFromEemey, knockBackPower));
        }
    }
    IEnumerator ResetHitAnimation()
    {
        yield return null;
        anim.SetBool("Hit", false);
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && fallEnemy)
        {
            anim.SetTrigger("Fall");
            isGround = false;
        }
    }
    IEnumerator destroyEnemy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
        spawnManager.EnemyDestroyed();
    }
    private IEnumerator knockBack(Rigidbody rb, Vector3 direction, float initialForce)
    {
        // 충돌 시 초기 힘을 줌
        rb.AddForce(direction * initialForce, ForceMode.Impulse);

        // 점진적으로 힘의 크기를 감소시키며 미끄러지는 효과를 구현
        float duration = 0.5f; // 미끄러지는 시간 설정
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // 시간에 따라 힘의 크기를 감소.
            float force = Mathf.Lerp(initialForce, 0f, elapsedTime / duration);
            rb.AddForce(direction * force * Time.deltaTime, ForceMode.VelocityChange);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
