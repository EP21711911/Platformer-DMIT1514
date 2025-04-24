using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Platformer;

public class Platformer : Game
{
    private const int _WindowWidth = 550, _WindowHeight = 400;
    internal const int _Gravity = 80;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Rectangle _gameBoundingBox;
    private Player _player;
    private Collider _ground;
    private List<Platform> _platforms;
    private Texture2D _Background, _ForeGround;
    private SoundEffect _BarkSound, _Theme;
    private SoundEffectInstance _barkInstance, _themeInstance;

    public Platformer()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _WindowWidth;
        _graphics.PreferredBackBufferHeight = _WindowHeight;
        _graphics.ApplyChanges();
        _gameBoundingBox = new Rectangle(0, 0, _WindowWidth, _WindowHeight);
        _player = new Player(new Vector2(200, 50), _gameBoundingBox);
        _ground = new TopCollider(new Vector2(0, 320), new Vector2(_WindowWidth, 1));
        // _ground = new Collider(new Vector2(0, 320), new Vector2(_WindowWidth, 1), Collider.ColliderType.Top);

        _platforms = new List<Platform>
        {
            new Platform(new Vector2(200, 200), new Vector2(100, 25), "Gold"),
            new Platform(new Vector2(50, 150), new Vector2(25, 25), "Gold"),
            new Platform(new Vector2(350, 30), new Vector2(50, 25), "Gold"),
            new Platform(new Vector2(350, 250), new Vector2(75, 25), "Gold"),
            new Platform(new Vector2(450, 100), new Vector2(50, 25), "Gold")
        };

        base.Initialize();
        _player.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Background & Foreground
        _Background = Content.Load<Texture2D>("RealHolyCity");
        _ForeGround = Content.Load<Texture2D>("GoldenTiles");

        // Sounds
        _BarkSound = Content.Load<SoundEffect>("barkAttack");
        _Theme = Content.Load<SoundEffect>("holyMission");
        _barkInstance = _BarkSound.CreateInstance();
        _themeInstance = _Theme.CreateInstance();
        _themeInstance.IsLooped = true;
        _themeInstance.Volume = 0.06f;
        _themeInstance.Play();

        // Player & World
        _player.LoadContent(Content);
        _ground.LoadContent(Content);
        foreach (var platform in _platforms)
        platform.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState kb = Keyboard.GetState();

        if (kb.IsKeyDown(Keys.Left) || kb.IsKeyDown(Keys.A))
        {
            _player.MoveHorizontally(-1);
        }
        else if (kb.IsKeyDown(Keys.Right) || kb.IsKeyDown(Keys.D))
        {
            _player.MoveHorizontally(1);
        }
        else
        {
            _player.Stop();
        }

        if (kb.IsKeyDown(Keys.Space))
        {
            _player.Jump();
            // _barkInstance.Play(); //jump sound
        }

        _player.Update(gameTime);
        _ground.ProcessCollisions(_player, gameTime);
        foreach (Platform platform in _platforms)
        {
            platform.Update(gameTime);
            platform.ProcessCollisions(_player, gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();

        _spriteBatch.Draw(_Background, Vector2.Zero, Color.White);
        _spriteBatch.Draw(_ForeGround, new Vector2(0, 320), Color.White);

        foreach (var platform in _platforms)
            platform.Draw(_spriteBatch);

        // _ground.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}