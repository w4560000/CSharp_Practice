using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Emit
{
    class Program
    {
        static void Main(string[] args)
        {
            // create Assembly
            var asmName = new AssemblyName(" Test ");
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);

            // create Module
            var mdlBldr = asmBuilder.DefineDynamicModule(" Main ", " Main.dll ");

            var typeBldr = mdlBldr.DefineType(" Hello ", TypeAttributes.Public);
            var methodBldr = typeBldr.DefineMethod(" SayHello ", MethodAttributes.Public); // return type null // parameter type );

            var il = methodBldr.GetILGenerator();
            il.Emit(OpCodes.Ldstr, " Hello, World "); 
            il.Emit(OpCodes.Call, typeof(Console).GetMethod(" WriteLine ", new Type[] { typeof(string) })); 
            il.Emit(OpCodes.Ret);
        }
    }
}
