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
            Debug.LogError("RespawnPoint �r inte tilldelad i Inspektorn.");
        }
    }

    private void HandleRespawnPlayer()
    {
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        // V�nta p� respawn-f�rdr�jningen
        yield return new WaitForSeconds(respawnDelay);

        // Aktivera spelaren f�rst
        player.gameObject.SetActive(true);

        // Inaktivera PlayerMovement och CharacterController tempor�rt
        playerMovement.enabled = false;
        characterController.enabled = false;

        // �terst�ll spelarens position och rotation
        player.transform.position = respawnPoint.position;
        player.transform.rotation = respawnPoint.rotation;

        // �teraktivera CharacterController och PlayerMovement
        characterController.enabled = true;
        playerMovement.enabled = true;

        // Eventuellt �terst�lla h�lsa eller andra statusv�rden
        Debug.Log("Spelaren har respawnat.");
    }
}
