using UnityEngine;
using UnityEngine.UI;

public class InspectableObject : MonoBehaviour
{
    [Header("Ustawienia interakcji")]
    public string infoText = "Naciśnij [E], aby zbadać obiekt.";
    public float lookDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Obiekty i efekty")]
    public GameObject infoCloud;     
    public Text infoCloudText;
    public AudioSource interactSound;

    [Header("Elementy prefabu (można przypisać ręcznie)")]
    public GameObject hiddenObject1;
    public GameObject movableObject2;

    [Space(10)]
    [Header("Ręczne przesunięcia (lokalne lub globalne)")]
    public Vector3 hiddenObject1Offset = new Vector3(0, 1, 0);
    public Vector3 movableObject2Offset = new Vector3(0, -1, 0);

    [Space(10)]
    [Header("Ustawienia ruchu")]
    public float moveSpeed = 2f;

    [Header("UI po zbadaniu")]
    public GameObject researchUIPanel;

    [Space(10)]
    [Header("Sterowanie graczem (do zablokowania po otwarciu UI)")]
    [Tooltip("Skrypt odpowiedzialny za poruszanie się gracza (np. PlayerMovement, FirstPersonController itp.)")]
    public MonoBehaviour playerMovementScript;

    [Tooltip("Skrypt odpowiedzialny za rozglądanie się myszką (jeśli jest oddzielny).")]
    public MonoBehaviour cameraLookScript;

    private bool isLookedAt = false;
    private bool isResearched = false;
    private bool isAnimating = false;
    private Transform playerCamera;

    private Vector3 hiddenObjStartPos;
    private Vector3 movableObjStartPos;

    private void Start()
    {
        playerCamera = Camera.main.transform;

        // Automatyczne wyszukiwanie obiektów 1 i 2, jeśli nie przypisano
        if (hiddenObject1 == null)
        {
            Transform found = transform.Find("Object1");
            if (found != null) hiddenObject1 = found.gameObject;
        }

        if (movableObject2 == null)
        {
            Transform found = transform.Find("Object2");
            if (found != null) movableObject2 = found.gameObject;
        }

        // Inicjalizacja
        if (infoCloud != null)
            infoCloud.SetActive(false);

        if (hiddenObject1 != null)
        {
            hiddenObjStartPos = hiddenObject1.transform.position;
            hiddenObject1.SetActive(false);
        }

        if (movableObject2 != null)
        {
            movableObjStartPos = movableObject2.transform.position;
            movableObject2.SetActive(false);
        }

        if (researchUIPanel != null)
            researchUIPanel.SetActive(false);

        Debug.Log($"[{name}] Stan początkowy: WAITING");
    }

    private void Update()
    {
        CheckLookAt();

        if (isLookedAt && Input.GetKeyDown(interactKey))
        {
            if (!isResearched && !isAnimating)
                StartCoroutine(ResearchSequence());
            else if (isResearched)
                ToggleResearchUI();
        }
    }

    private void CheckLookAt()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, lookDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isLookedAt)
                {
                    isLookedAt = true;
                    ShowInfoCloud(true);
                }
                return;
            }
        }

        if (isLookedAt)
        {
            isLookedAt = false;
            ShowInfoCloud(false);
        }
    }

    private void ShowInfoCloud(bool show)
    {
        if (infoCloud != null)
        {
            infoCloud.SetActive(show);
            if (show && infoCloudText != null)
                infoCloudText.text = infoText;
        }
    }

    private System.Collections.IEnumerator ResearchSequence()
    {
        isAnimating = true;
        Debug.Log($"[{name}] Stan: RESEARCHING");

        // Dźwięk
        if (interactSound != null)
            interactSound.Play();

        // Odkryj obiekty
        if (hiddenObject1 != null)
            hiddenObject1.SetActive(true);
        if (movableObject2 != null)
            movableObject2.SetActive(true);

        // Przesunięcia
        if (movableObject2 != null)
            yield return MoveObject(movableObject2.transform, movableObject2Offset);
        if (hiddenObject1 != null)
            yield return MoveObject(hiddenObject1.transform, hiddenObject1Offset);

        // Ukryj obiekt 1 po zakończeniu
        if (hiddenObject1 != null)
            hiddenObject1.SetActive(false);

        // Stan: researched
        isResearched = true;
        isAnimating = false;
        infoText = "Naciśnij [E], aby otworzyć panel badawczy.";

        Debug.Log($"[{name}] Stan: RESEARCHED");
    }

    private System.Collections.IEnumerator MoveObject(Transform obj, Vector3 offset)
    {
        Vector3 startPos = obj.position;
        Vector3 endPos = startPos + offset;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            obj.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }

    private void ToggleResearchUI()
    {
        if (researchUIPanel != null)
        {
            bool active = !researchUIPanel.activeSelf;
            researchUIPanel.SetActive(active);

            if (active)
            {
                Debug.Log($"[{name}] Otwieranie UI (stan: RESEARCHED)");
                LockPlayerControl(true);
            }
            else
            {
                Debug.Log($"[{name}] Zamknięto UI — reset do stanu WAITING");
                LockPlayerControl(false);
                ResetToWaitingState();
            }
        }
    }

    private void LockPlayerControl(bool uiOpen)
    {
        // Zatrzymaj ruch
        if (playerMovementScript != null)
            playerMovementScript.enabled = !uiOpen;

        if (cameraLookScript != null)
            cameraLookScript.enabled = !uiOpen;

        // Ustaw tryb kursora
        Cursor.visible = uiOpen;
        Cursor.lockState = uiOpen ? CursorLockMode.None : CursorLockMode.Locked;

        if (uiOpen)
            Debug.Log($"[{name}] Ruch gracza zablokowany, kursor aktywny.");
        else
            Debug.Log($"[{name}] Ruch gracza odblokowany, kursor ukryty.");
    }

    private void ResetToWaitingState()
    {
        // Cofnij pozycje obiektów i ukryj je
        if (hiddenObject1 != null)
        {
            hiddenObject1.transform.position = hiddenObjStartPos;
            hiddenObject1.SetActive(false);
        }

        if (movableObject2 != null)
        {
            movableObject2.transform.position = movableObjStartPos;
            movableObject2.SetActive(false);
        }

        // Przywróć stan
        isResearched = false;
        infoText = "Naciśnij [E], aby zbadać obiekt.";

        if (infoCloud != null && infoCloud.activeSelf && infoCloudText != null)
            infoCloudText.text = infoText;

        Debug.Log($"[{name}] Prefab zresetowany — stan: WAITING");
    }
}
