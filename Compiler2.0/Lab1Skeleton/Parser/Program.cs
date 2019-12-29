using System;
using System.IO;
using System.Diagnostics;

namespace Parser
{
	class MainClass
	{
		
		public static void Main(string[] args)
		{
            if (args.Length < 1)
            {
                Console.WriteLine("Usage; {0} [-t | <filename>]", Process.GetCurrentProcess().ProcessName);
                return;
            }

            try
			{
                StreamReader input;

                if (args[0] == "-t")
                {
                    input = new StreamReader(Console.OpenStandardInput());
                }
                else
                {
                    input = new StreamReader(args[0]);
                }

                string program = input.ReadToEnd();
                //var program = " print (5, (K := 8,(Cc6Rm := X7 + 124,(XpV := 4,70)))); print (831); print (K); print (pRDPs8, 4, x, 636 + OauJ2); qRLd2 := PIP; pOEsLMnd5 := (print ((print (neiAlR4t, V70umMqx7),7)); print (2623, 7); Nt74z := 10 + 4578; print (61 + sdwQj45); print (vSaC4pYsY); print (66); nL7Nn := JodUh; print ((print (awAcx36L); print (26); print ((mPDH3 := 53,5081)); FV6ZZV6 := 2 + XBC + 58 + 6082; iD0Bh := 6126; print (2); print (6); SisT := U0oh; Ys4r := 7006; lQUI := 3251; print (HyT8),1)),4); vq := 72; print (Ds); HzS7yTX := (K := K7,xwYmM1 + (print (8, 6224),Dw8JTG)); print (PX6FUwS)";
                var parser = new Parser(program);
                
				var ast = parser.Parse();
				Console.WriteLine(ast);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

		}
	}
}
