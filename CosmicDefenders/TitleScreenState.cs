using CosmicDefenders.Enums;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CosmicDefenders;

internal class TitleScreenState : IGameState
{
    Font _font;
    Text _titleText;
    List<Button> _menuButtons = new List<Button>();
    Cursor _cursorArrow;
    Cursor _cursorHand;
    TitleScreenOption _optionSelected = TitleScreenOption.None;

    public TitleScreenState(int windowWidth)
    {
        _font = new Font("Assets/SairaStencilOne-Regular.ttf");
        InitializeTitle(windowWidth);
        InitializeMenuButtons(windowWidth);

        _cursorArrow = new(Cursor.CursorType.Arrow);
        _cursorHand = new(Cursor.CursorType.Hand);
    }

    private void InitializeTitle(int width)
    {
        _titleText = new Text($"COSMIC DEFENDERS", _font, 40);
        _titleText.FillColor = Color.White;
        long titleX = width / 2;
        _titleText.Origin = new Vector2f(_titleText.GetGlobalBounds().Width / 2, _titleText.GetGlobalBounds().Height / 2);
        _titleText.Position = new Vector2f(titleX, 150);
    }

    private void InitializeMenuButtons(int width)
    {
        InitializeMenuButton(width, 250, "PLAY", TitleScreenOption.Play);
        InitializeMenuButton(width, 280, "SCORES", TitleScreenOption.Scores);
        InitializeMenuButton(width, 310, "EXIT", TitleScreenOption.Exit);
    }

    public void Clear(RenderWindow window)
    {
        window.Clear(Color.Black);
    }

    public void Draw(RenderWindow window)
    {
        DrawTitleText(window);
        DrawMenuItems(window);
        window.Display();
    }

    private void DrawTitleText(RenderWindow window)
    {
        window.Draw(_titleText);
    }

    private void DrawMenuItems(RenderWindow window)
    {
        foreach (var button in _menuButtons)
        {
            button.Draw(window);
        }
    }

    private void InitializeMenuButton(int windowWidth, int y, string text, TitleScreenOption option)
    {
        Action onClick = null;
        
        switch (option)
        {
            case TitleScreenOption.Play:
                onClick = () => _optionSelected = TitleScreenOption.Play;
                break;
            case TitleScreenOption.Scores:
                onClick = () => _optionSelected = TitleScreenOption.Scores;
                break;
            case TitleScreenOption.Exit:
                onClick = () => _optionSelected = TitleScreenOption.Exit;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Button menuItem = new Button(_font, 20, text, windowWidth / 2, y, onClick);

        _menuButtons.Add(menuItem);
    }


    public void Update(RenderWindow window)
    {
        window.DispatchEvents();


        UpdateButtons(window);
    }

    private void UpdateButtons(RenderWindow window)
    {
        bool isAnyButtonHovered = false;
        foreach (var button in _menuButtons)
        {
            if (button.IsMouseOver(window))
            {
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                    button.OnClick?.Invoke();
                isAnyButtonHovered = true;
            }
        }

        if (isAnyButtonHovered)
        {
            window.SetMouseCursor(_cursorHand);
        }
        else
        {
            window.SetMouseCursor(_cursorArrow);
        }
    }

    public int GetState()
    {
        return _optionSelected switch
        {
            TitleScreenOption.Play => Constants.States.GAME_SCREEN,
            TitleScreenOption.Scores => Constants.States.SCORE_SCREEN,
            TitleScreenOption.Exit => Constants.States.EXIT,
            _ => Constants.States.TITLE_SCREEN,
        };
    }
}