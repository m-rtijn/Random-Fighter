using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDareTextBasedGame
{
    class Event
    {
        public int targetHealth;
        public int targetBaseHealth;
        public string eventText;
        public string targetName;

        public Event(string inputEventText, int inputTargetHealth, string inputTargetName = "The Target")
        {
            targetHealth = inputTargetHealth;
            targetBaseHealth = inputTargetHealth;
            eventText = inputEventText;
            targetName = inputTargetName;

            Console.WriteLine(eventText);
        }

        /// <summary>
        /// let the target get dmg
        /// </summary>
        /// <param name="dmg"></param>
        /// <returns>false if dead true if alive</returns>
        public bool GetDamage(int dmg, string weapon)
        {
            targetHealth -= dmg;
            Console.WriteLine("You hit " + targetName + " with " + weapon + ".");
            Console.WriteLine(targetName + " got " + dmg.ToString() + " damage.");

            if (targetHealth <= 0)
            {
                Console.WriteLine("You killed " + targetName);
                return false;
            }
            else
            {
                Console.WriteLine(targetName + " still has " + targetHealth.ToString() + " health left.");
                return true;
            }
        }
    }
}
