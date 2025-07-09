using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public enum ActionName
{   
    None,
    ZoomCamera,
    UnZoomCamera,
    SpawnEnemy,
    ShowTutorialPanel,
    HideTutorialPanel,
    CustomAction
}

public class ActionManager : MonoBehaviour
{
   
    [SerializeField] private CameraZoomTrigger cameraZoom;

    [SerializeField] private Transform mainCanvas;

    [Header("Spawn")]
    [SerializeField] private EnemySpawnPoint spawnPoint;
    [SerializeField] private WaveDataSO enemyWaveForTuto;

    public void PerformAction(ActionName actionName)
    {
        switch (actionName)
        {
            case ActionName.ZoomCamera:
                ZoomCamera();
                break;
            case ActionName.UnZoomCamera:
                UnZoomCamera();
                break;
            case ActionName.SpawnEnemy:
                SpawnEnemy();
                break;
            case ActionName.ShowTutorialPanel:
                ShowTutorialPanel();
                break;
            case ActionName.HideTutorialPanel:
                HideTutorialPanel();
                break;
            case ActionName.CustomAction:
                CustomAction();
                break;
            default:
                Debug.LogWarning($"Action '{actionName}' is not defined!");
                break;
        }
    }
    private void ZoomCamera()
    {
        if (cameraZoom == null)
            return;
        cameraZoom.StartTransition();
    }

    private void UnZoomCamera()
    {
        if (cameraZoom == null)
            return;
        cameraZoom.ResetToMainCamera();
    }

    private void SpawnEnemy()
    {

        StartCoroutine(spawnPoint.SpawnEnemyTutorial(enemyWaveForTuto));
    }

    private void ShowTutorialPanel()
    {
        mainCanvas.gameObject.SetActive(true);
    }
    private void HideTutorialPanel()
    {
        mainCanvas.gameObject.SetActive(false);
    }
    private void CustomAction()
    {

    }
}
