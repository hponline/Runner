using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtyParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;


    public float jumpForce = 650;

    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // isOnGround nesnemiz True oldugu icin burasi calisir.
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && gameOver !=true)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            // Tekrar ziplamamasi icin false yapiyoruz.

            // Karaktermizin zýplayabilmesi icin Animasyondaki degiskeni aliyoruz.
            playerAnim.SetTrigger("Jump_trig");
            dirtyParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
        
    }

    // Collider parcacik carpistiricilarimiz birbiriyle etkilesime girdigi zaman Boolen nesnemiz True olacak
    private void OnCollisionEnter(Collision collision)
    {
        // player zeminde ise
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtyParticle.Play();
        }
        // player engellere carparsa
        else if(collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            gameOver = true;

            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtyParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            
            // komutu cagirir ve delay - repeat verir
            Invoke("LoadScreen", 3f);
            
        }

    }
    // Ekran Yukleme
    public void LoadScreen()
    {
        SceneManager.LoadScene(0);
    }
}
