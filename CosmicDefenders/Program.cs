using System.Numerics;
using CosmicDefenders;
using CosmicDefenders.Entities.Player;
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

        GameState gameState = new GameState();
        gameState.Run();
    }

    
}
