using SFML.Window;
using SFML.Graphics;
using SFML.System;

class Program
{
    static void Main()
    {
        var window = new RenderWindow(new VideoMode(800, 600), "Cosmic Defenders");
        window.Closed += (_, __) => window.Close();
        window.SetFramerateLimit(60); 
        window.SetVerticalSyncEnabled(true);

        // Painting the Spacecraft
        var shape = new RectangleShape(new Vector2f(50, 50));
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
                shape.Position = new Vector2f(shape.Position.X - 10, shape.Position.Y);
            }
            else if (e.Code == Keyboard.Key.Right)
            {
                shape.Position = new Vector2f(shape.Position.X + 10, shape.Position.Y);
            }
        };

        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(Color.Black);
            window.Draw(shape);
            window.Display();
        }
    }
}
