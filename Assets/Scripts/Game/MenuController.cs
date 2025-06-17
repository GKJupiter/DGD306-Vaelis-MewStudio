using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject firstSelectedButton; // Başlangıçta seçilecek buton

    private void OnEnable()
    {
        // Menü açıldığında ilk butonu seç
        EventSystem.current.SetSelectedGameObject(null); // temizle
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    private void Update()
    {
        // Eğer hiçbir şey seçili değilse, ilk butonu tekrar seç
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }

        // Geri tuşuna basıldığında (örnek: B tuşu - Xbox kolunda)
        if (Input.GetButtonDown("Cancel"))
        {
            GoBack();
        }
    }

    void GoBack()
    {
        Debug.Log("Geri tuşuna basıldı!");
        // Buraya geri gitme (menüyü kapatma vs.) işlemini yaz
    }
}
