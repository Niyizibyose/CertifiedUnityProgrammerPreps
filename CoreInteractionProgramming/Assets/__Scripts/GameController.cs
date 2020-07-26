using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private float WAITTIME = 2;

    public GameObject[] PlayerShip;
    public EmptySpaceFinder locator;

    public GameObject GameOverUI;

    // Start TimerCoroutine here to make the TimeCoroutine only dependent on Game Controller object instead of the PlayerShip
    public void StartCoroutine(float waitTime)
    {
        StartCoroutine(TimerCoroutine(waitTime));
    }

    public IEnumerator TimerCoroutine(float waitTime)
    {
        Debug.Log("Coroutine");

        print("Starting " + Time.time);

        GameOverUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        GameOverUI.gameObject.SetActive(false);

        print("Done " + Time.time);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator JumpCoroutine(float waitTime)
    {
        Debug.Log("Coroutine");

        print("Starting " + Time.time);

        PlayerShip[0].gameObject.SetActive(false);
        PlayerShip[1].gameObject.SetActive(false);

        yield return new WaitForSeconds(waitTime);

        PlayerShip[0].gameObject.SetActive(true);
        PlayerShip[1].gameObject.SetActive(true);

        print("Done " + Time.time);

        locator.FindEmptySpace();

    }


}
