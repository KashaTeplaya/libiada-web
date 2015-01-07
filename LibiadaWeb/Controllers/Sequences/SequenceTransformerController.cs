﻿namespace LibiadaWeb.Controllers.Sequences
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using LibiadaCore.Core;
    using LibiadaCore.Misc.DataTransformers;
    using LibiadaWeb.Models;
    using LibiadaWeb.Models.Repositories.Sequences;

    /// <summary>
    /// The dna transformation controller.
    /// </summary>
    public class SequenceTransformerController : Controller
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LibiadaWebEntities db;

        /// <summary>
        /// The dna sequence repository.
        /// </summary>
        private readonly DnaSequenceRepository dnaSequenceRepository;

        /// <summary>
        /// The sequence repository.
        /// </summary>
        private readonly CommonSequenceRepository commonSequenceRepository;

        /// <summary>
        /// The element repository.
        /// </summary>
        private readonly ElementRepository elementRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceTransformerController"/> class.
        /// </summary>
        public SequenceTransformerController()
        {
            db = new LibiadaWebEntities();
            dnaSequenceRepository = new DnaSequenceRepository(db);
            commonSequenceRepository = new CommonSequenceRepository(db);
            elementRepository = new ElementRepository(db);
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Index()
        {
            var sequences = db.DnaSequence.Where(d => d.NotationId == Aliases.Notation.Nucleotide).Include("matter");
            ViewBag.sequences = sequences.ToList();
            ViewBag.sequencesList = dnaSequenceRepository.GetSelectListItems(sequences, null);
            return View();
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="sequenceIds">
        /// The sequence ids.
        /// </param>
        /// <param name="toAmino">
        /// The to amino.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        public ActionResult Index(IEnumerable<long> sequenceIds, bool toAmino)
        {
            int notationId = toAmino ? Aliases.Notation.AminoAcid : Aliases.Notation.Triplet;

            foreach (var sequenceId in sequenceIds)
            {
                CommonSequence dataBaseSequence = db.CommonSequence.Single(c => c.Id == sequenceId);
                Chain sourceChain = commonSequenceRepository.ToLibiadaChain(sequenceId);

                BaseChain transformedChain = toAmino
                                                 ? DnaTransformer.Encode(sourceChain)
                                                 : DnaTransformer.EncodeTriplets(sourceChain);

                var result = new DnaSequence
                    {
                        MatterId = dataBaseSequence.MatterId,
                        NotationId = notationId,
                        PieceTypeId = Aliases.PieceType.FullGenome,
                        PiecePosition = 0
                    };
                long[] alphabet = elementRepository.ToDbElements(transformedChain.Alphabet, notationId, false);
                dnaSequenceRepository.Insert(result, alphabet, transformedChain.Building);
            }

            return RedirectToAction("Index", "CommonSequence");
        }
    }
}
