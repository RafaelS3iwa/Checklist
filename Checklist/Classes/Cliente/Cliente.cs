using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist
{
    public class Cliente
    {
        protected int _id; 
        protected string _nome;

        public Cliente(int id, string nome)
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
