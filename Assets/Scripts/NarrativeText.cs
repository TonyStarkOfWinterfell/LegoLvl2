using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeText : MonoBehaviour
{

    //NarrativeText.Type("You jumped in place",100);

        public static void Type(string text, int delay)
        {
            var rand = new System.Random();

            char[] characters = text.ToCharArray();
            int count = rand.Next(1, 7);
            foreach (char character in characters)
            {

                Thread.Sleep(rand.Next(0, delay));
                count++;
                //Rainbow obj = new Rainbow();
                Color(count);

            System.Console.Write(character);
                if (count > 7) { count = 0; }
            }

        System.Console.Write("\n");

        }
        public static void Color(int count)
        {

            switch (count)
            {
                case 1:
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                    break;
                case 2:
                System.Console.ForegroundColor = System.ConsoleColor.DarkYellow;
                    break;
                case 3:
                System.Console.ForegroundColor = System.ConsoleColor.Yellow;
                    break;
                case 4:
                System.Console.ForegroundColor = System.ConsoleColor.Green;
                    break;
                case 5:
                System.Console.ForegroundColor = System.ConsoleColor.Cyan;
                    break;
                case 6:
                System.Console.ForegroundColor = System.ConsoleColor.Blue;
                    break;
                case 7:
                System.Console.ForegroundColor = System.ConsoleColor.DarkMagenta;
                    break;

            }
        }
    }

