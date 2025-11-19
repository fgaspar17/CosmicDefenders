using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CosmicDefenders;

internal class Button
{
    Text _textItem;
    RectangleShape _background;
    public Action? OnClick;

    public Button(Font font, uint fontSize, string text, long x, long y, Action? onClick)
    {
        _textItem = new Text($"{text}", font, fontSize);
        FloatRect bounds = _textItem.GetLocalBounds();
        _textItem.FillColor = Color.White;
        //long textX = window.Size.X / 2;
        _textItem.Origin = new Vector2f(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
        _textItem.Position = new Vector2f(x, y);

        _background = new RectangleShape(new Vector2f(bounds.Width + 5, bounds.Height + 5));
        _background.FillColor = Color.Transparent;
        //_background.OutlineColor = Color.White;
        //_background.OutlineThickness = 2;
        _background.Origin = new Vector2f(_background.Size.X / 2, _background.Size.Y / 2);
        _background.Position = new Vector2f(_textItem.Position.X, _textItem.Position.Y);

        OnClick = onClick;
    }

    public bool IsMouseOver(RenderWindow window)
    {
        bool isMouseOver = false;
        Vector2i mousePoint = Mouse.GetPosition(window);
        Vector2f mousePointF = new Vector2f(mousePoint.X, mousePoint.Y);
        var rect = _background.GetGlobalBounds();
        if (rect.Contains(mousePointF))
        {
            isMouseOver = true;
        }

        return isMouseOver;
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(_background);
        window.Draw(_textItem);
    }
}