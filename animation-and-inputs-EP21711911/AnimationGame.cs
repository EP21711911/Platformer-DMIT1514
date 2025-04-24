using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Animation;

public class AnimationGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _Background, _ForeGround;
    
    private CelAnimationSequence _WalkingAnim, _IdleAnim, _DeathAnim, _AttackAnim, _HurtAnim;
    private CelAnimationPlayer _animationPlayer;
    private Texture2D _WalkingSprite, _IdleSprite, _DeathSprite, _AttackSprite, _HurtSprite;
    private SpriteEffects spriteEffects;
 private SoundEffect _BarkSound, _Theme;
 private SoundEffectInstance _barkInstance, _themeInstance;
      private int _x;
      private int _y= 300;
      private bool _Attacking = false;
    private KeyboardState _kbPreviousState;
    public AnimationGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {        
        //New executable thing (Batch)
       _spriteBatch = new SpriteBatch(GraphicsDevice);
       
_animationPlayer = new CelAnimationPlayer();
#region walkingSprite
        _WalkingSprite  = Content.Load<Texture2D>("Walking");
        _WalkingAnim = new CelAnimationSequence(_WalkingSprite, 48, 1 / 8f);
        
        #endregion

    #region IdleSprite
      _IdleSprite = Content.Load<Texture2D>("Idle");
        _IdleAnim = new CelAnimationSequence(_IdleSprite, 48, 1 / 8f);

        
#endregion
#region AttackSprite
      _AttackSprite = Content.Load<Texture2D>("Attack");
        _AttackAnim = new CelAnimationSequence(_AttackSprite, 48, 1 / 8f);
        
        
#endregion

#region Background
        _Background = Content.Load<Texture2D>("RealHolyCity");
        _graphics.PreferredBackBufferWidth = _Background.Width; ;
        _graphics.PreferredBackBufferHeight = _Background.Height;
        
#endregion
#region ForeGround
        _ForeGround = Content.Load<Texture2D>("GoldenTiles");

#endregion
#region Sounds
        _BarkSound = Content.Load<SoundEffect>("barkAttack");
        _Theme = Content.Load<SoundEffect>("holyMission");
        _barkInstance = _BarkSound.CreateInstance();
        _themeInstance = _Theme.CreateInstance();
         _themeInstance.IsLooped = true;
         _themeInstance.Volume = 0.06f;   
        _themeInstance.Play();
#endregion
        _graphics.ApplyChanges();
    }
    protected override void Update(GameTime gameTime)
    {
        _animationPlayer.Update(gameTime);
       
#region player movement
        KeyboardState kbCurrentState = Keyboard.GetState();

        #region arrow keys
        if(kbCurrentState.IsKeyDown(Keys.Down) || kbCurrentState.IsKeyDown(Keys.S) ) //down arrow
        {
            _y ++;
         
        }
        if(kbCurrentState.IsKeyDown(Keys.Up) || kbCurrentState.IsKeyDown(Keys.W)) //up arrow
        {
            _y--;
           
        }
        if(kbCurrentState.IsKeyDown(Keys.Left) || kbCurrentState.IsKeyDown(Keys.A)) //left arrow
        {
            spriteEffects = SpriteEffects.FlipHorizontally;
            _x--;
        
        }
        if(kbCurrentState.IsKeyDown(Keys.Right) || kbCurrentState.IsKeyDown(Keys.D)) //right arrow
        {
            spriteEffects = SpriteEffects.None;
            _x++;
           
        }
      if(kbCurrentState.IsKeyUp(Keys.W) && kbCurrentState.IsKeyUp(Keys.A) && kbCurrentState.IsKeyUp(Keys.S) && kbCurrentState.IsKeyUp(Keys.D) )
        {
           _animationPlayer.Play(_IdleAnim);
        }
        if(kbCurrentState.IsKeyDown(Keys.W) || kbCurrentState.IsKeyDown(Keys.A) || kbCurrentState.IsKeyDown(Keys.S) || kbCurrentState.IsKeyDown(Keys.D) )
        {if (!_Attacking){
            _animationPlayer.Play(_WalkingAnim);
        }
         
        }
    if(kbCurrentState.IsKeyDown(Keys.Space ))
        {
        //    _animationPlayer.Play(_AttackAnim);
        //    _Attacking = true;
        //    _barkInstance.Play();
           
        }
        #endregion
        #endregion
_kbPreviousState = kbCurrentState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gold);

        _spriteBatch.Begin();
        //Images
        _spriteBatch.Draw(_Background, Vector2.Zero, Color.White);
        _spriteBatch.Draw(_ForeGround, new Vector2(0, 320), Color.White);
        //Player
        _animationPlayer.Draw(_spriteBatch, new Vector2(_x,_y), spriteEffects);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
