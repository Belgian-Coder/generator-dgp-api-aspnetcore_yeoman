using System.ComponentModel.DataAnnotations;

namespace StarterKit.Api.Models.Examples
{
    /// <summary>
    /// XML comments added to classes will be visible in Swagger documentation
    /// These are necessary due to official guidelines
    /// </summary>
    public class Example : ModelBase
    {
        /// <summary>
        /// XML comments added to properties will be visible in Swagger documentation
        /// These are necessary due to official guidelines
        /// </summary>
        /// <example>My name</example>
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        public string Name { get; set; }
    }
}
