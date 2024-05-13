using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist.Classes.Abas
{
    public class SessaoAba
    {
        public static Guia AbaAtual { get; set; }

        public static void ArmazenarAba(Guia guia)
        {
            AbaAtual = guia;
        }

        public static void LimparAba()
        {
            AbaAtual = null;
        }
    }
}
