﻿namespace LibiadaWeb.Controllers.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Helpers;

    using LibiadaCore.Core;
    using LibiadaCore.Core.Characteristics.Calculators.CongenericCalculators;
    using LibiadaCore.Extensions;

    using LibiadaWeb.Models.Repositories.Sequences;

    using Models.Repositories.Catalogs;

    using Newtonsoft.Json;

    /// <summary>
    /// The congeneric calculation controller.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class CongenericCalculationController : AbstractResultController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LibiadaWebEntities db;

        /// <summary>
        /// The sequence repository.
        /// </summary>
        private readonly CommonSequenceRepository commonSequenceRepository;

        /// <summary>
        /// The characteristic type repository.
        /// </summary>
        private readonly CharacteristicTypeLinkRepository characteristicTypeLinkRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CongenericCalculationController"/> class.
        /// </summary>
        public CongenericCalculationController() : base("Congeneric calculation")
        {
            db = new LibiadaWebEntities();
            commonSequenceRepository = new CommonSequenceRepository(db);
            characteristicTypeLinkRepository = new CharacteristicTypeLinkRepository(db);
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Index()
        {
            var viewDataHelper = new ViewDataHelper(db);
            ViewBag.data = JsonConvert.SerializeObject(viewDataHelper.FillViewData(c => c.CongenericSequenceApplicable, 1, int.MaxValue, "Calculate"));
            return View();
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="matterIds">
        /// The matter ids.
        /// </param>
        /// <param name="characteristicTypeLinkIds">
        /// The characteristic type and link ids.
        /// </param>
        /// <param name="notations">
        /// The notation ids.
        /// </param>
        /// <param name="languages">
        /// The language ids.
        /// </param>
        /// <param name="translators">
        /// The translator ids.
        /// </param>
        /// <param name="sort">
        /// The is sort.
        /// </param>
        /// <param name="theoretical">
        /// The theoretical.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(
            long[] matterIds,
            int[] characteristicTypeLinkIds,
            Notation[] notations,
            Language[] languages,
            Translator?[] translators,
            bool sort,
            bool theoretical)
        {
            return Action(() =>
            {
                var characteristics = new List<List<List<KeyValuePair<int, double>>>>();
                var theoreticalRanks = new List<List<List<double>>>();
                var matterNames = new List<string>();
                var elementNames = new List<List<string>>();
                var characteristicNames = new List<string>();
                var newCharacteristics = new List<LibiadaWeb.CongenericCharacteristic>();

                bool isLiteratureSequence = false;

                // cycle through matters; first level of characteristics array
                for (int w = 0; w < matterIds.Length; w++)
                {
                    long matterId = matterIds[w];
                    matterNames.Add(db.Matter.Single(m => m.Id == matterId).Name);
                    elementNames.Add(new List<string>());
                    characteristics.Add(new List<List<KeyValuePair<int, double>>>());
                    theoreticalRanks.Add(new List<List<double>>());

                    // cycle through characteristics and notations; second level of characteristics array
                    for (int i = 0; i < characteristicTypeLinkIds.Length; i++)
                    {
                        var notation = notations[i];

                        long sequenceId;

                        if (db.Matter.Single(m => m.Id == matterId).Nature == Nature.Literature)
                        {
                            Language language = languages[i];
                            Translator? translator = translators[i];

                            isLiteratureSequence = true;
                            sequenceId = db.LiteratureSequence.Single(l => l.MatterId == matterId
                                                                      && l.Notation == notation
                                                                      && l.Language == language
                                                                      && translator == l.Translator).Id;
                        }
                        else
                        {
                            sequenceId = db.CommonSequence.Single(c => c.MatterId == matterId && c.Notation == notation).Id;
                        }

                        Chain chain = commonSequenceRepository.ToLibiadaChain(sequenceId);
                        chain.FillIntervalManagers();
                        characteristics.Last().Add(new List<KeyValuePair<int, double>>());
                        int characteristicTypeLinkId = characteristicTypeLinkIds[i];

                        string className = characteristicTypeLinkRepository.GetCharacteristicType(characteristicTypeLinkId).ClassName;
                        ICongenericCalculator calculator = CongenericCalculatorsFactory.CreateCongenericCalculator(className);
                        Link link = characteristicTypeLinkRepository.GetLibiadaLink(characteristicTypeLinkId);
                        List<long> sequenceElements = DbHelper.GetElementIds(db, sequenceId);
                        int calculated = db.CongenericCharacteristic.Count(c => c.SequenceId == sequenceId && c.CharacteristicTypeLinkId == characteristicTypeLinkId);
                        if (calculated < chain.Alphabet.Cardinality)
                        {
                            for (int j = 0; j < chain.Alphabet.Cardinality; j++)
                            {
                                long elementId = sequenceElements[j];

                                CongenericChain tempChain = chain.CongenericChain(j);

                                if (!db.CongenericCharacteristic.Any(b => b.SequenceId == sequenceId
                                                                       && b.CharacteristicTypeLinkId == characteristicTypeLinkId
                                                                       && b.ElementId == elementId))
                                {
                                    double value = calculator.Calculate(tempChain, link);
                                    var currentCharacteristic = new LibiadaWeb.CongenericCharacteristic
                                    {
                                        SequenceId = sequenceId,
                                        CharacteristicTypeLinkId = characteristicTypeLinkId,
                                        ElementId = elementId,
                                        Value = value
                                    };

                                    newCharacteristics.Add(currentCharacteristic);
                                }
                            }
                        }

                        // cycle through all alphabet elements; third level of characteristics array
                        for (int d = 0; d < chain.Alphabet.Cardinality; d++)
                        {
                            long elementId = sequenceElements[d];

                            double characteristic = db.CongenericCharacteristic.Single(c => c.SequenceId == sequenceId
                                                                                         && c.CharacteristicTypeLinkId == characteristicTypeLinkId
                                                                                         && c.ElementId == elementId).Value;

                            characteristics.Last().Last().Add(new KeyValuePair<int, double>(d, characteristic));

                            if (i == 0)
                            {
                                elementNames.Last().Add(chain.Alphabet[d].ToString());
                            }
                        }

                        // theoretical frequencies of orlov criterion
                        if (theoretical)
                        {
                            theoreticalRanks[w].Add(new List<double>());
                            ICongenericCalculator countCalculator = CongenericCalculatorsFactory.CreateCongenericCalculator("Count");
                            var counts = new List<int>();
                            for (int f = 0; f < chain.Alphabet.Cardinality; f++)
                            {
                                counts.Add((int)countCalculator.Calculate(chain.CongenericChain(f), Link.NotApplied));
                            }

                            ICongenericCalculator frequencyCalculator = CongenericCalculatorsFactory.CreateCongenericCalculator("Probability");
                            var frequency = new List<double>();
                            for (int f = 0; f < chain.Alphabet.Cardinality; f++)
                            {
                                frequency.Add(frequencyCalculator.Calculate(chain.CongenericChain(f), Link.NotApplied));
                            }

                            double maxFrequency = frequency.Max();
                            double k = 1 / Math.Log(counts.Max());
                            double b = (k / maxFrequency) - 1;
                            int n = 1;
                            double plow = chain.GetLength();
                            double p = k / (b + n);
                            while (p >= (1 / plow))
                            {
                                theoreticalRanks.Last().Last().Add(p);
                                n++;
                                p = k / (b + n);
                            }
                        }
                    }
                }

                db.CongenericCharacteristic.AddRange(newCharacteristics);
                db.SaveChanges();

                // characteristics names
                for (int k = 0; k < characteristicTypeLinkIds.Length; k++)
                {
                    string characteristicType = characteristicTypeLinkRepository.GetCharacteristicName(characteristicTypeLinkIds[k], notations[k]);
                    if (isLiteratureSequence)
                    {
                        Language language = languages[k];
                        characteristicNames.Add(characteristicType + " " + language.GetDisplayValue());
                    }
                    else
                    {
                        characteristicNames.Add(characteristicType);
                    }
                }

                // rank sorting
                if (sort)
                {
                    for (int f = 0; f < matterIds.Length; f++)
                    {
                        for (int p = 0; p < characteristics[f].Count; p++)
                        {
                            SortKeyValuePairList(characteristics[f][p]);
                        }
                    }
                }

                var characteristicsList = new List<SelectListItem>();
                for (int i = 0; i < characteristicNames.Count; i++)
                {
                    characteristicsList.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = characteristicNames[i],
                        Selected = false
                    });
                }

                return new Dictionary<string, object>
                {
                    { "characteristics", characteristics },
                    { "matterNames", matterNames },
                    { "elementNames", elementNames },
                    { "characteristicNames", characteristicNames },
                    { "matterIds", matterIds },
                    { "theoreticalRanks", theoreticalRanks },
                    { "characteristicsList", characteristicsList }
                };
            });
        }

        /// <summary>
        /// The sort key value pair list.
        /// </summary>
        /// <param name="arrayForSort">
        /// The array for sort.
        /// </param>
        private void SortKeyValuePairList(List<KeyValuePair<int, double>> arrayForSort)
        {
            arrayForSort.Sort((firstPair, nextPair) => nextPair.Value.CompareTo(firstPair.Value));
        }
    }
}
