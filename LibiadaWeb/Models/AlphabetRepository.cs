using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using LibiadaCore.Classes.Root.SimpleTypes;
using LibiadaCore.Classes.TheoryOfSet;
using LibiadaWeb;

namespace LibiadaWeb.Models
{ 
    public class AlphabetRepository : IAlphabetRepository
    {
        private readonly LibiadaWebEntities db;

        public AlphabetRepository(LibiadaWebEntities db)
        {
            this.db = db;
        }

        public IQueryable<alphabet> All
        {
            get { return db.alphabet; }
        }

        public IQueryable<alphabet> AllIncluding(params Expression<Func<alphabet, object>>[] includeProperties)
        {
            IQueryable<alphabet> query = db.alphabet;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public alphabet Find(long id)
        {
            return db.alphabet.Single(x => x.id == id);
        }

        public void InsertOrUpdate(alphabet alphabet)
        {
            if (alphabet.id == default(long)) {
                // New entity
                db.alphabet.AddObject(alphabet);
            } else {
                // Existing entity
                db.alphabet.Attach(alphabet);
                db.ObjectStateManager.ChangeObjectState(alphabet, EntityState.Modified);
            }
        }

        public void Delete(long id)
        {
            var alphabet = Find(id);
            db.alphabet.DeleteObject(alphabet);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose() 
        {
            db.Dispose();
        }

        //TODO: ������� � ������� �������� id ����� ����� ���� ������� ����������� �������� � ����������� ���� ������ ��������
        public Alphabet FromDbAlphabetToLibiadaAlphabet(IEnumerable<alphabet> dbAlphabet)
        {
            IEnumerable<element> dbElements = dbAlphabet.Select(a => a.element);

            Alphabet alphabet = new Alphabet();
            alphabet.Add(NullValue.Instance());
            foreach (var element in dbElements)
            {
                alphabet.Add(new ValueString(element.value));
            }
            return alphabet;
        }

        public IEnumerable<alphabet> FromLibiadaAlphabetToDbAlphabet(Alphabet libiadaAlphabet, int notationId)
        {
            List<alphabet> dbAlphabet = new List<alphabet>();
            for (int j = 0; j < libiadaAlphabet.Power; j++)
            {
                dbAlphabet.Add(new alphabet());
                dbAlphabet[j].number = j + 1;
                String strElem = libiadaAlphabet[j].ToString();
                if (!db.element.Any(e => e.notation_id == notationId && e.value.Equals(strElem)))
                {
                    throw new Exception("������� " + strElem + " �� ������ � ��.");
                }
                dbAlphabet[j].element = db.element.Single(e => e.notation_id == notationId && e.value.Equals(strElem));

                db.alphabet.AddObject(dbAlphabet[j]);
            }
            db.SaveChanges();

            return dbAlphabet;
        }
    }
}