using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    CapsuleCollider cc;

    IEnumerator waltAttackCountCoroutine;
    public Coroutine attackCoroutine;

    public int attackCount = 1;
    public float speed = 10f;
    public float knockBackPower = 1300f;

    public bool isGround = true;
    public bool gameOver = false;
    public bool powerUp = false;
    public bool gameStart = false;
    public bool speedUp = false;
    public float gameOverHeight = -6f;

    public Image skillCoolTime;

    public AudioClip attackSound;
    public AudioClip skillAttackSound;
    public AudioClip skillVoice;
    public AudioClip speedUpVoice;

    private bool isPaused = false;

    private AudioSource playerAudio;
    private SpawnManager spawnManager;
    private Enemy enemy;
    private Timer timer;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        playerAudio = GetComponent<AudioSource>();
        spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        enemy = GameObject.FindObjectOfType<Enemy>();
        timer = GameObject.FindObjectOfType<Timer>();

        attackCount = 1;
        speed = 10f;
        knockBackPower = 1300;

        isGround = true;
        gameOver = false;
        powerUp = false;
        gameStart = false;
        gameOverHeight = -6f;

        isPaused = false;
        speedUp = false;

        Time.timeScale = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameStart || isPaused)
            return;

        if (transform.position.y < gameOverHeight)
        {
            gameOver = true;
            StartCoroutine(GameOverDelay());
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            Run();
            Rotate(h, v);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        if (speedUp)
            StartCoroutine(SpeedUp());
        else if (!speedUp)
            speed = 10f;

        Emotion();
        Attack();
    }
    void Run()
    {
        anim.SetBool("Run", true);
        transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime);
    }
    void Rotate(float h, float v)
    {
        Vector3 dir = new Vector3(h, 0, v).normalized;
        //오일러 각을 이용해서 y축으로 회전할 각도를 구해서 회전
        transform.eulerAngles = new Vector3(0, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0);

        Quaternion rot = Quaternion.identity; // Quaternion 값을 저장할 변수 선언 및 초기화

        rot.eulerAngles = new Vector3(0, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0); // 역시 eulerAngles를 이용한 오일러 각도를 Quaternion으로 저장

        transform.rotation = rot; // 그 각도로 회전
    }
    void Emotion()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            anim.SetBool("Ang", true);
        }
        else if(Input.GetKey(KeyCode.Alpha2))
        {
            anim.SetBool("No", true);
        }
        else if(Input.GetKey(KeyCode.Alpha3))
        {
            anim.SetBool("Yes", true);
        }
        else if(Input.GetKey(KeyCode.Alpha4))
        {
            anim.SetBool("Thanks", true);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            anim.SetBool("Bye", true);
        }
        else
        {
            anim.SetBool("Ang",false);
            anim.SetBool("No", false);
            anim.SetBool("Yes", false);
            anim.SetBool("Thanks", false);
            anim.SetBool("Bye", false);
        }
    }
    public void Attack()
    {
        if (Input.GetKey(KeyCode.Space) && attackCount == 1)
        {
            StartCoroutine(AttackCoroutine());
            if (waltAttackCountCoroutine == null) // waltAttackCount 코루틴이 실행 중이지 않을 때만 실행
            {
                playerAudio.PlayOneShot(skillVoice, 1);
                StartCoroutine(CoolTime(4.4f));
                waltAttackCountCoroutine = WaltAttackCount();
                StartCoroutine(waltAttackCountCoroutine);
            }
        }
    }
    public IEnumerator AttackCoroutine()
    {
        anim.SetBool("Attack", true);
        attackCount = 0;
        yield return new WaitForSeconds(0.2f);
        powerUp = true;
        if (speedUp)
            knockBackPower = 2800f;
        else
            knockBackPower = 2300f;
        StartCoroutine(NonPowerUp());
    }
    IEnumerator NonPowerUp()
    {
        yield return new WaitForSeconds(0.6f);
        anim.SetBool("Attack", false);
        powerUp = false;
        knockBackPower = 1300;
    }
    IEnumerator WaltAttackCount()
    {
        yield return new WaitForSeconds(4.5f);
        attackCount = 1;
        waltAttackCountCoroutine = null;// 코루틴이 끝나면 변수를 null로 초기화하여 다시 실행할 수 있게 함
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("SpeedUp"))
        {
            playerAudio.PlayOneShot(speedUpVoice, 1f);
            speedUp = true;
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision) //물체 충돌시 호출
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            gameOver = false;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            //충돌한 객체의 rigidbody 컴포넌트 가져오기
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            //충돌한 객체와 플레이어 간의 방향벡터 계산
            awayFromPlayer.Normalize();//방향 벡터 정규화, 벡터의 크기를 1로 만들어 방향 정보 유지, 힘을 가할때 방향만 고려

            StartCoroutine(knockBack(enemyRigidbody, awayFromPlayer, knockBackPower));

            if (powerUp)
            {
                playerAudio.PlayOneShot(skillAttackSound, 1f);
            }
            else
            {
                anim.SetBool("Hit", true);
                playerAudio.PlayOneShot(attackSound, 1f);
                StartCoroutine(ResetHitAnimation());
            }
        }
    }
    IEnumerator SpeedUp()
    {
        speed = 20f;
        yield return new WaitForSeconds(5f);
        speedUp = false;
    }
    IEnumerator ResetHitAnimation()
    {
        yield return null;
        anim.SetBool("Hit",false );
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && gameOver)
        {
            anim.SetTrigger("Fall");
            isGround = false;
        }
    }
    IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(0.3f); //0.3 지연

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        DestoryPlayer();
        gameOver = true;
        Time.timeScale = 0f;
        timer.text_timer.gameObject.SetActive(false);
        spawnManager.text_wave.gameObject.SetActive(false);
        spawnManager.soundManager.gameObject.SetActive(false);
        spawnManager.gameOverCanvas.gameObject.SetActive(true);
    }
    private IEnumerator knockBack(Rigidbody rb, Vector3 direction, float initialForce)
    {
        if(rb == null) yield break;

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
    IEnumerator CoolTime(float cool)
    {

        float elapsedTime = 0f;
        float originalCool = cool;

        while (elapsedTime < originalCool)
        {
            elapsedTime += Time.deltaTime;
            skillCoolTime.fillAmount = elapsedTime / originalCool;
            yield return new WaitForFixedUpdate();
        }
        skillCoolTime.fillAmount = 1;
    }
    public void DestoryPlayer()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
