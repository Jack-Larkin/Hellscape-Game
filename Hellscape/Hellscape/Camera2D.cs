
using Hellscape;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


/*
 * Camera class to describe the matrix that the spritebatch uses to draw to the screen
 * 
 * NOTE: The majority of this file is not my own work, credit goes to:
 * 
 * Schofield,S(2020), 2D Games Camera [Source Code] now.ntu.ac.uk
 */


 public class Camera2D
{
    private readonly Viewport _viewport;

    public Camera2D(Viewport viewport)
    {
        _viewport = viewport;

        Rotation = 0;
        Zoom = 1;
        Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
        Position = Vector2.Zero;
    }

    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public float Zoom { get; set; }
    public Vector2 Origin { get; set; }

    //matrix to describe camera exxects of the spritebatch when drawing to the screen
    public Matrix GetViewMatrix()
    {
        return
            Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
            Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1) *
            Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
    }

    //my own additional function to keep the camera centred onto the player
    public void lockToPlayer(Player player)
    {
        Position = new Vector2(player.getScreenPosition().X - Origin.X, player.getScreenPosition().Y - Origin.Y);

    }
}