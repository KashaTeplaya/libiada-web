namespace LibiadaWeb.Models.Repositories.Sequences
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LibiadaCore.Core;
    using LibiadaCore.Core.SimpleTypes;

    /// <summary>
    /// The element repository.
    /// </summary>
    public class ElementRepository : IElementRepository
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LibiadaWebEntities db;

        /// <summary>
        /// The lazy cache.
        /// </summary>
        private Element[] lazyCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementRepository"/> class.
        /// </summary>
        /// <param name="db">
        /// The db.
        /// </param>
        public ElementRepository(LibiadaWebEntities db)
        {
            this.db = db;
        }

        /// <summary>
        /// Gets the cached elements.
        /// </summary>
        private Element[] CachedElements
        {
            get
            {
                if (lazyCache == null)
                {
                    lazyCache = db.Element.Where(e => Aliases.Notation.StaticNotations.Contains(e.NotationId)).ToArray();
                }

                return lazyCache;
            }
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }

        /// <summary>
        /// The elements in db.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet.
        /// </param>
        /// <param name="notationId">
        /// The notation id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ElementsInDb(Alphabet alphabet, int notationId)
        {
            var elements = from IBaseObject element in alphabet select element.ToString();

            int existingElementsCount = db.Element.Count(e => elements.Contains(e.Value) && e.NotationId == notationId);

            return alphabet.Cardinality == existingElementsCount;
        }

        /// <summary>
        /// The to db elements.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet.
        /// </param>
        /// <param name="notationId">
        /// The notation id.
        /// </param>
        /// <param name="createElements">
        /// The create elements.
        /// </param>
        /// <returns>
        /// The <see cref="long[]"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown if alphabet element is not found in db. 
        /// </exception>
        public long[] ToDbElements(Alphabet alphabet, int notationId, bool createElements)
        {
            if (!ElementsInDb(alphabet, notationId))
            {
                if (createElements)
                {
                    CreateLackingElements(alphabet, notationId);
                }
                else
                {
                    throw new Exception("At least one element of alphabet is not found in database.");
                }
            }

            var staticNotation = Aliases.Notation.StaticNotations.Contains(notationId);

            var stringElements = alphabet.Select(element => element.ToString()).ToList();
            
            var elements = staticNotation ?
                            CachedElements.Where(e => e.NotationId == notationId && stringElements.Contains(e.Value)).ToList() :
                            db.Element.Where(e => e.NotationId == notationId && stringElements.Contains(e.Value)).ToList();

            return (from stringElement in stringElements 
                         join element in elements 
                         on stringElement equals element.Value 
                    select element.Id).ToArray();
        }

        /// <summary>
        /// The to libiada alphabet.
        /// </summary>
        /// <param name="elementIds">
        /// The element ids.
        /// </param>
        /// <returns>
        /// The <see cref="Alphabet"/>.
        /// </returns>
        public Alphabet ToLibiadaAlphabet(List<long> elementIds)
        {
            var alphabet = new Alphabet { NullValue.Instance() };
            foreach (long elementId in elementIds)
            {
                Element el = db.Element.Single(e => e.Id == elementId);
                alphabet.Add(new ValueString(el.Value));
            }

            return alphabet;
        }

        /// <summary>
        /// The get elements.
        /// </summary>
        /// <param name="elementIds">
        /// The element ids.
        /// </param>
        /// <returns>
        /// The <see cref="List{Element}"/>.
        /// </returns>
        public List<Element> GetElements(List<long> elementIds)
        {
            var elements = new List<Element>();
            for (int i = 0; i < elementIds.Count(); i++)
            {
                long elementId = elementIds[i];
                elements.Add(db.Element.Single(e => e.Id == elementId));
            }

            return elements;
        }

        /// <summary>
        /// The get select list items.
        /// </summary>
        /// <param name="allElements">
        /// The all elements.
        /// </param>
        /// <param name="selectedElements">
        /// The selected elements.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{SelectListItem}"/>.
        /// </returns>
        public IEnumerable<SelectListItem> GetSelectListItems(
            IEnumerable<Element> allElements,
            IEnumerable<Element> selectedElements)
        {
            HashSet<long> elementIds = selectedElements != null
                                     ? new HashSet<long>(selectedElements.Select(c => c.Id))
                                     : new HashSet<long>();
            if (allElements == null)
            {
                allElements = db.Element;
            }

            var elementsList = new List<SelectListItem>();
            foreach (var element in allElements)
            {
                elementsList.Add(new SelectListItem
                    {
                        Value = element.Id.ToString(),
                        Text = element.Name,
                        Selected = elementIds.Contains(element.Id)
                    });
            }

            return elementsList;
        }

        /// <summary>
        /// The get select list items.
        /// </summary>
        /// <param name="elements">
        /// The elements.
        /// </param>
        /// <returns>
        /// The <see cref="List{SelectListItem}"/>.
        /// </returns>
        public List<SelectListItem> GetSelectListItems(IEnumerable<Element> elements)
        {
            HashSet<long> elementIds = elements != null
                                           ? new HashSet<long>(elements.Select(c => c.Id))
                                           : new HashSet<long>();
            var allElements = db.Element;
            var elementsList = new List<SelectListItem>();
            foreach (var element in allElements)
            {
                elementsList.Add(new SelectListItem
                    {
                        Value = element.Id.ToString(),
                        Text = element.Name,
                        Selected = elementIds.Contains(element.Id)
                    });
            }

            return elementsList;
        }

        /// <summary>
        /// The create lacking elements.
        /// </summary>
        /// <param name="libiadaAlphabet">
        /// The libiada alphabet.
        /// </param>
        /// <param name="notationId">
        /// The notation id.
        /// </param>
        private void CreateLackingElements(Alphabet libiadaAlphabet, int notationId)
        {
            List<string> elements = (from IBaseObject element in libiadaAlphabet select element.ToString()).ToList();

            var existingElements = db.Element.Where(e => elements.Contains(e.Value) && e.NotationId == notationId).Select(e => e.Value);

            var notExistingElements = elements.Where(e => !existingElements.Contains(e)).ToList();

            foreach (var element in notExistingElements)
            {
                var newElement = new Element
                {
                    Value = element,
                    Name = element,
                    NotationId = notationId
                };

                db.Element.Add(newElement);
            }

            db.SaveChanges();
        }
    }
}
