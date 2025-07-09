using UnityEngine;
using DG.Tweening; // Thư viện DOTween

public class CameraZoomTrigger : MonoBehaviour
{
    public Camera mainCamera; // Camera chính
    public Camera secondaryCamera; // Camera phụ
    public float transitionDuration = 1f; // Thời gian chuyển đổi

    private Vector3 initialPosition; // Vị trí ban đầu của camera chính
    private Quaternion initialRotation; // Góc nhìn ban đầu của camera chính
    private float initialFOV; // FOV ban đầu của camera chính

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Tự động lấy camera chính nếu chưa gán
        }

        // Lưu trạng thái ban đầu của camera chính
        initialPosition = mainCamera.transform.position;
        initialRotation = mainCamera.transform.rotation;
        initialFOV = mainCamera.fieldOfView;
    }

    public void StartTransition()
    {
        // Lấy trạng thái từ camera phụ
        Vector3 targetPosition = secondaryCamera.transform.position;
        Quaternion targetRotation = secondaryCamera.transform.rotation;
        float targetFOV = secondaryCamera.fieldOfView;

        // Tween vị trí
        mainCamera.transform.DOMove(targetPosition, transitionDuration)
            .SetEase(Ease.InOutSine);

        // Tween góc nhìn
        mainCamera.transform.DORotateQuaternion(targetRotation, transitionDuration)
            .SetEase(Ease.InOutSine);

        // Tween FOV
        mainCamera.DOFieldOfView(targetFOV, transitionDuration)
            .SetEase(Ease.InOutSine);
    }

    public void ResetToMainCamera()
    {
        // Tween trở lại vị trí ban đầu
        mainCamera.transform.DOMove(initialPosition, transitionDuration)
            .SetEase(Ease.InOutSine);

        // Tween trở lại góc nhìn ban đầu
        mainCamera.transform.DORotateQuaternion(initialRotation, transitionDuration)
            .SetEase(Ease.InOutSine);

        // Tween trở lại FOV ban đầu
        mainCamera.DOFieldOfView(initialFOV, transitionDuration)
            .SetEase(Ease.InOutSine);
    }
}
