using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist.Classes.Item
{
    internal static class SessaoItem
    {
        public static Item ItemAtual { get; private set; }

        public static void ArmazenarItem(Item item)
        {
            ItemAtual = item;
        }
    }
}
