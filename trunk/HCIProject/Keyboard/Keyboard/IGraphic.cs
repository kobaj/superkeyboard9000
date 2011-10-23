using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Keyboard
{
    public interface IGraphic
    {
        int Width { get; }
        int Height { get; }

        void Update(Vector2 newPosition);

        void SetColor(Color color);
        void SetRotation(float rotation);
        void SetScale(float scale);

        Color GetColor();

        void Draw(SpriteBatch spriteBatch, Vector2 offset);

        void SetSpriteEffects(SpriteEffects spriteEffect);
        void SetSourceRect(Rectangle rectangle);
    }
}
