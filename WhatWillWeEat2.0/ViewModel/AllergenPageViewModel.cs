using StartUp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatWillWeEat2._0.ViewModel
{
    public class AllergenPageViewModel
    {
        private Allergen currentAllergen;
        private Allergen oldAllergen;

        public AllergenPageViewModel(Allergen allergen)
        {
            currentAllergen = allergen;
            oldAllergen = allergen.Clone();
        }
    }
}
