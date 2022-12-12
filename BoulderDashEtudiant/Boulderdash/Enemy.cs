// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash




//library
#region
using Display;
using Map;
using SFML.Graphics;
using SFML.System;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
#endregion

namespace Enemy
{
    //class enemy that herit from displayable that dictate what is showable
    #region
    abstract class enemy:Displayable
    {
        //variable
        #region
        protected Direction D { get; set; }         //direction it looking
        protected int tour { get; set; }            //the number of turn
        protected Map.Map map { get; set; }         //the map 
        protected int speed { get; set; } = 20;     //the number of turn it take to move

        protected static Random Rand = new Random();//the random aspect of enemy
        #endregion

        //constructer
        #region
        public enemy(string fichierImage, Coord XY, Direction dir,RenderWindow Screen, Map.Map map):base(fichierImage, Screen,XY)
        {
            this.map = map; 
            D = dir;

        }
        #endregion

        //function
        #region
        //the moving part of the enemy is obligated but not strick
        protected abstract void Deplacement();
        //the function that make the ennemy showable
        public override void Afficher()
        {
            tour++;
            kill();
            if (tour % speed == 0) { Deplacement(); }
            base.Afficher();
        }
        //the part the enemy kill the hero is mandatory but not strick
        protected abstract void kill();
        #endregion
    }
    #endregion

    //class mole that herit from enemy
    #region
    class Mole :enemy
    {
        //variable
        #region
        //none
        #endregion

        //constructer
        #region
        //using the constructeer from enemy and displayable to make the mole
        public Mole(Coord XY, RenderWindow Screen,Map.Map map) : base("images/papillon24.bmp", XY,Direction.Gauche,Screen,map)
        {

        }
        #endregion

        //function
        #region
        //the way the mole move
        protected override void Deplacement()
        {
            
            D = (Direction)Rand.Next(0, 5);//random from every direction and can be idle

            Coord NewXY = (new Coord(D)).Add(XY);
            if (map.IsEmpty(NewXY) || map.GetObjet(NewXY) == Objet.T) { XY = NewXY; }
            else { Deplacement(); }
            
        }

        //the way it can kill the hero   
        protected override void kill()
        {
            if (map.RockFord.XY.Equals(XY)){ map.GameContinue = false; }
        }
        #endregion
    }
    #endregion

    //class moth the herit from enemy 
    #region
    class Moth : enemy
    {
        //variable
        #region
        //none
        #endregion

        //constructer
        #region
        // this use the constructer of enemy and displayable
        public Moth(Coord XY, RenderWindow Screen,Map.Map map) : base("images/papillon24.bmp", XY, Direction.Haut, Screen,map)
        {

        }
        #endregion

        //fuction
        #region
        //the way the moth move by following the right wall and circling counter clock wise
        protected override void Deplacement()
        {
            
            //this algo check always on the left to make the moth follow the left wall
            Coord dir = new Coord(D).RotatedLeft();//create dir whit a direction 
            for (int i = 0; i < 4; i++)
            {
                if (map.IsEmpty(XY.Clone().Add(dir))) { XY.Add(dir); D = dir.AsDirection(); return; }
                //check in map if position dir facing is empty
                //so we make a clone of XY that is a vector and add the direction if empty go to that direction and change the direction to the
                //one of the move and the return make it stop when find a move
                dir.RotatedRight();//then check were he is facing and after on his right... till finding a posible move 
                
            }
            


        }
        //the way the moth kill the hero
        protected override void kill()
        {
            if (map.RockFord.XY.Equals(XY)){ map.GameContinue = false; }
        }
        #endregion
    }
    #endregion

}
