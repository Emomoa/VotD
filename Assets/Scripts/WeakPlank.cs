using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WeakPlank : MonoBehaviour
{
    private bool isBroken = false;
    public GameObject player;
    private Rigidbody playerRB;
    private PlayerMovement playerMove;
    public AudioSource audioSource;
    public AudioClip[] plankBreak;
    private void Start()
    {
        playerMove = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        BreakPlank();
    }

    // Checks if player is sneaking and allows the player to walk a little while before breaking plank.
    private async void BreakPlank()
    {
        string groundTag = playerMove.GetGroundTag();
        if (groundTag == "WeakPlank")
        {
            if (!playerMove.isSneaking)
            {
                await Task.Delay(1500);

                if (playerMove.isSneaking)
                {
                    return;
                }
                else
                {
                    isBroken = true;
                    // Play sound and animation for broken plank
                    if (audioSource.isPlaying == false)
                    {
                        playerMove.isDead = true;
                        audioSource.Play();
                        await Task.Delay(1500);
                        if (!playerMove.isDead)
                        {
                            Debug.Log("The plank has broken!");
                            Scene currentScene = SceneManager.GetActiveScene();
                            playerMove.isDead = false;
                            SceneManager.LoadScene(currentScene.name);
                            //playerMove.Die();
                        }

                        //gameObject.SetActive(false);
                    }


                }
            }
        }
    }
}
