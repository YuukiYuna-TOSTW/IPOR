using UnityEngine;
using TMPro;

public class RobotStepAnimator : MonoBehaviour
{
    private Animator animator;
    private int currentStep = 0;
    private int maxSteps = 5;

    [Header("UI Settings - Step 0 (Info & Achievements)")]
    public GameObject leftInfoBackground;
    public TextMeshProUGUI leftInfoText;
    public GameObject rightInfoBackground;
    public TextMeshProUGUI rightInfoText;

    [Header("UI Settings - Animation Steps (1-6)")]
    public GameObject textBackgroundObject;
    public TextMeshProUGUI infoText;

    [TextArea(2, 3)]
    public string[] stepDescriptions = new string[]
    {
        "",
        "Omniwheel HRC",
        "Chassis Atas HRC",
        "TT Motor HRC",
        "Chassis HRC",
        "Chassis Bawah HRC"
    };

    [Header("Step 0 Custom Content")]
    [TextArea(3, 5)]
    public string robotDescription = "Robot Transporter";

    [TextArea(3, 5)]
    public string robotAchievements = "Prestasi & Kompetisi:\n\nFinalis Lomba Robotik Pare-Pare\nInovasi Sistem Transporter Otomatis\nSistem Navigasi Omniwheel Presisi";

    [Header("Highlight Settings")]
    public Material highlightMaterial; // Satu material highlight yang sama untuk semuanya

    [System.Serializable]
    public class StepGroup
    {
        public Renderer[] renderersInStep;       // Daftar komponen di step ini
    }
    public StepGroup[] stepHighlightGroups;

    // Menyimpan material asli dari setiap komponen secara otomatis
    private Material[][] originalMaterials;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateStepText();
        SaveOriginalMaterials();
        ResetAllMaterials();
    }

    // Menyimpan material awal setiap komponen saat game mulai
    void SaveOriginalMaterials()
    {
        if (stepHighlightGroups == null) return;

        originalMaterials = new Material[stepHighlightGroups.Length][];
        for (int i = 0; i < stepHighlightGroups.Length; i++)
        {
            var group = stepHighlightGroups[i];
            if (group.renderersInStep != null)
            {
                originalMaterials[i] = new Material[group.renderersInStep.Length];
                for (int j = 0; j < group.renderersInStep.Length; j++)
                {
                    if (group.renderersInStep[j] != null)
                    {
                        // Menyimpan material asli bawaan objek tersebut
                        originalMaterials[i][j] = group.renderersInStep[j].sharedMaterial;
                    }
                }
            }
        }
    }

    void OnMouseDown()
    {
        currentStep++;
        if (currentStep > maxSteps)
        {
            currentStep = 0;
        }

        ApplyStepChanges();
    }

    public void GoToSpecificStep(int targetStep)
    {
        currentStep = Mathf.Clamp(targetStep, 0, maxSteps);
        ApplyStepChanges();
    }

    void ApplyStepChanges()
    {
        if (animator != null)
        {
            animator.SetInteger("AnimatorStep", currentStep);
        }

        UpdateStepText();
        UpdateHighlightMaterials();
    }

    void UpdateStepText()
    {
        if (leftInfoBackground != null) leftInfoBackground.SetActive(true);
        if (leftInfoText != null) leftInfoText.text = robotDescription;

        if (currentStep <= 0)
        {
            if (rightInfoBackground != null) rightInfoBackground.SetActive(true);
            if (rightInfoText != null) rightInfoText.text = robotAchievements;
            if (textBackgroundObject != null) textBackgroundObject.SetActive(false);
        }
        else
        {
            if (rightInfoBackground != null) rightInfoBackground.SetActive(false);
            if (textBackgroundObject != null) textBackgroundObject.SetActive(true);

            if (infoText != null && stepDescriptions.Length > currentStep)
            {
                infoText.text = stepDescriptions[currentStep];
            }
        }
    }

    void UpdateHighlightMaterials()
    {
        if (stepHighlightGroups == null || stepHighlightGroups.Length == 0) return;

        // Kembalikan semua komponen ke warna aslinya dulu
        ResetAllMaterials();

        // Beri highlight pada grup step yang aktif
        int groupIndex = currentStep - 1;
        if (groupIndex >= 0 && groupIndex < stepHighlightGroups.Length)
        {
            var group = stepHighlightGroups[groupIndex];
            if (group.renderersInStep != null)
            {
                foreach (var rend in group.renderersInStep)
                {
                    if (rend != null) rend.material = highlightMaterial;
                }
            }
        }
    }

    void ResetAllMaterials()
    {
        if (stepHighlightGroups == null || originalMaterials == null) return;

        for (int i = 0; i < stepHighlightGroups.Length; i++)
        {
            var group = stepHighlightGroups[i];
            if (group.renderersInStep != null && originalMaterials[i] != null)
            {
                for (int j = 0; j < group.renderersInStep.Length; j++)
                {
                    if (group.renderersInStep[j] != null && originalMaterials[i][j] != null)
                    {
                        // Kembalikan ke material asli masing-masing komponen
                        group.renderersInStep[j].material = originalMaterials[i][j];
                    }
                }
            }
        }
    }
}