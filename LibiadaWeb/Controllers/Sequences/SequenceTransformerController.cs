﻿namespace LibiadaWeb.Controllers.Sequences
{
    using System.Collections.Generic;
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
        /// The matter repository.
        /// </summary>
        private readonly MatterRepository matterRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceTransformerController"/> class.
        /// </summary>
        public SequenceTransformerController()
        {
            db = new LibiadaWebEntities();
            dnaSequenceRepository = new DnaSequenceRepository(db);
            commonSequenceRepository = new CommonSequenceRepository(db);
            elementRepository = new ElementRepository(db);
            matterRepository = new MatterRepository(db);
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Index()
        {
            var matterIds = db.DnaSequence.Where(d => d.NotationId == Aliases.Notation.Nucleotide).Select(d => d.MatterId);

            var matters = db.Matter.Where(m => matterIds.Contains(m.Id));

            ViewBag.data = new Dictionary<string, object>
                {
                    { "minimumSelectedMatters", 1 },
                    { "maximumSelectedMatters", int.MaxValue },
                    { "natureId", Aliases.Nature.Genetic }, 
                    { "matters", matterRepository.GetMatterSelectList(matters) }
                };
            return View();
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="matterIds">
        /// The sequence ids.
        /// </param>
        /// <param name="transformType">
        /// The to amino.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        public ActionResult Index(IEnumerable<long> matterIds, string transformType)
        {
            int notationId = transformType.Equals("toAmino") ? Aliases.Notation.AminoAcid : Aliases.Notation.Triplet;

            foreach (var matterId in matterIds)
            {
                var sequenceId = db.CommonSequence.Single(c => c.MatterId == matterId && c.NotationId == Aliases.Notation.Nucleotide).Id;
                Chain sourceChain = commonSequenceRepository.ToLibiadaChain(sequenceId);

                BaseChain transformedChain = transformType.Equals("toAmino")
                                                 ? DnaTransformer.EncodeAmino(sourceChain)
                                                 : DnaTransformer.EncodeTriplets(sourceChain);

                var result = new DnaSequence
                    {
                        MatterId = matterId,
                        NotationId = notationId,
                        FeatureId = Aliases.Feature.FullGenome,
                        PiecePosition = 0
                    };
                long[] alphabet = elementRepository.ToDbElements(transformedChain.Alphabet, notationId, false);
                dnaSequenceRepository.Insert(result, alphabet, transformedChain.Building);
            }

            return RedirectToAction("Index", "CommonSequences");
        }
    }
}
