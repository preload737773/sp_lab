using System;
using System.Reflection.Emit;

namespace LowLevelFunction
{
    public class LowLevelFunction
    {
        private delegate int Operation(int a, int b);
        
        public static int LowLevelMultiplication(int a, int b)
        {
            Type[] OperationArgs = { typeof(int), typeof(int) };
            DynamicMethod mul = new DynamicMethod("Mul", typeof(int), OperationArgs);
            ILGenerator il = mul.GetILGenerator(256);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Mul_Ovf);
            il.Emit(OpCodes.Ret);
            Operation result = (Operation)mul.CreateDelegate(typeof(Operation));
            return result(a, b);
        }
    }
}