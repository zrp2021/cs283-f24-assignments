using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class Game
{
    private Fish player;
    private List<Fish> fishes;
    public void Setup()
    {
        // make player
        player = new Fish(1);
        fishes = new List<Fish>();
        gameOver = false;

    }

    public void Update(float dt)
    {
        CheckTouches();

        // if the player died, make the other fish scurry off screen
        if (!player.isAlive) {
            foreach (Fish f in fishes) {
                if (!f.isSpedUp && f.isAlive) {
                    f.dx *= 5;
                    f.dy *= 5;
                    f.isSpedUp = true;
                }
                f.Update(dt);
            }
        }
        else {
            player.Update(dt);
            foreach (Fish f in fishes) {
                f.Update(dt);
            }

            // 1 in 10 chance of adding a new fish
            if (RandomRange(0, 10) == 0) {
            fishes.Add(new Fish());
            }
        }
        
        // remove fish not on screen
        fishes.RemoveAll(fish => !fish.IsOnScreen());
    }

    public void Draw(Graphics g)
    {
        player.Draw(Graphics g);
        foreach (Fish f in fishes) {
            f.Draw(Graphics g);
        }
    }

    public void MouseClick(MouseEventArgs mouse)
    {
        if (mouse.Button == MouseButtons.Left)
        {
            System.Console.WriteLine(mouse.Location.X + ", " + mouse.Location.Y);
        }
    }

    public void KeyDown(KeyEventArgs key)
    {
        if (key.KeyCode == Keys.D || key.KeyCode == Keys.Right)
        {
            player.dx = 10;
        }
        else if (key.KeyCode == Keys.A || key.KeyCode == Keys.Left)
        {
            player.dx = 10;
        }
        // allow for diagonal movement
        if (key.KeyCode== Keys.S || key.KeyCode == Keys.Down)
        {
            player.dy = 10;
        } 
        else if (key.KeyCode == Keys.W || key.KeyCode == Keys.Up)
        {
            player.dy = -10;
        }
    }

    /** 
    * Check if fish is within the window
    */
    private bool IsOnScreen(Fish f) {
        w = f.fishPic.width;
        h = f.fishPic.height;
        return f.x > -1 * w && f.x < Window.width && 
            f.y > -1 * h && f.y < Window.height;
    }

    private void CheckTouches() {
        foreach (Fish f in fishes) {
            if (f.isTouching(player) && f.fishType == "evilFish") {
                player.isAlive = false;
            }
            else if (f.isTouching(player) && f.fishType == "niceFish") {
                f.isAlive = false;
            }
        }
    }
}
