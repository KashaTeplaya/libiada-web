﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibiadaWeb.Models.CalculatorsData
{
    public class OrderTransformationData
    {
        public OrderTransformationResult[] ResultTransformation { get; set; }

        public int UniqueFinalOrdersCount { get; set; }
    }
}