using System;
using System.Drawing;
using System.Windows.Forms;

public class Fish
{
    private static Random rng = new Random();
    public Image fishPic;
    public string fishType;
    public bool isAlive;
    public float x;
    public float y;
    public float dx;
    public float dy;
    public bool isSpedUp;

    // Constructor for making the player
    public Fish(int i)
    {
        this.fishType = "player";
        fishPic = GetFish();
        x = (float)(Window.width * 0.5);
        y = (float)(Window.height * 0.5);
        isAlive = true;
        isSpedUp = false;
    }

    // Constructor for making the other fish
    public Fish()
    {
        fishPic = GetFish();
        SetStartLoc();
        isAlive = true;
        isSpedUp = false;
    }

    private Image GetFish()
    {
        if (this.fishType == "player")
        {
            return Image.FromFile("FishAssets/Player.png");
        }
        int rand = rng.Next(1, 5);
        string filename = "FishAssets/";
        if (rng.Next(1, 3) == 1)
        {
            this.fishType = "niceFish";
        }
        else
        {
            this.fishType = "evilFish";
        }
        filename += this.fishType + rand + ".png";
        return Image.FromFile(filename);
    }

    private void SetStartLoc()
    {
        x = (float)rng.Next(0, Window.width);
        y = (float)rng.Next(0, Window.height);
        // Determine which side of the screen fish will start from
        switch (rng.Next(1, 5))
        {
            case 1:
                x = 0;
                dx = 50;
                dy = (float)rng.Next(-20, 21);
                break;
            case 2:
                x = Window.width;
                dx = -50;
                dy = (float)rng.Next(-20, 21);
                break;
            case 3:
                y = 0;
                dy = 50;
                dx = (float)rng.Next(-20, 21);
                break;
            default:
                y = Window.height;
                dy = -50;
                dx = (float)rng.Next(-20, 21);
                break;
        }
    }

    public bool isTouching(Fish other)
    {
        Rectangle rect1 = new Rectangle((int)this.x, (int)this.y, 50, 50);
        Rectangle rect2 = new Rectangle((int)other.x, (int)other.y, 50, 50);
        return rect1.IntersectsWith(rect2);
    }

    public void Update(float dt)
    {
        if (!this.isAlive)
        {
            // Sink the skeletons
            y += 25 * dt;
            x += rng.Next(-10, 10) * dt;
        }
        else
        {
            // Swim the fish, but don't let player leave the screen
            float d1 = dx * dt;
            float d2 = dy * dt;
            x += (x + d1 > 0 && x + d1 < Window.width 
                || this.fishType != "player") ? d1 : 0;
            y += (x + d2 > 0 && x + d2 < Window.width 
                || this.fishType != "player") ? d2 : 0;
            if (this.fishType != "player" && rng.Next(0, 5) == 4)
            {
                // Randomize fish swimming to make fish swim fishy
                dx += 10 * rng.Next(-7, 7);
                dy += 10 * rng.Next(-7, 7);
            }
        }
    }

    public void Draw(Graphics g)
    {
        Image imageToDraw;
        if (this.isAlive)
        {
            imageToDraw = fishPic;
        }
        else if (this.fishType == "player")
        {
            imageToDraw = Image.FromFile("FishAssets/deadFish.png");
        }
        else
        {
            imageToDraw = Image.FromFile("FishAssets/fishBones.png");
        }

        g.DrawImage(imageToDraw, x, y, 50, 50);
    }
}
