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
    private float _speed;
    private int _direction = 1; // 1 for right, -1 for left
    private float _leftBound, _rightBound;
    public Platform(Vector2 position, Vector2 dimensions, string textureName)
    {
        _position = position;
        _dimensions = dimensions;
        _textureName = textureName;

        // Randomize speed and bounds
        Random rand = new Random(Guid.NewGuid().GetHashCode()); //Generates new ID
        _speed = rand.Next(30, 100); // pixels per second
        _leftBound = _position.X - rand.Next(20, 100);  // how far left it moves
        _rightBound = _position.X + rand.Next(20, 100); // how far right it moves

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
    internal void Update(GameTime gameTime)
{
    float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

    //  move platform
    _position.X += _speed * _direction * delta;

    // clamps
    if (_position.X < _leftBound || _position.X > _rightBound)
    {
        _direction *= -1;
    }

    // update collider positions
    _colliders[0].SetPosition(new Vector2(_position.X + 3, _position.Y));                             // Top
    _colliders[1].SetPosition(new Vector2(_position.X + _dimensions.X - 1, _position.Y + 1));          // Right
    _colliders[2].SetPosition(new Vector2(_position.X + 3, _position.Y + _dimensions.Y));              // Bottom
    _colliders[3].SetPosition(new Vector2(_position.X + 1, _position.Y + 1));                          // Left
}
}