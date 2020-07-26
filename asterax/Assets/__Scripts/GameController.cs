using System;
using __Scripts.Asteroids;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private Jumper player;
    [Header("UI")]
    [SerializeField] private UiTextModifier scoreText;
    [SerializeField] private UiTextModifier jumpsText;
    [SerializeField] private GameOverCanvas _gameOverCanvas;
    
    private float _score;

    private void Awake()
    {
        //asteroidSpawner.OnAsteroidDestroyed += AsteroidDestroyed;
        asteroidSpawner.OnAllAsteroidDestroyed += GameOver;
        player.OnJumpUsed += JumpUsed;
    }

    private void Start()
    {
        jumpsText.UpdateText(player.JumpsRemaining);
    }

    /// <summary>
    /// <para>
    /// If the asteroid has been destroyed by a bullet then it adds up the asteroid points and the total points
    /// </para><para>
    /// Updates the score text with the current score
    /// </para>
    /// <remarks>This method is executed every time an asteroid is destroyed</remarks>
    /// </summary>
    /// <param name="asteroid">The asteroid that has been destroyed</param>
    private void AsteroidDestroyed(Asteroid asteroid)
    {
        if (!asteroid.DestroyedByBullet) return;
        _score += asteroid.Points;
        scoreText.UpdateText(_score);
    }

    /// <summary>
    /// <para>If jumps remaining equals -1 then it calls the GameOver method, if not then it updates the jump remaining
    /// text</para>
    /// <remarks>This method is called every time the player ship is destroyed</remarks>
    /// </summary>
    /// <param name="jumpsRemaining"></param>
    private void JumpUsed(int jumpsRemaining)
    {
        if(jumpsRemaining == -1) GameOver();
        else jumpsText.UpdateText(jumpsRemaining);
    }

    /// <summary>
    /// <para>
    /// Shows the GameOverCanvas with the final score
    /// </para>
    /// </summary>
    private void GameOver()
    {
        _gameOverCanvas.ShowGameOverCanvas(_score);
    }
}
