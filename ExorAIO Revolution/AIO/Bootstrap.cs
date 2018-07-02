/*
 * To whoever got to see this point of my GitHub history,
 *
 * For years, ever since my childhood, I've been enamored by the greatness of the computer world and games in general - to tell stories in ways not possible using traditional media or communication.
 * ExorAIO is my letter to that.
 * Games are an interactive art, some let you explore new worlds. Some challenge your mind in brand new ways. Some make you feel like a hero or a friend, even when life is hard on you.
 * League of Legends, for example, has been able not only to become the most played game in the world, but some people became addicted to it, and started considering new possibilities in life and started viewing the world from a different perspective, motivated by the lore of their favorite champions.
 * For me, that was the case, I could see myself in a perspective that I could have never imagined to see myself before, I had no longer real ambitions moved by my own god complex or my sense of justice, I just kept developing cause that's what made me feel happy and helped me get through the hard days of my life: it helped me understand who I really was as a person and what were the kind of people I was looking for.
 * That's what games are, they warp ourselves in the world we would want to live in, and no matter the game: if you, the players, are enjoying yourselves, that's what really matters.
 * Preferences are preferences after all, and our differences are the reason we have a thriving video game industry and even scripting platforms.
 * Throughout my journey, I didn't just learn how to code, I've got to meet wonderful people, always willing to help each other without getting nothing in return, just for the sake of a better and more prosperous community, I've never seen people behaving like this towards other people in the real world.
 * This is what made both LeagueSharp and Aimtec feel special, not the quality of the scripts nor the security levels of the platforms, but the community: people with different ideas, different beliefs and different views of the world surrounding them, working together like brothers for the greater good.
 * I've got to know people who, behind their mask, were fighting every single day against life adversities, to ensure a better future for them and their families.
 * That's what people really are, every one of us fights for their freedom and, in its name, refuses to accept the injustices life brings.
 * I am not the real developer of this project, every single one of you is, and you got to be a part of it even just by talking to me, even just to mess around together once, it made that day for me.
 * I extend my gratitude to all those who had the time to try my assembly and changed their idea about me regarding how I was behaving back in 2014 on LeagueSharp, I'm deeply sorry, you really helped me out and gave me the motivation I needed to keep going, Thank you all.
 * Thanks to anyone who made this project possible, I've put my heart and soul into it, I hope you enjoyed and will keep enjoying using it as much as I enjoyed making it.
 * Thank you for having been an integral part of the best days of my life
 *
 * With neverending love and gratitude,
 *
 * Exory
 */

using System;
using System.Reflection;
using AIO.Utilities;

#pragma warning disable 1587
namespace AIO
{
    /// <summary>
    ///     The bootstrap class.
    /// </summary>
    internal static class Bootstrap
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Tries to load the champion which is being currently played.
        /// </summary>
        public static void LoadChampion()
        {
            try
            {
                var pluginName = "AIO.Champions." + UtilityClass.Player.ChampionName;
                var type = Type.GetType(pluginName, true);
                if (type != null)
                {
                    Activator.CreateInstance(type);
                    Console.WriteLine($"ExorAIO: Revolution - {UtilityClass.Player.ChampionName} Loaded.");
                }
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case TargetInvocationException _:
                        Console.WriteLine($"ExorAIO: Revolution - Error occurred while trying to load {UtilityClass.Player.ChampionName}.");
                        Console.WriteLine(e);
                        break;
                    case TypeLoadException _:
                        for (var i = 1; i < 30; i++)
                        {
                            Console.WriteLine($"ExorAIO: Revolution - {UtilityClass.Player.ChampionName} is NOT supported yet.");
                        }
                        break;
                }
            }
        }

        #endregion
    }
}