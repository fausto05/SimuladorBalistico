using UnityEngine;

public class ProjectileDataLogger : MonoBehaviour
{
    private float startTime;
    private Vector3 startPos;

    private bool hasCollided = false;

    private ShotManager _shotManager;
    private ShotUI _shotUI;
    private ProjectileLauncher _projectileLauncher;

    void Awake() 
    {
        _shotManager = FindFirstObjectByType<ShotManager>();
        _shotUI = FindFirstObjectByType<ShotUI>();
        _projectileLauncher = FindFirstObjectByType<ProjectileLauncher>();

        if (_shotManager == null)
        if (_shotUI == null)
        if (_projectileLauncher == null){}
    }

    void Start()
    {
        startTime = Time.time;
        startPos = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;
        hasCollided = true;

        float flightTime = Time.time - startTime;
        Vector3 impactPoint = collision.contacts[0].point;

        float distance = Vector3.Distance(startPos, impactPoint);

        int pieces = CountFallenPieces();

        bool hitTarget = collision.gameObject.CompareTag("Target");

        if (_projectileLauncher == null)
        {
            return;
        }

        ShotResult result = new ShotResult(
            _projectileLauncher.angleSlider.value,
            _projectileLauncher.forceSlider.value,
            _projectileLauncher.massDropdown.value + 1, 
            hitTarget,
            distance,
            pieces
        );

        if (_shotManager != null)
        {
            _shotManager.SaveShot(result, () => {
                if (_shotUI != null)
                {
                    _shotUI.ClearResultsUI();  
                    _shotUI.Invoke(nameof(_shotUI.ShowResults), 0.3f);
                }
            });
        }
    }

    int CountFallenPieces()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        int count = 0;
        foreach (GameObject t in targets)
        {
            if (t.transform.position.y < 0.5f)
            count++;
        }
        return count;
    }
}

