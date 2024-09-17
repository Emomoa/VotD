using System.Threading.Tasks;
using UnityEngine;

public class WeakPlank : MonoBehaviour
{
    private bool isBroken = false;
    [SerializeField]
    private PlayerMovement playerMove;
    public AudioSource audioSource;
    public AudioClip[] plankBreak;

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
                        audioSource.Play();
                        Debug.Log("The plank has broken!");
                    }


                }
            }
        }
    }
}