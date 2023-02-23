using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Razor.Helpers.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class PresentationAttribute : System.Attribute
    {
        // This is a positional argument
        public PresentationAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the datatype to use while in input.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets the class of the <i> element used for icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the position to use for the icon element.
        /// Decides also whether or not an icon should be displayed.
        /// </summary>
        public IconPosition IconPosition { get; set; } = IconPosition.None;


        /// <summary>
        /// Gets or sets whether or not to autofocus the field.
        /// </summary>
        public bool Autofocus { get; set; } = false;
    }

    /// <summary>
    /// IconPosition enum.
    /// Used to represents the position of the icon.
    /// </summary>
    public enum IconPosition
    {
        None,
        Start,
        End
    }
}
