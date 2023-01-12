using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.ViewportAdapters;

namespace SAE12
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public readonly ScreenManager _screenManager;
        public OrthographicCamera _camera;
        public SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        protected override void Initialize()
        {
            //création caméra
            var viewport = new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080);
            _camera = new OrthographicCamera(viewport);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //Taille fenetre
            _graphics.PreferredBackBufferWidth = 1920; 
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            LoadScreenStart();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var matrix = _camera.GetViewMatrix();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
        public void LoadScreenStart()
        {
            _screenManager.LoadScreen(new Start(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
        public void LoadScreenLobby()
        {
            _screenManager.LoadScreen(new Lobby(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
        public void LoadScreenNiveau1()
        {
            _screenManager.LoadScreen(new Niveau1(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
        public void LoadScreenNiveau2()
        {
            _screenManager.LoadScreen(new Niveau2(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadScreenNiveau3()
        {
            _screenManager.LoadScreen(new Niveau3(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
        public void LoadScreenNiveau4()
        {
            _screenManager.LoadScreen(new Niveau4(this), new FadeTransition(GraphicsDevice, Color.White));
        }
        public void LoadScreenNiveau5()
        {
            _screenManager.LoadScreen(new Niveau5(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
        public void LoadScreenFin()
        {
            _screenManager.LoadScreen(new Fin(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
        public void LoadScreenCredits()
        {
            _screenManager.LoadScreen(new Credits(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

    }
}