using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Swimma.GameLibrary
{
    public class SoundLibrary
    {
        private static SoundEffect menuItemFocus;
        private static SoundEffect menuItemOK;
        private static SoundEffect enemyHit;
        private static SoundEffect playerDie;
        private static SoundEffect gameOver;


        private static SoundEffect gemCollect;
        private static SoundEffect bubbleCollect;

        private static Song backGroundMusic;
        private static Boolean backGroundSongStarted;
        private static SoundEffect lifeCollect;

        public static SoundEffect MenuItemFocus
        {
            get { return menuItemFocus; }
            set { menuItemFocus = value; }
        }

        public static SoundEffect MenuItemOK
        {
            get { return menuItemOK; }
            set { menuItemOK = value; }
        }

        public static SoundEffect EnemyHit
        {
            get { return enemyHit; }
            set { enemyHit = value; }
        }

        public static SoundEffect PlayerDie
        {
            get { return playerDie; }
            set { playerDie = value; }
        }

        public static SoundEffect GameOver
        {
            get { return gameOver; }
            set { gameOver = value; }
        }
        public static SoundEffect GemCollect
        {
            get { return gemCollect; }
            set { gemCollect = value; }
        }
        public static SoundEffect LifeCollect
        {
            get { return lifeCollect; }
            set { lifeCollect = value; }
        }

        public static SoundEffect BubbleCollect
        {
            get { return bubbleCollect; }
            set { bubbleCollect = value; }
        }

        public static Song BackGroundMusic
        {
            get { return backGroundMusic; }
            set { backGroundMusic = value; }
        }

        public static Boolean BackGroundSongStarted
        {
            get { return backGroundSongStarted; }
            set { backGroundSongStarted = value; }
        }
    }

}
