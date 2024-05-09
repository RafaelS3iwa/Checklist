using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist.Classes
{
    internal static class SessaoId
    {
        public static Cliente IdAtual { get; private set;}

        public static void ArmazenarId(Cliente cliente)
        {
            IdAtual = cliente;
        }

        public static void LimparId()
        {
            IdAtual = null;
        }
    }
}
