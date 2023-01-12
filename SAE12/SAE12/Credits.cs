using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using System;


namespace SAE12
{
    public class Credits : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu 
        public readonly ScreenManager _screenManager;
        Texture2D _texture;
        public SpriteBatch _spriteBatch;

        SpriteFont _police;

        Texte _texte;

        private Texture2D _box;
        private Texture2D _boxPerso;

        public SoundEffect _soundPerso;
        public SoundEffect _soundActive;

        public Song _song;

        public bool inter;
        public bool move = false;
        public bool debut = true;
        public bool active = false;

        int etat = 0;
        int count = 120;

        string txtPerso = "Dorian PAPIRIS   Quentin BARDOTTI   Alix LEJEUNE";

        public Credits(Game1 game) : base(game)
        {
            _myGame = game;
            _spriteBatch = _myGame._spriteBatch;

            _texte = new Texte();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent()
        {
            _texture = Content.Load<Texture2D>("fin");

            _box = Content.Load<Texture2D>("text_box");
            _boxPerso = Content.Load<Texture2D>("perso_box");

            _police = Content.Load<SpriteFont>("fontuwu");
            _texte.Load(_police, txtPerso);

            _soundPerso = Content.Load<SoundEffect>("voix_perso");
            _soundActive = Content.Load<SoundEffect>("voix_princesse");

            _song = Content.Load<Song>("maxwell music");
            MediaPlayer.Play(_song);
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            

            if (inter)
            _texte.Update(gameTime, _soundPerso, 2);

          

            if (count > 0)
                count--;

            if(count <= 0 && etat == 0)
            {

                _texte.Load(_police, txtPerso);
                inter = true;
                etat = 1;
                
            }

            Console.WriteLine(count);
        }



        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.End();

            if (inter)
            {

                _texte.Draw(_spriteBatch, 380, 700);
            }

        }






    }
}
