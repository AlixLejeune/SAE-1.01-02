using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

namespace Jeu
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TiledMap tiledMap;
        private TiledMapRenderer tiledMapRenderer;
        private Vector2 positionPerso;
        private AnimatedSprite perso;
        public const int VITESSE_PERSO = 4;
        private KeyboardState keyboardState;
        private TiledMapTileLayer mapLayer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            positionPerso = new Vector2(20, 340);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            tiledMap = Content.Load<TiledMap>("mapGenerale");
            tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, tiledMap);
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("persoAnimation.sf", new JsonContentLoader());
            perso = new AnimatedSprite(spriteSheet);
            mapLayer = tiledMap.GetLayer<TiledMapTileLayer>("obstacles");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            tiledMapRenderer.Update(gameTime);
            keyboardState = Keyboard.GetState();
            string animation = "idle";


            if (keyboardState.IsKeyDown(Keys.Z))
            {
                ushort tx = (ushort)(positionPerso.X / tiledMap.TileWidth);
                ushort ty = (ushort)(positionPerso.Y / tiledMap.TileHeight - 1);
                animation = "walkNorth";
                if (!IsCollision(tx, ty))
                    positionPerso.Y -= VITESSE_PERSO;
            }
            if (keyboardState.IsKeyDown(Keys.Q))
            {
                ushort tx = (ushort)(positionPerso.X / tiledMap.TileWidth - 1);
                ushort ty = (ushort)(positionPerso.Y / tiledMap.TileHeight);
                animation = "walkWest";
                if (!IsCollision(tx, ty))
                    positionPerso.X -= VITESSE_PERSO;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                ushort tx = (ushort)(positionPerso.X / tiledMap.TileWidth);
                ushort ty = (ushort)(positionPerso.Y / tiledMap.TileHeight + 1);
                animation = "walkSouth";
                if (!IsCollision(tx, ty))
                    positionPerso.Y += VITESSE_PERSO;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                ushort tx = (ushort)(positionPerso.X / tiledMap.TileWidth + 1);
                ushort ty = (ushort)(positionPerso.Y / tiledMap.TileHeight);
                animation = "walkEast";
                if (!IsCollision(tx, ty))
                    positionPerso.X += VITESSE_PERSO;
            }
            perso.Play(animation);
            perso.Update(deltaSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            tiledMapRenderer.Draw();

            _spriteBatch.Begin();
            _spriteBatch.Draw(perso, positionPerso);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool IsCollision(ushort x, ushort y)
        {
            // définition de tile qui peut être null (?)
            TiledMapTile? tile;
            if (mapLayer.TryGetTile(x, y, out tile) == false)
                return false;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }
    }
}