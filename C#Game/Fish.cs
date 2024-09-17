using System;
using System.Drawing;
using System.Windows.Forms;

public class Fish
{
    private static Random rng = new Random();
    private Image fishPic;
    public string fishType { get; set; }
    public bool isAlive { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public float dx { get; set; }
    public float dy { get; set; }
    public bool isSpedUp { get; set; }

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
                dx = 10;
                dy = (float)rng.Next(-10, 11);
                break;
            case 2:
                x = Window.width;
                dx = -10;
                dy = (float)rng.Next(-10, 11);
                break;
            case 3:
                y = 0;
                dy = 10;
                dx = (float)rng.Next(-10, 11);
                break;
            default:
                y = Window.height;
                dy = -10;
                dx = (float)rng.Next(-10, 11);
                break;
        }
    }

    public bool isTouching(Fish other)
    {
        Rectangle rect1 = new Rectangle((int)x, (int)y, fishPic.Width, fishPic.Height);
        Rectangle rect2 = new Rectangle((int)other.x, (int)other.y, other.fishPic.Width, other.fishPic.Height);
        return rect1.IntersectsWith(rect2);
    }

    public void Update(float dt)
    {
        if (!this.isAlive)
        {
            // Sink the skeletons
            y += -5 * dt;
            x += rng.Next(-3, 3) * dt;
        }
        else
        {
            // Swim the fish
            x += dx * dt;
            y += dy * dt;
            if (this.fishType == "player")
            {
                dx = dy = 0;
            }
            else if (rng.Next(0, 5) == 4)
            {
                // Randomize fish swimming to make fish swim fishy
                dx += rng.Next(-7, 7);
                dy += rng.Next(-7, 7);
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
