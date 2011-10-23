using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Keyboard
{
    public enum RenderType { Singular, PostFirst, PostLast }

    public abstract class RenderArea
    {
        public RenderType Type;
        protected int X;
        protected int Y;
        protected int Width;
        protected int Height;
        RenderTarget2D PrimaryRenderTarget;
        RenderTarget2D SecondaryRenderTarget;

        public void InitializeRenderArea(int x, int y,int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Type = RenderType.Singular;
        }

        public void SetRenderType(RenderType type)
        {
            Type = type;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            RenderTargetBinding[] OldRenderTarget = spriteBatch.GraphicsDevice.GetRenderTargets();
            
            if(PrimaryRenderTarget==null)
                PrimaryRenderTarget = new RenderTarget2D(spriteBatch.GraphicsDevice, Width, Height);

            if(Type!= RenderType.Singular && SecondaryRenderTarget == null)
                SecondaryRenderTarget = new RenderTarget2D(spriteBatch.GraphicsDevice, Width, Height);

            spriteBatch.GraphicsDevice.SetRenderTarget(PrimaryRenderTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            using (SpriteBatch mainSpriteBatch = new SpriteBatch(spriteBatch.GraphicsDevice))
            {
                mainSpriteBatch.Begin();
                Draw(mainSpriteBatch);
                mainSpriteBatch.End();
            }
            spriteBatch.GraphicsDevice.SetRenderTarget(null);

            if (Type != RenderType.Singular)
            {
                spriteBatch.GraphicsDevice.SetRenderTarget(SecondaryRenderTarget);
                spriteBatch.GraphicsDevice.Clear(Color.Transparent);

                using (SpriteBatch secondarySpriteBatch = new SpriteBatch(spriteBatch.GraphicsDevice))
                {
                    secondarySpriteBatch.Begin();
                    PostDraw(PrimaryRenderTarget, secondarySpriteBatch);
                    secondarySpriteBatch.End();
                }
                spriteBatch.GraphicsDevice.SetRenderTarget(null);
            }

            spriteBatch.GraphicsDevice.SetRenderTargets(OldRenderTarget);

            if (Type == RenderType.Singular)
            {
                spriteBatch.Draw(PrimaryRenderTarget, new Rectangle(X, Y, Width, Height), Color.White);
            }
            else if (Type == RenderType.PostFirst)
            {
                spriteBatch.Draw(SecondaryRenderTarget, new Rectangle(X, Y, Width, Height), Color.White);
                spriteBatch.Draw(PrimaryRenderTarget, new Rectangle(X, Y, Width, Height), Color.White);
            }
            else if (Type == RenderType.PostLast)
            {
                spriteBatch.Draw(PrimaryRenderTarget, new Rectangle(X, Y, Width, Height), Color.White);
                spriteBatch.Draw(SecondaryRenderTarget, new Rectangle(X, Y, Width, Height), Color.White);
            }
            
            
        }

        public Texture2D LoadTexture()
        {
            return PrimaryRenderTarget;
        }

        protected abstract void Draw(SpriteBatch spriteBatch);
        protected abstract void PostDraw(Texture2D screen, SpriteBatch spriteBatch);
    }
}
                                                      