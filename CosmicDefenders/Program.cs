using System.Numerics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

class Program
{
    static void Main()
    {
        RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Cosmic Defenders");
        window.Closed += (_, __) => window.Close();
        window.SetFramerateLimit(60); 
        window.SetVerticalSyncEnabled(true);

        // Painting the Spacecraft
        Texture texture = new (Path.Combine("Assets", "DurrrSpaceShip.png"));
        Sprite spaceShip = new (texture);
        spaceShip.Position = new Vector2f(375, 500);


        RectangleShape shape = new(new Vector2f(50, 50));
        shape.FillColor = Color.Green;
        shape.Position = new Vector2f(375, 500);
        

        // Subscription to keyboard events
        window.KeyPressed += (sender, e) =>
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
            else if (e.Code == Keyboard.Key.Left)
            {
                spaceShip.Position = new Vector2f(spaceShip.Position.X - 10, spaceShip.Position.Y);
            }
            else if (e.Code == Keyboard.Key.Right)
            {
                spaceShip.Position = new Vector2f(spaceShip.Position.X + 10, spaceShip.Position.Y);
            }
        };

        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(Color.Black);
            window.Draw(spaceShip);
            var debugRect = GetDebugRectangle(spaceShip);
            window.Draw(debugRect);
            window.Display();
        }
    }

    static RectangleShape GetDebugRectangle(Sprite sprite)
    {
        var bounds = sprite.GetGlobalBounds();
        var debugRect = new RectangleShape(new Vector2f(bounds.Width, bounds.Height));
        debugRect.Position = new Vector2f(bounds.Left, bounds.Top);
        debugRect.OutlineColor = Color.Red;
        debugRect.OutlineThickness = 1;
        debugRect.FillColor = Color.Transparent;
        return debugRect;
    }
}
