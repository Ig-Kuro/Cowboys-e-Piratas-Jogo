using UnityEngine;

public class PageManager : MonoBehaviour
{
    [Header("Arraste as páginas aqui, na ordem")]
    public GameObject[] pages;

    private int currentPage = 0;

    private void Start()
    {
        ShowPage(currentPage);
    }

    public void NextPage()
    {
        if (pages.Length == 0) return;

        currentPage++;
        if (currentPage >= pages.Length)
            currentPage = pages.Length - 1; // trava no fim

        ShowPage(currentPage);
    }

    public void PreviousPage()
    {
        if (pages.Length == 0) return;

        currentPage--;
        if (currentPage < 0)
            currentPage = 0; // trava no início

        ShowPage(currentPage);
    }

    public void ShowPage(int index)
    {
        if (pages.Length == 0 || index < 0 || index >= pages.Length)
            return;

        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(i == index);
    }
}
