﻿using System;
using Newtonsoft.Json.Converters;

namespace EmployeeMonthlyPayslipApp.Models
{
    public class InterfaceConverter<TInterface, TConcrete> : CustomCreationConverter<TInterface>
        where TConcrete : TInterface, new()
    {
        public override TInterface Create(Type objectType)
        {
            return new TConcrete();
        }
    }
}