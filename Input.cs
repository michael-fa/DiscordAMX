using AMXWrapperCore;
using discordamx.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace discordamx
{
    internal static partial class Program
    {
        public static void ProcessLocalInput()
        {
            string input = Console.ReadLine()!;
            if (input != null  && m_Setup)
            {
                string[] split = input.Split(' ');
                if (split[0].Length > 0)
                {
                    Type type = typeof(Program);
                    //Use Func to call func instead of this old way.
                    MethodInfo theMethod = type.GetMethod(split[0].ToLower())!;
                    if (theMethod != null)
                    {
                        theMethod.Invoke(typeof(Program), new object[] { input });
                    }
                    else
                    {
                        //Call OnConsoleInput for every script
                        AMXPublic p = null!;
                        foreach (Script scr in Scripting.Manager.m_Scripts)
                        {
                            p = scr.m_Amx.FindPublic("OnConsoleInput");
                            if (p != null)
                            {
                                var tmp1 = p.AMX.Push(split[0]);
                                p.Execute();
                                p.AMX.Release(tmp1);
                            }
                            p = null!;
                        }
                    }

                }


            }
        }


        public static void test(string args)
        {
            Console.WriteLine("Test called. ");
        }

        public static void clr(string args)
        {
            Console.Clear();
        }

    }
}
