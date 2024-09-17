using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class Game
{
    private Fish player;
    private List<Fish> fishes;
    private bool gameOver;
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

        // prevent player from leaving screen
        if (player.x < 50) {
            player.x = 50;
            player.dx = 0;
        }
        else if (player.x > Window.width - 50) {
            player.x = Window.width - 50;
            player.dx = 0;
        }
        if (player.y < 0) {
            player.y = 0;
            player.dy = 0;
        }
        else if (player.y > Window.height - 50) {
            player.y = Window.height - 50;
            player.dy = 0;
        }

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
            if ((new Random()).Next(0, 10) == 0) {
            fishes.Add(new Fish());
            }
        }
        
        // remove fish not on screen
        fishes.RemoveAll(fish => !IsOnScreen(fish));

        // check if game over
        if ( !player.isAlive && fishes.Count > 0) {
            gameOver = true;
        }
    }

    public void Draw(Graphics g)
    {
        g.DrawImage(Image.FromFile("FishAssets/scene.png"), 0, 0, 
            Window.width, Window.height);

        if (!gameOver){
            player.Draw(g);
            foreach (Fish f in fishes) {
                f.Draw(g);
            }
        }
        else {
            // draw game over screen
            Pen pen = new Pen(Color.Black, 3);
            g.DrawRectangle(pen, new Rectangle(0, 0, 200, 150));
            DrawString("Game Over!\n\nPress enter \nto restart", 0, 0, g);
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
            player.dx += 50;
        }
        else if (key.KeyCode == Keys.A || key.KeyCode == Keys.Left)
        {
            player.dx += -50;
        }
        else if (key.KeyCode== Keys.S || key.KeyCode == Keys.Down)
        {
            player.dy += 50;
        }
        
        else if (key.KeyCode == Keys.W || key.KeyCode == Keys.Up)
        {
            player.dy += -50;
        }
        else if (key.KeyCode == Keys.Enter) {
            Setup();
        }
    }

    /** 
    * Check if fish is within the window
    */
    private bool IsOnScreen(Fish f) {
        float w = f.fishPic.Width;
        float h = f.fishPic.Height;
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
                f.fishType = "fishBones";
            }
        }
    }

    // inspired by https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-draw-text-on-a-windows-form?view=netframeworkdesktop-4.8
    public void DrawString(string text, float x, float y, Graphics g)
    {
        g.DrawString(text, new Font("Arial", 16), 
            new SolidBrush(Color.Red), x, y, new StringFormat());
    }
}
