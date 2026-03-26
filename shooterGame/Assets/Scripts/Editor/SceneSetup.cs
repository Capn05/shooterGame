using UnityEngine;
using UnityEditor;

public class SceneSetup : EditorWindow
{
    [MenuItem("ShooterGame/Setup Scene")]
    public static void SetupScene()
    {
        if (!EditorUtility.DisplayDialog("Setup Scene",
            "This will create the Player, Ground, and wire up all scripts. Continue?",
            "Yes", "Cancel"))
            return;

        // Ensure Ground layer exists
        SetupLayer(6, "Ground");

        // Create Ground
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(10, 1, 10);
        ground.layer = LayerMask.NameToLayer("Ground");

        // Create Player
        GameObject player = new GameObject("Player");
        player.transform.position = new Vector3(0, 1, 0);

        CharacterController cc = player.AddComponent<CharacterController>();
        cc.center = new Vector3(0, 1, 0);
        cc.height = 2f;

        // Setup Camera
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            mainCam = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();
        }

        mainCam.transform.SetParent(player.transform);
        mainCam.transform.localPosition = new Vector3(0, 1.7f, 0);
        mainCam.transform.localRotation = Quaternion.identity;

        // Create GroundCheck
        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.SetParent(player.transform);
        groundCheck.transform.localPosition = Vector3.zero;

        // Attach PlayerMovement
        PlayerMovement movement = player.AddComponent<PlayerMovement>();
        movement.groundCheck = groundCheck.transform;
        movement.groundMask = LayerMask.GetMask("Ground");

        // Attach PlayerHealth
        PlayerHealth health = player.AddComponent<PlayerHealth>();

        // Attach MouseLook to camera
        MouseLook mouseLook = mainCam.gameObject.AddComponent<MouseLook>();
        mouseLook.playerBody = player.transform;

        // Attach WeaponSystem to camera
        WeaponSystem weapon = mainCam.gameObject.AddComponent<WeaponSystem>();
        weapon.playerCamera = mainCam;

        // Attach WeaponHUD to player
        WeaponHUD hud = player.AddComponent<WeaponHUD>();
        hud.weapon = weapon;
        hud.playerHealth = health;

        // Mark scene as dirty so it can be saved
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        Debug.Log("Scene setup complete! Press Play to test.");
    }

    static void SetupLayer(int layerIndex, string layerName)
    {
        SerializedObject tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");
        SerializedProperty layer = layers.GetArrayElementAtIndex(layerIndex);

        if (layer.stringValue != layerName)
        {
            layer.stringValue = layerName;
            tagManager.ApplyModifiedProperties();
        }
    }
}
