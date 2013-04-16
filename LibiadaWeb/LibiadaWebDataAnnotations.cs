﻿using System;
using System.ComponentModel.DataAnnotations;

namespace LibiadaWeb
{
    [MetadataType(typeof (CatalogTableDataAnnotations))]
    public partial class catalog_table
    {
    }

    public class CatalogTableDataAnnotations
    {
        [Display(Name = "Название")]
        public string name { get; set; }

        [Display(Name = "Описание")]
        public string description { get; set; }
    }

    [MetadataType(typeof (AccidentalDataAnnotations))]
    public partial class accidental
    {
    }

    public class AccidentalDataAnnotations : CatalogTableDataAnnotations
    {
    }

    [MetadataType(typeof (AlphabetDataAnnotations))]
    public partial class alphabet
    {
    }

    public class AlphabetDataAnnotations
    {
        [Display(Name = "Принадлежит цепочке")]
        public long chain_id { get; set; }

        [Display(Name = "Содержит элемент")]
        public int element_id { get; set; }

        [Display(Name = "Номер элемента в алфавите")]
        public int number { get; set; }
    }

    [MetadataType(typeof (BinaryCharacteristicDataAnnotations))]
    public partial class binary_characteristic
    {
    }

    public class BinaryCharacteristicDataAnnotations : CharacteristicDataAnnotations
    {
        [Display(Name = "Первый элемент")]
        public int first_element_id { get; set; }

        [Display(Name = "Второй элемент")]
        public int second_element_id { get; set; }
    }

    [MetadataType(typeof (ChainDataAnnotations))]
    public partial class chain
    {
    }

    public class ChainDataAnnotations
    {
        [Display(Name = "Форма записи")]
        public long notation_id { get; set; }

        [Display(Name = "Дата создания")]
        public DateTimeOffset creation_date { get; set; }

        [Display(Name = "Принадлежит объекту исследования")]
        public long matter_id { get; set; }

        [Display(Name = "Цепочка разнородная")]
        public bool dissimilar { get; set; }

        [Display(Name = "Тип фрагмента цепочки")]
        public int piece_type_id { get; set; }

        [Display(Name = "Позиция с которой начинается цепочка")]
        public long piece_position { get; set; }
    }

    [MetadataType(typeof (CharacteristicDataAnnotations))]
    public partial class characteristic
    {
    }

    public class CharacteristicDataAnnotations
    {
        [Display(Name = "Принадлежит цепочке")]
        public long chain_id { get; set; }

        [Display(Name = "Тип характеристики")]
        public int characteristic_type_id { get; set; }

        [Display(Name = "Значение")]
        public double value { get; set; }

        [Display(Name = "Значение в виде строки")]
        public string value_string { get; set; }

        [Display(Name = "Привязка")]
        public int link_up_id { get; set; }

        [Display(Name = "Дата создания")]
        public DateTimeOffset creation_date { get; set; }
    }

    [MetadataType(typeof (CharacteristicApplicabilityDataAnnotations))]
    public partial class characteristic_applicability
    {
    }

    public class CharacteristicApplicabilityDataAnnotations : CatalogTableDataAnnotations
    {
    }

    [MetadataType(typeof (CharacteristicGroupDataAnnotations))]
    public partial class characteristic_group
    {
    }

    public class CharacteristicGroupDataAnnotations : CatalogTableDataAnnotations
    {
    }

    [MetadataType(typeof (CharacteristicTypeDataAnnotations))]
    public partial class characteristic_type
    {
    }

    public class CharacteristicTypeDataAnnotations : CatalogTableDataAnnotations
    {
        [Display(Name = "Группа характеристик")]
        public int characteristic_group_id { get; set; }

        [Display(Name = "Название класса")]
        public string class_name { get; set; }

        [Display(Name = "Применимость характеристики")]
        public int characteristic_applicability_id { get; set; }
    }

    [MetadataType(typeof (DnaChainDataAnnotations))]
    public partial class dna_chain
    {
    }

    public class DnaChainDataAnnotations : ChainDataAnnotations
    {
        [Display(Name = "Заголовок FASTA файла")]
        public string fasta_header { get; set; }
    }

    [MetadataType(typeof (ElementDataAnnotations))]
    public partial class element
    {
    }

    public class ElementDataAnnotations : CatalogTableDataAnnotations
    {
        [Display(Name = "Значение")]
        public string value { get; set; }

        [Display(Name = "Форма записи")]
        public long notation_id { get; set; }

        [Display(Name = "Дата создания")]
        public DateTimeOffset creation_date { get; set; }
    }

    [MetadataType(typeof (FmotivDataAnnotations))]
    public partial class fmotiv
    {
    }

    public class FmotivDataAnnotations : ChainDataAnnotations
    {
        [Display(Name = "Значение")]
        public string value { get; set; }

        [Display(Name = "Название")]
        public string name { get; set; }

        [Display(Name = "Описание")]
        public string description { get; set; }

        [Display(Name = "Тип ф-мотива")]
        public int fmotiv_type_id { get; set; }
    }

    [MetadataType(typeof (FmotivTypeDataAnnotations))]
    public partial class fmotiv_type
    {
    }

    public class FmotivTypeDataAnnotations : CatalogTableDataAnnotations
    {
    }

    [MetadataType(typeof (HomogeneousCharacteristicDataAnnotations))]
    public partial class homogeneous_characteristic
    {
    }

    public class HomogeneousCharacteristicDataAnnotations : CharacteristicDataAnnotations
    {
        [Display(Name = "Элемент")]
        public int element_id { get; set; }
    }

    [MetadataType(typeof (InstrumentDataAnnotations))]
    public partial class instrument
    {
    }

    public class InstrumentDataAnnotations : CatalogTableDataAnnotations
    {
    }

    [MetadataType(typeof (LanguageDataAnnotations))]
    public partial class language
    {
    }

    public class LanguageDataAnnotations : CatalogTableDataAnnotations
    {
    }

    [MetadataType(typeof (LinkUpDataAnnotations))]
    public partial class link_up
    {
    }

    public class LinkUpDataAnnotations : CatalogTableDataAnnotations
    {
    }

    [MetadataType(typeof (LiteratureChainDataAnnotations))]
    public partial class literature_chain
    {
    }

    public class LiteratureChainDataAnnotations : ChainDataAnnotations
    {
        [Display(Name = "Язык")]
        public int language_id { get; set; }

        [Display(Name = "Оригинал")]
        public bool original { get; set; }
    }

    [MetadataType(typeof (MatterDataAnnotations))]
    public partial class matter
    {
    }

    public class MatterDataAnnotations : CatalogTableDataAnnotations
    {
        [Display(Name = "Природа")]
        public int nature_id { get; set; }

        [Display(Name = "Сторонняя БД")]
        public int remote_db_id { get; set; }

        [Display(Name = "id в сторонней БД")]
        public string id_in_remote_db { get; set; }
    }

    [MetadataType(typeof (MeasureDataAnnotations))]
    public partial class measure
    {
    }

    public class MeasureDataAnnotations : ChainDataAnnotations
    {
        [Display(Name = "Значение")]
        public string value { get; set; }

        [Display(Name = "Название")]
        public string name { get; set; }

        [Display(Name = "Описание")]
        public string description { get; set; }

        [Display(Name = "? я чото п")]
        public int beats { get; set; }

        [Display(Name = "? я чото п")]
        public int beatbase { get; set; }

        [Display(Name = "? я чото п")]
        public int ticks_per_beat { get; set; }

        [Display(Name = "? я чото п")]
        public int fifths { get; set; }
    }

    [MetadataType(typeof (MusicChainDataAnnotations))]
    public partial class music_chain
    {
    }

    public class MusicChainDataAnnotations : ChainDataAnnotations
    {
        [Display(Name = "Инструмент")]
        public int instrument_id { get; set; }
    }

    [MetadataType(typeof (NatureDataAnnotations))]
    public partial class nature
    {
    }

    public class NatureDataAnnotations : CatalogTableDataAnnotations
    {
    }

    [MetadataType(typeof (NotationDataAnnotations))]
    public partial class notation
    {
    }

    public class NotationDataAnnotations : CatalogTableDataAnnotations
    {
        [Display(Name = "Природа")]
        public int nature_id { get; set; }
    }

    [MetadataType(typeof (NoteDataAnnotations))]
    public partial class note
    {
    }

    public class NoteDataAnnotations : ElementDataAnnotations
    {
        [Display(Name = "Числитель в дроби доли")]
        public int numerator { get; set; }

        [Display(Name = "Знаменатель в дроби доли")]
        public int denominator { get; set; }

        [Display(Name = "Количество МИДИ тиков в доле")]
        public int ticks { get; set; }

        [Display(Name = "Оригинальный числитель в дроби доли")]
        public int onumerator { get; set; }

        [Display(Name = "Оригинальный знаменатель в дроби доли")]
        public int odenominator { get; set; }

        [Display(Name = "Триоль")]
        public bool triplet { get; set; }

        [Display(Name = "Приоритет")]
        public int priority { get; set; }

        [Display(Name = "Лига")]
        public int tie_id { get; set; }
    }

    [MetadataType(typeof (NoteSymbolDataAnnotations))]
    public partial class note_symbol
    {
    }

    public class NoteSymbolDataAnnotations : CatalogTableDataAnnotations
    {
    }

    [MetadataType(typeof (PieceTypeDataAnnotations))]
    public partial class piece_type
    {
    }

    public class PieceTypeDataAnnotations : NotationDataAnnotations
    {
    }

    [MetadataType(typeof (PitchDataAnnotations))]
    public partial class pitch
    {
    }

    public class PitchDataAnnotations
    {
        [Display(Name = "Номер октавы")]
        public int octave { get; set; }

        [Display(Name = "Уникальный номер ноты по миди стандарту")]
        public int midinumber { get; set; }

        [Display(Name = "Инструмент")]
        public int instrument_id { get; set; }

        [Display(Name = "Принадлежит ноте")]
        public long note_id { get; set; }

        [Display(Name = "Знак альтерации")]
        public int accidental_id { get; set; }

        [Display(Name = "Обозначение ноты")]
        public int note_symbol_id { get; set; }
    }

    [MetadataType(typeof (RemoteDbDataAnnotations))]
    public partial class remote_db
    {
    }

    public class RemoteDbDataAnnotations : CatalogTableDataAnnotations
    {
        [Display(Name = "url адрес")]
        public string url { get; set; }
    }

    [MetadataType(typeof (TieDataAnnotations))]
    public partial class tie
    {
    }

    public class TieDataAnnotations : CatalogTableDataAnnotations
    {
    }
}