namespace WhatWillWeEat2._0.Services
{
    public class InvalidIngredientException : Exception
    {
        public InvalidIngredientException(string message) : base(message) { }
    }
}
