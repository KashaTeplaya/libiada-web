﻿namespace LibiadaWeb.Controllers.Sequences
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using LibiadaCore.Core;
    using LibiadaCore.DataTransformers;

    using LibiadaWeb.Helpers;
    using LibiadaWeb.Models.Repositories.Sequences;

    using Newtonsoft.Json;

    /// <summary>
    /// The DNA transformation controller.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class SequenceTransformerController : Controller
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LibiadaWebEntities db;

        /// <summary>
        /// The DNA sequence repository.
        /// </summary>
        private readonly GeneticSequenceRepository dnaSequenceRepository;

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
            dnaSequenceRepository = new GeneticSequenceRepository(db);
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
            var matterIds = db.DnaSequence.Where(d => d.Notation == Notation.Nucleotides).Select(d => d.MatterId).ToArray();

            var viewDataHelper = new ViewDataHelper(db);
            var data = viewDataHelper.FillViewData(1, int.MaxValue, m => matterIds.Contains(m.Id), "Transform");
            data.Add("nature", (byte)Nature.Genetic);
            ViewBag.data = JsonConvert.SerializeObject(data);
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
        [ValidateAntiForgeryToken]
        public ActionResult Index(IEnumerable<long> matterIds, string transformType)
        {
            Notation notation = transformType.Equals("toAmino") ? Notation.AminoAcids : Notation.Triplets;

            foreach (var matterId in matterIds)
            {
                var sequenceId = db.CommonSequence.Single(c => c.MatterId == matterId && c.Notation == Notation.Nucleotides).Id;
                Chain sourceChain = commonSequenceRepository.GetLibiadaChain(sequenceId);

                BaseChain transformedChain = transformType.Equals("toAmino")
                                                 ? DnaTransformer.EncodeAmino(sourceChain)
                                                 : DnaTransformer.EncodeTriplets(sourceChain);

                var result = new DnaSequence
                    {
                        MatterId = matterId,
                        Notation = notation
                    };

                long[] alphabet = elementRepository.ToDbElements(transformedChain.Alphabet, notation, false);
                dnaSequenceRepository.Insert(result, alphabet, transformedChain.Building);
            }

            return RedirectToAction("Index", "CommonSequences");
        }
    }
}
