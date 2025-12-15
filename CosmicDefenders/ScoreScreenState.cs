using System.Text;
using System.Threading;
using CosmicDefenders.Constants;
using CosmicDefenders.Helpers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CosmicDefenders;

internal class ScoreScreenState : IGameState
{
    Font _font;
    Font _fontPressEnter;
    Text _titleText;
    Text _scoresText;
    Text _pressEnterText;
    bool _nextScreen = false;
    Clock _clock;

    public ScoreScreenState(int windowWidth)
    {
        _font = new Font("Assets/SairaStencilOne-Regular.ttf");
        _fontPressEnter = new Font("Assets/Doto-Black.ttf");
        _clock = new Clock();
        InitializeTitleText(windowWidth);
        InitializeScoresText(windowWidth);
        InitializePressAnyButtonText(windowWidth);
    }

    private void InitializeTitleText(int width)
    {
        _titleText = new Text("SCORES", _font, 40);
        _titleText.FillColor = Color.White;
        long x = width / 2;
        _titleText.Origin = new Vector2f(_titleText.GetGlobalBounds().Width / 2, _titleText.GetGlobalBounds().Height / 2);
        _titleText.Position = new Vector2f(x, 100);
    }

    private void InitializePressAnyButtonText(int width)
    {
        _pressEnterText = new Text("PRESS ENTER TO CONTINUE", _fontPressEnter, 15);
        _pressEnterText.FillColor = Color.White;
        long x = width / 2;
        _pressEnterText.Origin = new Vector2f(_pressEnterText.GetGlobalBounds().Width / 2, _pressEnterText.GetGlobalBounds().Height / 2);
        _pressEnterText.Position = new Vector2f(x, 500);
    }

    private void InitializeScoresText(int width)
    {
        List<int> scores = ScoresHelper.GetScores();
        StringBuilder scoresSb = new StringBuilder();
        if (scores.Count > 0)
        {
            for (int i = 0; i < scores.Count; i++)
            {
                scoresSb.AppendLine($"{i + 1}. {scores[i]}");
            }
        }
        else
        {
            scoresSb.Append("NO SCORE");
        }
        _scoresText = new Text(scoresSb.ToString(), _font, 15);
        _scoresText.FillColor = Color.White;
        long x = width / 2;
        _scoresText.Origin = new Vector2f(_scoresText.GetGlobalBounds().Width / 2, _scoresText.GetGlobalBounds().Height / 2);
        _scoresText.Position = new Vector2f(x, 300);
    }

    public void Clear(RenderWindow window)
    {
        window.Clear(Color.Black);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(_titleText);
        window.Draw(_scoresText);
        if (_clock.ElapsedTime.AsSeconds() % 1 < 0.5f)
            window.Draw(_pressEnterText);
        window.Display();
    }

    public void Update(RenderWindow window)
    {
        window.DispatchEvents();
        if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
        {
            _nextScreen = true;
        }
    }

    public int GetState()
    {
        return _nextScreen ? States.TITLE_SCREEN : States.SCORE_SCREEN;
    }
}