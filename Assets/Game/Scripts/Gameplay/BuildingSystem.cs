using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
   /* public static BuildingSystem instance;
    public GridManager[] gridManager;
    //sound
    public static AudioSource audioSource;
    public AudioClip placeSound;
    public AudioClip removeSound;
    public AudioClip upgradeSound;
    public AudioClip missPlaceSound;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        gridManager = transform.GetChild(0).GetComponentsInChildren<GridManager>();
        audioSource = GetComponent<AudioSource>();
    }
    public void ShowGrid()
    {
        foreach (GridManager grid in gridManager)
        {
            grid.HideAndShowGrid(true);
        }
    }
    #region soundEfx
    public void PlayPlaceSound()
    {
        if (placeSound != null)
        {
            audioSource.PlayOneShot(placeSound);
        }
    }

    public void PlayRemoveSound()
    {
        if (removeSound != null)
        {
            audioSource.PlayOneShot(removeSound);
        }
    }

    public void PlayUpgradeSound()
    {
        if (upgradeSound != null)
        {
            audioSource.PlayOneShot(upgradeSound);
        }
    }
    public void PlayMissPlaceSound()
    {
        if (upgradeSound != null)
        {
            audioSource.PlayOneShot(missPlaceSound);
        }
    }
    #endregion*/
}
