// <copyright file="DragDropService.cs" company="SELF">
// Copyright (c) SELF. All rights reserved.
// </copyright>

namespace BlazorUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// DragDropService
    /// </summary>
    public class DragDropService<T>
    {
        /// <summary>
        /// ActiveItem
        /// </summary>
        public T ActiveItem { get; set; }

        /// <summary>
        /// DragtargetItem
        /// </summary>
        public T DragtargetItem { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// ActiveSpacerId
        /// </summary>
        public int? ActiveSpacerId { get; set; }

        /// <summary>
        /// ShouldRender
        /// </summary>
        public bool ShouldRender { get; set; } = true;

        /// <summary>
        /// StateHasChanged
        /// </summary>
        public EventHandler StateHasChanged { get; set; }

        public void Reset()
        {
            this.ShouldRender = true;
            this.ActiveItem = default;
            this.ActiveSpacerId = null;
            this.Items = null;
            this.DragtargetItem = default;
            this.StateHasChanged.Invoke(this, EventArgs.Empty);
        }

    }
}
