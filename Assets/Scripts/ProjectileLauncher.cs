using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectileLauncher : MonoBehaviour
{
    [Header("UI References")] 
    public Slider angleSlider; 
    public Slider yawSlider; 
    public Slider forceSlider; 
    public TMP_Dropdown massDropdown; 
    public Button shootButton; 
    
    [Header("Projectile Settings")] 
    public GameObject projectilePrefab; 
    public Transform spawnPoint; 
    
    private GameObject currentProjectile; 
    
    private Rigidbody rb; 
    private float angle; 
    private float force; 
    private float mass;

    void Start() 
    { 
        shootButton.onClick.AddListener(Shoot); 
    }

    void Shoot()
    { 
        // Valores de UI
        float angle = angleSlider.value; 
        float yaw = yawSlider.value; 
        float force = forceSlider.value; 
        float mass = massDropdown.value + 1; 
        
        currentProjectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation); 
        Destroy(currentProjectile, 5f); 
        
        rb = currentProjectile.GetComponent<Rigidbody>(); 
        
        rb.mass = mass; 
        
        Vector3 baseDir = spawnPoint.forward; 
        
        Quaternion yawRotation = Quaternion.AngleAxis(yaw, Vector3.up); 
        
        Quaternion pitchRotation = Quaternion.AngleAxis(-angle, spawnPoint.right); 
        
        Vector3 dir = yawRotation * (pitchRotation * baseDir); 
        
        rb.AddForce(dir * force, ForceMode.Impulse); 
    }
}