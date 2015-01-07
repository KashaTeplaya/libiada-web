namespace LibiadaWeb.Models.Repositories.Sequences
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LibiadaCore.Core;
    using LibiadaCore.Core.SimpleTypes;
    using Npgsql;
    using NpgsqlTypes;

    /// <summary>
    /// The dna chain repository.
    /// </summary>
    public class DnaSequenceRepository : CommonSequenceImporter, IDnaSequenceRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DnaSequenceRepository"/> class.
        /// </summary>
        /// <param name="db">
        /// The db.
        /// </param>
        public DnaSequenceRepository(LibiadaWebEntities db) : base(db)
        {
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="commonSequence">
        /// The chain.
        /// </param>
        /// <param name="fastaHeader">
        /// The fasta header.
        /// </param>
        /// <param name="webApiId">
        /// The web api id.
        /// </param>
        /// <param name="productId">
        /// The product id.
        /// </param>
        /// <param name="complement">
        /// The complement.
        /// </param>
        /// <param name="partial">
        /// The partial.
        /// </param>
        /// <param name="alphabet">
        /// The alphabet.
        /// </param>
        /// <param name="building">
        /// The building.
        /// </param>
        public void Insert(
            CommonSequence commonSequence, 
            string fastaHeader, 
            int? webApiId, 
            int? productId, 
            bool complement, 
            bool partial, 
            long[] alphabet, 
            int[] building)
        {
            var parameters = FillParams(commonSequence, alphabet, building);
            parameters.Add(new NpgsqlParameter
            {
                ParameterName = "fasta_header", 
                NpgsqlDbType = NpgsqlDbType.Varchar, 
                Value = fastaHeader
            });
            parameters.Add(new NpgsqlParameter
            {
                ParameterName = "web_api_id", 
                NpgsqlDbType = NpgsqlDbType.Integer, 
                Value = webApiId
            });
            parameters.Add(new NpgsqlParameter
            {
                ParameterName = "product_id", 
                NpgsqlDbType = NpgsqlDbType.Integer, 
                Value = productId
            });
            parameters.Add(new NpgsqlParameter
            {
                ParameterName = "partial", 
                NpgsqlDbType = NpgsqlDbType.Boolean, 
                Value = partial
            });
            parameters.Add(new NpgsqlParameter
            {
                ParameterName = "complement", 
                NpgsqlDbType = NpgsqlDbType.Boolean, 
                Value = complement
            });

            const string Query = @"INSERT INTO dna_chain (
                                        id, 
                                        notation_id,
                                        matter_id, 
                                        piece_type_id, 
                                        piece_position, 
                                        fasta_header, 
                                        alphabet, 
                                        building, 
                                        remote_id, 
                                        remote_db_id, 
                                        web_api_id,
                                        product_id,
                                        partial,
                                        complement
                                    ) VALUES (
                                        @id, 
                                        @notation_id,
                                        @matter_id,
                                        @piece_type_id, 
                                        @piece_position, 
                                        @fasta_header, 
                                        @alphabet, 
                                        @building, 
                                        @remote_id, 
                                        @remote_db_id, 
                                        @web_api_id,
                                        @product_id,
                                        @partial,
                                        @complement
                                    );";
            db.Database.ExecuteSqlCommand(Query, parameters.ToArray());
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="sequence">
        /// The chain.
        /// </param>
        /// <param name="alphabet">
        /// The alphabet.
        /// </param>
        /// <param name="building">
        /// The building.
        /// </param>
        public void Insert(DnaSequence sequence, long[] alphabet, int[] building)
        {
            Insert(ToCommonSequence(sequence), sequence.FastaHeader, sequence.WebApiId, null, false, false, alphabet, building);
        }

        /// <summary>
        /// The to chain.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see cref="CommonSequence"/>.
        /// </returns>
        public CommonSequence ToCommonSequence(DnaSequence source)
        {
            return new CommonSequence
            {
                Id = source.Id,
                NotationId = source.NotationId, 
                MatterId = source.MatterId, 
                PieceTypeId = source.PieceTypeId, 
                PiecePosition = source.PiecePosition
            };
        }

        /// <summary>
        /// The get select list items.
        /// </summary>
        /// <param name="sequences">
        /// The chains.
        /// </param>
        /// <returns>
        /// The <see cref="List{Object}"/>.
        /// </returns>
        public List<SelectListItem> GetSelectListItems(IEnumerable<DnaSequence> sequences)
        {
            return GetSelectListItems(db.DnaSequence.ToList(), sequences);
        }

        /// <summary>
        /// The get select list items.
        /// </summary>
        /// <param name="allSequences">
        /// The all chains.
        /// </param>
        /// <param name="selectedSequences">
        /// The selected chain.
        /// </param>
        /// <returns>
        /// The <see cref="List{Object}"/>.
        /// </returns>
        public List<SelectListItem> GetSelectListItems(
            IEnumerable<DnaSequence> allSequences,
            IEnumerable<DnaSequence> selectedSequences)
        {
            HashSet<long> chainIds = selectedSequences != null
                ? new HashSet<long>(selectedSequences.Select(c => c.Id))
                : new HashSet<long>();
            if (allSequences == null)
            {
                allSequences = db.DnaSequence.Include("matter");
            }

            var chainsList = new List<SelectListItem>();
            foreach (var sequence in allSequences)
            {
                chainsList.Add(new SelectListItem
                {
                    Value = sequence.Id.ToString(), 
                    Text = sequence.Matter.Name, 
                    Selected = chainIds.Contains(sequence.Id)
                });
            }

            return chainsList;
        }

        /// <summary>
        /// The create complement alphabet.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet.
        /// </param>
        /// <returns>
        /// The <see cref="Alphabet"/>.
        /// </returns>
        public Alphabet CreateComplementAlphabet(Alphabet alphabet)
        {
            var newAlphabet = new Alphabet { NullValue.Instance() };

            for (int i = 0; i < alphabet.Cardinality; i++)
            {
                newAlphabet.Add(GetComplementElement(alphabet[i]));
            }

            return newAlphabet;
        }

        /// <summary>
        /// The get complement element.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see cref="ValueString"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if any element in chain is not recognized as nucleotide.
        /// </exception>
        public ValueString GetComplementElement(IBaseObject source)
        {
            switch (source.ToString())
            {
                case "A":
                case "a":
                    return new ValueString('T');
                case "C":
                case "c":
                    return new ValueString('G');
                case "G":
                case "g":
                    return new ValueString('C');
                case "T":
                case "t":
                    return new ValueString('A');
                default:
                    throw new ArgumentException("Unknown nucleotide.", "source");
            }
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