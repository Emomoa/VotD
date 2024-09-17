using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject player;
    public float respawnDelay = 2f;

    private PlayerMovement playerMovement;
    private CharacterController characterController;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerDeath += HandleRespawnPlayer;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerDeath -= HandleRespawnPlayer;
    }

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        playerMovement = player.GetComponent<PlayerMovement>();
        characterController = player.GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (respawnPoint == null)
        {
            Debug.LogError("RespawnPoint är inte tilldelad i Inspektorn.");
        }
    }

    private void HandleRespawnPlayer()
    {
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        // Vänta på respawn-fördröjningen
        yield return new WaitForSeconds(respawnDelay);

        // Aktivera spelaren först
        player.gameObject.SetActive(true);

        // Inaktivera PlayerMovement och CharacterController temporärt
        playerMovement.enabled = false;
        characterController.enabled = false;

        // Återställ spelarens position och rotation
        player.transform.position = respawnPoint.position;
        player.transform.rotation = respawnPoint.rotation;

        // Återaktivera CharacterController och PlayerMovement
        characterController.enabled = true;
        playerMovement.enabled = true;

        // Eventuellt återställa hälsa eller andra statusvärden
        Debug.Log("Spelaren har respawnat.");
    }
}
