﻿using System;

namespace UpBlazor.Domain.Models.Normalized
{
    public class NormalizedSavingsPlan
    {
        public Guid SavingsPlanId { get; set; }
        public decimal Amount { get; set; }
    }
}