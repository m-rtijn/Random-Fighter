using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDareTextBasedGame
{
    class Player
    {
        // Properties
        public int health;
        public int xp;
        public int money;
        public string name;
        public List<string> weapons = new List<string>();
        private Random random = new Random();


        public Player(string inputName, string firstWeapon)
        {
            name = inputName;
            weapons.Add(firstWeapon);
            health = 100;
            xp = 10;
        }

        /// <summary>
        /// Checks if move is avaialable and returns damage
        /// </summary>
        /// <param name="move">selected move</param>
        /// <returns>damage count. if it's -1 the move is unavailable.</returns>
        public int DoMove(string inputWeapon)
        {
            foreach(string weapon in weapons)
            {
                if (weapon == inputWeapon)
                {
                    int dmg = random.Next(xp / 10, xp / 1);
                    return dmg;
                }
            }
            return -1;
        }

        public void GetDamage(int dmg)
        {
            health -= dmg;
            Console.WriteLine("Oh no! You recieved " + dmg.ToString() + " damage.");
        }
    }
}
