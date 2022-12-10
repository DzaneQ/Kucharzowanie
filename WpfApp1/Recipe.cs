using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfApp1
{
    public class RecipeData // jeden JSON
    {
        public string title;
        public IList<string> tags;
        public DateTime date;
        private string Type;
        public string type
        {
            get
            {
                return Type.Substring(0, 5);
            }
            set
            {
                Type = value;
            }
        }
        public string content;
    }
    //public class RecipeGroup
    //{
    //    public RecipeData[] recipeData;

    //    public RecipeData this[int index]
    //    {
    //        get
    //        {
    //            return recipeData[index];
    //        }

    //        set
    //        {
    //            recipeData[index] = value;
    //        }
    //    }

    //    internal void Add(RecipeGroup recipeGroup)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
