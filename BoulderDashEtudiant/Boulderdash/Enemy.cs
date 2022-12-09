// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash





using Display;
using Map;
using SFML.Graphics;
using SFML.System;
using System;
using System.ComponentModel;

namespace Enemy
{
    abstract class enemy:Displayable
    {
        protected static Random Rand = new Random();
        public enemy(string fichierImage, Coord XY, Direction dir,RenderWindow Screen, Map.Map map):base(fichierImage, Screen,XY)
        {
            this.map = map; 
            D = dir;

        }
        protected abstract void Deplacement();
        public override void Afficher()
        {
            tour++;
            if (tour % speed == 0) { Deplacement(); }
            base.Afficher();
        }
        

        protected Direction D { get; set; }
        protected int tour { get; set; }
        protected Map.Map map { get; set; }
        protected int speed { get; set; } = 20;
        
    }

    class Mole:enemy
    {
        public Mole(Coord XY, RenderWindow Screen,Map.Map map) : base("images/papillon24.bmp", XY,Direction.Gauche,Screen,map)
        {

        }

        protected override void Deplacement()
        {
            D = (Direction)Rand.Next(0, 5);

            Coord NewXY = (new Coord(D)).Add(XY);
            if (map.IsEmpty(NewXY) || map.GetObjet(NewXY) == Objet.T) { XY = NewXY; }
            else { Deplacement(); }            
        }
    }
    class Moth : enemy
    {
        public Moth(Coord XY, RenderWindow Screen,Map.Map map) : base("images/papillon24.bmp", XY, Direction.Haut, Screen,map)
        {

        }

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
    }

}
