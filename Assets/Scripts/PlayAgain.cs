using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{

    public static bool isRestart = false;



    public void restartGame()
    {
        //sahnelerin yeniden yüklenmesi veya bir sonraki sahneye geçi? i?lemleri "Scene Management" s?n?f? ile olur.
        //aktif olan sahnenin tekrar yüklenmesi için:

        SceneManager.LoadScene("Level1");
        isRestart = true;

    }

    public void quitGame()
    {
        //oyunu kapatmak için kullan?l?r, unity ekran?nda çal??maz, apk'da çal???r.
        Application.Quit();
    }
}
