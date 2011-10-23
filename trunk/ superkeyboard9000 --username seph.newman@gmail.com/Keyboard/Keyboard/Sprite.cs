using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Keyboard
{
    //The base component of the game
    //An image with a position
    public class Sprite : IGraphic
    {
        public int Width { get { return Texture!=null?Texture.Width:0; } }
        public int Height { get { return Texture!=null?Texture.Height:0; } }

        public Texture2D Texture;
        public Vector2 Position = Vector2.Zero;
        public Rectangle? SourceRect = null;
        public Rectangle? Destination = null;
        public Color Color = Color.White;
        public float Rotation = 0f;
        public Vector2 Origin = Vector2.Zero;
        public float Scale = 1f;
        public SpriteEffects Effects = SpriteEffects.None;
        public float LayerDepth = 0;


        public Sprite(Texture2D textureGiven)
        {
            Texture = textureGiven;
        }

        //Texture and position
        //Origin Based: Positions are relative to the center of the sprite,
        //Otherwise they are relative to (0,0) - The UpperLeft point of the sprite
        public Sprite(Texture2D textureGiven, Vector2 positionGiven)
        {

            Texture = textureGiven;
            Position = positionGiven;
        }

        //Texture and rectangle
        public Sprite(Texture2D textureGiven, Rectangle destinationGiven)
        {
            Texture = textureGiven;
            Destination = destinationGiven;
        }

        //Texture, rectangle, and color
        public Sprite(Texture2D textureGiven, Rectangle destinationGiven, Color colorGiven)
        {
            Texture = textureGiven;
            Destination = destinationGiven;
            Color = colorGiven;
        }

        public Sprite(Texture2D textureGiven, Vector2 positionGiven, Rectangle sourceGiven)
        {
            Texture = textureGiven;
            Position = positionGiven;
            SourceRect = sourceGiven;
        }

        //Copy Constructor
        public Sprite(Sprite other)
        {
            Texture = other.Texture;
            Position = other.Position;
            SourceRect = other.SourceRect;
            Destination = other.Destination;
            Color = other.Color;
            Rotation = other.Rotation;
            Origin = other.Origin;
            Scale = other.Scale;
            Effects = other.Effects;
            LayerDepth = other.LayerDepth;

        }

        public void SetColor(Color color)
        {
            Color = color;
        }

        public Color GetColor()
        {
            return Color;
        }

        public void SetSpriteEffects(SpriteEffects spriteEffect)
        {
            Effects = spriteEffect;
        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
        }

        public void SetSourceRect(Rectangle rectangle)
        {
            SourceRect = rectangle;
        }

        public void SetPosFromNonOrg(Vector2 positionGiven)
        {
            Position = new Vector2(positionGiven.X + Texture.Width / 2, positionGiven.Y + Texture.Height / 2);
        }

        public void Update(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
           
            if (Texture == null)
                return;
            if (Destination == null)
            {
                spriteBatch.Draw(
                    Texture,
                    Position + offset,
                    SourceRect,
                    Color,
                    Rotation,
                    Origin,
                    Scale,
                    Effects,
                    LayerDepth);
                

            }
            else
            {
                Rectangle Boundary = new Rectangle(Convert.ToInt32(Destination.Value.X + offset.X), Convert.ToInt32(Destination.Value.Y + offset.Y), Destination.Value.Width, Destination.Value.Height);
                spriteBatch.Draw(
                    Texture,
                    Boundary,
                    SourceRect,
                    Color,
                    Rotation,
                    Origin,
                    Effects,
                    LayerDepth);

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Vector2.Zero);

        }
        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
        }
        public void SetScale(float scale)
        {
            Scale = scale;
        }
    }
}
