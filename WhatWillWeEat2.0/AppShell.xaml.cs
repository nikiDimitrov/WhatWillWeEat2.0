namespace WhatWillWeEat2._0
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RecipesListPage), typeof(RecipesListPage));
        }
    }
}
