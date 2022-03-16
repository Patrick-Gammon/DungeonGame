using System;
using SplashKitSDK;

// Program executes general code
public class Program
{
    public static void Main()
    {
        // Create a new game
        Window gameWindow = new Window("Dungeon Game", 800, 600);
        DungeonGame dungeonGame = new DungeonGame(gameWindow);

        // Run the dungeon events
        do{
            SplashKit.ProcessEvents();
            dungeonGame.Draw();
            dungeonGame.HandleInput();
            dungeonGame.TurnEvents();
        } while (dungeonGame.quit == true);

        gameWindow.Close();
        gameWindow = null;
    }
}