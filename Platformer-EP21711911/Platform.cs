using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer;

public class Platform
{
    private Texture2D _texture;
    private string _textureName;
    private Vector2 _position, _dimensions;
    private List<Collider> _colliders;

    public Platform(Vector2 position, Vector2 dimensions, string textureName)
    {
        _position = position;
        _dimensions = dimensions;
        _textureName = textureName;

        _colliders = new List<Collider>
        {
            new TopCollider(new Vector2(position.X + 3, position.Y), new Vector2(dimensions.X - 6, 1)),
            new RightCollider(new Vector2(position.X + dimensions.X - 1, position.Y + 1), new Vector2(1, dimensions.Y - 2)),
            new BottomCollider(new Vector2(position.X + 3, position.Y + dimensions.Y), new Vector2(dimensions.X - 6, 1)),
            new LeftCollider(new Vector2(position.X + 1, position.Y + 1), new Vector2(1, dimensions.Y - 2))
        };
    }

    internal void LoadContent(ContentManager content)
    {
        if (!string.IsNullOrEmpty(_textureName))
    {
        _texture = content.Load<Texture2D>(_textureName);
    }
        foreach (Collider collider in _colliders)
        {
            collider.LoadContent(content);
        }
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        if (_texture != null)
    {
        spriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, (int)_dimensions.X, (int)_dimensions.Y), Color.White);
    }
        // foreach (Collider collider in _colliders)
        // {
        //     collider.Draw(spriteBatch);
        // }
    }

    internal void ProcessCollisions(Player player, GameTime gameTime)
    {
        foreach (Collider collider in _colliders)
        {
            collider.ProcessCollisions(player, gameTime);
        }
    }
}