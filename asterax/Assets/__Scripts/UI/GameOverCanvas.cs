using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverCanvas : MonoBehaviour
{
	[SerializeField] private UiTextModifier totalScore;
	[SerializeField] private float secondsDisplaying;
	[SerializeField] private float animationSpeed;

	private CanvasGroup _canvasGroup;
	private Func<IEnumerator> _showCanvas;
	private Func<IEnumerator> _hideCanvas;
	private WaitForSeconds _waitingTime;
	
	private void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		_canvasGroup.alpha = 0;
		_showCanvas = ShowCanvas;
		_hideCanvas = HideCanvas;
		_waitingTime = new WaitForSeconds(secondsDisplaying);
	}

	/// <summary>
	/// <para>
	/// Updates the total score text with the final score and start the coroutine to show the canvas
	/// </para>
	/// </summary>
	/// <param name="finalScore"></param>
	public void ShowGameOverCanvas(float finalScore)
	{
		totalScore.UpdateText(finalScore);
		StartCoroutine(_showCanvas());
	}

	/// <summary>
	/// <para>
	/// Increases the alpha of the canvas group every frame. Once the alpha hits 1 it starts the coroutine to hide the
	/// canvas.
	/// </para>
	/// </summary>
	/// <returns></returns>
	private IEnumerator ShowCanvas()
	{
		var startTime = Time.time;
		while (_canvasGroup.alpha < 0.99f)
		{
			_canvasGroup.alpha = Mathf.Lerp(0, 1, (Time.time - startTime) * animationSpeed);
			yield return null;
		}

		yield return StartCoroutine(_hideCanvas());
	}

	/// <summary>
	/// <para>
	/// Waits the time established by the variable secondsDisplaying and then it reloads the scene.
	/// </para>
	/// </summary>
	/// <returns></returns>
	private IEnumerator HideCanvas()
	{
		yield return _waitingTime;
		SceneManager.LoadScene(0);
	}
}

