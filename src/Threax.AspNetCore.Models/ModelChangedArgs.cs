using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// A class that can be used to alert that a model is changing.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ModelChangedArgs<TModel>
    {
        /// <summary>
        /// The old value.
        /// </summary>
        public TModel Old { get; set; }

        /// <summary>
        /// The new value.
        /// </summary>
        public TModel New { get; set; }
    }
}
