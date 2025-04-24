using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer;

public abstract class Collider
{
    protected Vector2 _position, _dimensions;
    protected Texture2D _texture;

    public Rectangle BoundingBox => new Rectangle((int)_position.X, (int)_position.Y, (int)_dimensions.X, (int)_dimensions.Y);

    protected Collider(Vector2 position, Vector2 dimensions)
    {
        _position = position;
        _dimensions = dimensions;
    }

    public abstract void LoadContent(ContentManager content); // abstract instead

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, BoundingBox, new Rectangle(0, 0, 1, 1), Color.White);
    }

    public abstract bool ProcessCollisions(Player player, GameTime gameTime);

}

#region Children Colliders
public class TopCollider : Collider
{
    public TopCollider(Vector2 position, Vector2 dimensions)
        : base(position, dimensions) { }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("ColliderTop");
    }

    public override bool ProcessCollisions(Player player, GameTime gameTime)
    {
        if (!BoundingBox.Intersects(player.BoundingBox)) return false;

        if (player.Velocity.Y > 0)
        {
            player.Land(BoundingBox);
            player.StandOn(gameTime);
            return true;
        }
        return false;
    }
}

public class BottomCollider : Collider
{
    public BottomCollider(Vector2 position, Vector2 dimensions)
        : base(position, dimensions) { }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("ColliderBottom");
    }

    public override bool ProcessCollisions(Player player, GameTime gameTime)
    {
        if (!BoundingBox.Intersects(player.BoundingBox)) return false;

        if (player.Velocity.Y < 0)
        {
            player.MoveVertically(0);
            return true;
        }
        return false;
    }
}

public class LeftCollider : Collider
{
    public LeftCollider(Vector2 position, Vector2 dimensions)
        : base(position, dimensions) { }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("ColliderLeft");
    }

    public override bool ProcessCollisions(Player player, GameTime gameTime)
    {
        if (!BoundingBox.Intersects(player.BoundingBox)) return false;

        if (player.Velocity.X > 0)
        {
            player.MoveHorizontally(0);
            return true;
        }
        return false;
    }
}

public class RightCollider : Collider
{
    public RightCollider(Vector2 position, Vector2 dimensions)
        : base(position, dimensions) { }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("ColliderRight");
    }

    public override bool ProcessCollisions(Player player, GameTime gameTime)
    {
        if (!BoundingBox.Intersects(player.BoundingBox)) return false;

        if (player.Velocity.X < 0)
        {
            player.MoveHorizontally(0);
            return true;
        }
        return false;
    }
}
#endregion
