using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatWillWeEat2._0.Services
{
    public class InvalidIngredientException : Exception
    {
        public InvalidIngredientException(string message) : base(message) { }
    }
}
