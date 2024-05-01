using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class Chair : MonoBehaviour
{
    [SerializeField] private Transform sittingPos;
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private GameObject player;
    
    private Vector3 playerScale;
    private DynamicMoveProvider dynamicMoveProvider;
    private bool isPlayerInRange = false;
    private bool isPlayerSitting = false;

    private void Awake()
    {
        dynamicMoveProvider = player.GetComponent<DynamicMoveProvider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionText.gameObject.SetActive(true);
            interactionText.text = "Press [E] to " + (isPlayerSitting ? "Stand Up" : "Sit");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isPlayerSitting)
            {
                Standing();
                isPlayerSitting = false;
            }
            else
            {
                SitOnChair();
                isPlayerSitting = true;
            }
            interactionText.text = "Press [E] to " + (isPlayerSitting ? "Stand Up" : "Sit");
        }
    }
    
    private void SitOnChair()
    {
        player.transform.position = sittingPos.position;
        player.transform.localScale *= 0.5f;
        if (dynamicMoveProvider != null) dynamicMoveProvider.moveSpeed = isPlayerSitting ? 0f : 1f;
        Debug.Log("Player is now " + (isPlayerSitting ? "sitting" : "standing") + " on the chair.");
    }
    
    private void Standing()
    {
        player.transform.localScale = playerScale;
        if (dynamicMoveProvider != null) dynamicMoveProvider.moveSpeed = isPlayerSitting ? 1f : 0f;
        Debug.Log("Player is now " + (isPlayerSitting ? "standing" : "sitting") + ".");
    }
}
