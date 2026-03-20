using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    public void UpdateAmmo(int currentAmmo, int maxAmmo, bool infiniteAmmo)
    {
        if (infiniteAmmo)
            ammoText.text = currentAmmo + " / ∞";
        else
            ammoText.text = currentAmmo + " / " + maxAmmo;
    }
}
