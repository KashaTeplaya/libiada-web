﻿namespace LibiadaWeb.Models.Repositories.Catalogs
{
    using System;
    using System.Linq;

    using LibiadaWeb;

    using Attribute = LibiadaWeb.Attribute;

    /// <summary>
    /// The attribute repository.
    /// </summary>
    public class AttributeRepository : IAttributeRepository
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LibiadaWebEntities db;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeRepository"/> class.
        /// </summary>
        /// <param name="db">
        /// The db.
        /// </param>
        public AttributeRepository(LibiadaWebEntities db)
        {
            this.db = db;
        }

        /// <summary>
        /// Gets attribute by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="Attribute"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown if attribute with given name is not found in db.
        /// </exception>
        public Attribute GetAttributeByName(string name)
        {
            if (!db.Attribute.Any(a => a.Name == name))
            {
                throw new Exception("Unknown attribute: " + name);
            }

            return db.Attribute.Single(a => a.Name == name);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }
    }
}