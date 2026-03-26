using UnityEngine;

public class WeaponHUD : MonoBehaviour
{
    public WeaponSystem weapon;
    public PlayerHealth playerHealth;

    void OnGUI()
    {
        if (weapon == null || playerHealth == null)
            return;

        // Crosshair
        float crosshairSize = 12f;
        float centerX = Screen.width / 2f;
        float centerY = Screen.height / 2f;
        GUI.color = Color.white;
        GUI.Label(new Rect(centerX - crosshairSize / 2f, centerY - crosshairSize / 2f, crosshairSize, crosshairSize), "+");

        // Ammo display (bottom right)
        GUIStyle ammoStyle = new GUIStyle(GUI.skin.label);
        ammoStyle.fontSize = 24;
        ammoStyle.fontStyle = FontStyle.Bold;
        ammoStyle.normal.textColor = Color.white;

        string ammoText = $"{weapon.currentAmmo} / {weapon.maxAmmo}";
        GUI.Label(new Rect(Screen.width - 200, Screen.height - 60, 180, 40), ammoText, ammoStyle);

        // Health display (bottom left)
        GUIStyle healthStyle = new GUIStyle(GUI.skin.label);
        healthStyle.fontSize = 24;
        healthStyle.fontStyle = FontStyle.Bold;
        healthStyle.normal.textColor = playerHealth.currentHealth > 30 ? Color.green : Color.red;

        string healthText = $"HP: {playerHealth.currentHealth}";
        GUI.Label(new Rect(20, Screen.height - 60, 180, 40), healthText, healthStyle);
    }
}
