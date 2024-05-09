using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist.Classes.Item
{
    public class Item
    {
        protected int _id;
        protected string _nome;
        protected string _tipo;
        protected string _descricao;
        protected DateTime _criacao;
        protected DateTime _atualizacao;
        protected int _situacao;

        public Item(int id, string nome, string tipo, string descricao, DateTime criacao, DateTime atualizacao, int situacao)
        {
            _id = id;
            _nome = nome;
            _tipo = tipo;
            _descricao = descricao;
            _criacao = criacao;
            _atualizacao = atualizacao;
            _situacao = situacao;
        }

        public int getId()
        {
            return _id;
        }

        public string getnome()
        {
            return _nome;
        }

        public string getTipo()
        {
            return _tipo;
        }

        public string getDescricao()
        {
            return _descricao;
        }

        public DateTime getCriacao()
        {
            return _criacao;
        }

        public DateTime getAtualizacao()
        {
            return _atualizacao;
        }

        public int getSituacao()
        {
            return _situacao;
        }
    }
}
