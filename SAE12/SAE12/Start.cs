using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;


namespace SAE12
{
    public class Start : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu 
        public readonly ScreenManager _screenManager;
        Texture2D _texture;
        public SpriteBatch _spriteBatch;

        public Start(Game1 game) : base(game)
        {
            _myGame = game;
            _spriteBatch = _myGame._spriteBatch;
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent()
        {
            _texture = Content.Load<Texture2D>("start");
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                _myGame.LoadScreenLobby();
            }
        }



        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.End();

        }






    }
}
