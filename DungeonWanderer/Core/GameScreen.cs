﻿using System;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Artemis;
using DungeonWanderer.Components;
using DungeonWanderer.Systems;
using Artemis.Manager;
using DungeonWanderer.Templates;

namespace DungeonWanderer.Core
{
    public class GameScreen : Screen
    {
        private CameraComponent camera;
        private EntityWorld entityWorld;
        private World box2DWorld;
        public GameScreen(DWGame dwgame) : base(dwgame)
        {
        }

        public override void Draw(GameTime gametime)
        {
            spriteBatch.Begin();

            entityWorld.Draw();

            spriteBatch.End();
        }

        public override void Initialize()
        {
            box2DWorld = new World(new Vector2(0f, -9.82f));
            entityWorld = new EntityWorld();

            entityWorld.SystemManager.SetSystem<MovementSystem>(new MovementSystem(box2DWorld), GameLoopType.Update);
            entityWorld.SystemManager.SetSystem<CameraUpdateSystem>(new CameraUpdateSystem(game.Graphics), GameLoopType.Update);
            entityWorld.SystemManager.SetSystem<PlayerControllerSystem>(new PlayerControllerSystem(), GameLoopType.Update);
            entityWorld.SystemManager.SetSystem<SelfRotatingPlatformtSystem>(new SelfRotatingPlatformtSystem(), GameLoopType.Update);

            entityWorld.SetEntityTemplate(PlayerTemplate.Name, new PlayerTemplate());
            entityWorld.SetEntityTemplate(BasicTerrainTemplate.Name, new BasicTerrainTemplate());
            entityWorld.SetEntityTemplate(BasicRotatingTerrainTemplate.Name, new BasicRotatingTerrainTemplate());

            Entity player=entityWorld.CreateEntityFromTemplate(PlayerTemplate.Name, new Vector2(1, 1), box2DWorld, 
                game.AssetManager.TextureManager.GetTexture("iuper"));
            entityWorld.CreateEntityFromTemplate(BasicTerrainTemplate.Name, new Vector2(1, -2f),
                box2DWorld, game.AssetManager.TextureManager.GetTexture("terrain"), 0f);
            entityWorld.CreateEntityFromTemplate(BasicRotatingTerrainTemplate.Name, new Vector2(6, -3f),
                box2DWorld, game.AssetManager.TextureManager.GetTexture("terrain"), Math.PI / 2);
            camera = player.GetComponent<CameraComponent>();

            entityWorld.SystemManager.SetSystem<RenderingSystem>(new RenderingSystem(camera, spriteBatch,game.GraphicsDevice), GameLoopType.Draw);

            //TODO Imlement player respawn, health systems, obstacles and enemies spawn systems,
            //background /foreground static images, scene loading from a json file

        }

        public override void Update(GameTime gameTime)
        {
            box2DWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            entityWorld.Update();
        }
    }
}
