using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist.Classes.Abas
{
    public class Guia
    {
        protected int _id;
        protected string _nome;

        public Guia(int id, string nome)
        {
            _id = id;
            _nome = nome;
        }

        public int getId()
        {
            return _id;
        }

        public string getNome()
        {
            return _nome;
        }
    }
}
